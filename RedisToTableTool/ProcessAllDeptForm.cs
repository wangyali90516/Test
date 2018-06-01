using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using J.Base.Lib;
using JinYinMao.Business.Assets.Request;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using RedisToTableTool.Model;

namespace RedisToTableTool
{
    public partial class ProcessAllDeptForm : Form
    {
        private readonly HttpClient httpClient;
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;

        private bool IsCompleted = false;

        public ProcessAllDeptForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            CheckForIllegalCrossThreadCalls = false;
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromSeconds(180);
            this.InitializeComponent();
        }

        /// <summary>
        ///     批量执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_BatchProcess_Click(object sender, EventArgs e)
        {
            try
            {
                //获取所有的rowkeys
                List<string> rowKeys = File.ReadAllLines("AllRowKeys.txt").ToList();
                if (rowKeys.Count == 0)
                {
                    MessageBox.Show("没有获取到需要处理的数据");
                    return;
                }
                //获取所有执行的rowkeys
                //开始执行
                this.btn_StartProcess.Enabled = false;
                this.btn_BatchProcess.Enabled = false;
                this.txb_hasProcessNums.Text = "0";
                this.txb_SuccessNums.Text = "0";
                this.txb_ProcessNums.Text = "0";
                this.lbl_showmsg.Text = "正在准备数据,请稍等.......";
                //一并拉出所有的数据
                CloudTable deptTable = this.tableClient.GetTableReference("AssetBatchBookCreditAssignments");
                string partitionKey = "BatchBookCreditAssignment";
                TableQuery<BatchBookCreditInputDto> queryDeptInfo = new TableQuery<BatchBookCreditInputDto>()
                    .Where($"PartitionKey eq '{partitionKey}'");
                List<BatchBookCreditInputDto> creditInfos = deptTable.ExecuteQuery(queryDeptInfo).Where(x => rowKeys.Contains(x.RowKey)).ToList();
                this.lbl_showmsg.Text = "数据获取完成,正准备开始执行.......";
                foreach (BatchBookCreditInputDto batchBookCreditInputDto in creditInfos)
                {
                    this.lbl_showmsg.Text = $"正在开始拉取Rowkey:{batchBookCreditInputDto.RowKey}的数据......";
                    //开始执行
                    this.txb_RansomOrderId.Text = batchBookCreditInputDto.RowKey;
                    List<SubInvestOrderInputDto> listDeptsimpleInfos = batchBookCreditInputDto.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>();
                    NotifyBatchCreditRequest requeBatch = this.MapToConfirmDebt(batchBookCreditInputDto, listDeptsimpleInfos);
                    bool batchOrderSearch = await this.GetBatchordersearch(batchBookCreditInputDto.RowKey);
                    bool isReturn = false;
                    if (!batchOrderSearch)
                    {
                        //通知银行
                        for (int i = 0; i < 5; i++)
                        {
                            bool sendDebtForBankModelResult = await this.SendDebtForBank(new SendDebtForBankModel
                            {
                                Currency = batchBookCreditInputDto.Currency,
                                OrderId = batchBookCreditInputDto.OrderId,
                                Remark = batchBookCreditInputDto.Remark,
                                Status = batchBookCreditInputDto.Status,
                                SubInvestOrderList = listDeptsimpleInfos,
                                TotalAmount = batchBookCreditInputDto.TotalAmount,
                                UpdatedTime = batchBookCreditInputDto.UpdatedTime,
                                UserId = batchBookCreditInputDto.UserId
                            });
                            if (sendDebtForBankModelResult)
                            {
                                break;
                            }
                            if (i == 4)
                            {
                                isReturn = true;
                            }
                        }
                    }
                    if (isReturn)
                    {
                        this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}债转批量通知银行失败已经跳过这个";
                        Logger.LoadData(@"BatchDebtInfos\error" + batchBookCreditInputDto.RowKey + ".txt", batchBookCreditInputDto.RowKey);
                        continue;
                    }
                    await this.ConfirmDebtInfo(requeBatch); //债转确认
                    this.txb_ProcessNums.Text = listDeptsimpleInfos.Count.ToString();
                    List<string> deptIds = listDeptsimpleInfos.Select(x => x.DebtToTransferId).ToList();
                    //再执行
                    while (listDeptsimpleInfos.Count > 0)
                    {
                        //先进行确认
                        await this.ConfirmDebtInfo(requeBatch);
                        //开始执行一个
                        //string subOrderId = deptSimpleInfo.SubOrderId;
                        List<DeptModel> listdeptInfos = this.GetAllDeptModels1(listDeptsimpleInfos[0].RedemptionOrderId, deptIds);
                        foreach (DeptModel deptModel in listdeptInfos)
                        {
                            DeptModel model = deptModel;
                            SubInvestOrderInputDto deptSimpleInfo = listDeptsimpleInfos.FirstOrDefault(x => x.DebtToTransferId == model.DebtToTransferId);
                            //1.先调网关判断是否正确
                            string subOrderId = deptSimpleInfo?.SubOrderId;
                            string deptId = deptSimpleInfo?.DebtToTransferId;
                            bool getwayResult = await this.GetOrderSearch(subOrderId);
                            if (getwayResult)
                            {
                                //2.存在网关数据 获取debtToTransferId  AssetDebtRebates 是否status=1
                                if (deptModel != null)
                                {
                                    //AdvanceDebtRequest request = this.GetAdvanceDebtRequest(deptId);
                                    AdvanceDebtRequest request = new AdvanceDebtRequest();
                                    //调用放款接口
                                    bool grantResult = await this.SendAdvanceDebt(request);
                                    bool result = await this.GetBatchordersearch(request.OrderId);
                                    //如果成功则正确
                                    if (grantResult || result)
                                    {
                                        //放款确认
                                        bool confirmAdvanceDebtResult = await this.ConfirmAdvanceDebt(new ConfirmAdvanceDebtRequest
                                        {
                                            MerchantId = this.loadAppSettings.MerchantId,
                                            Message = "成功",
                                            OrderId = request?.OrderId,
                                            Remark = "",
                                            Status = "S"
                                        });
                                        if (confirmAdvanceDebtResult)
                                        {
                                            listDeptsimpleInfos.Remove(deptSimpleInfo);
                                            this.txb_hasProcessNums.Text = (Convert.ToInt32(this.txb_hasProcessNums.Text) + 1).ToString();
                                            Logger.LoadData(@"BatchDebtInfos\sucess_" + batchBookCreditInputDto.RowKey + ".txt", subOrderId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //Thread.Sleep(5000);
                    //单个rowkey执行完毕
                    this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}已经执行完毕";
                    this.txb_hasProcessNums.Text = "0";
                    this.txb_ProcessNums.Text = "0";
                }
                //执行完毕后再显示
                this.lbl_showmsg.Text = "全部执行完毕";
                this.btn_StartProcess.Enabled = true;
                this.btn_BatchProcess.Enabled = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        ///     执行完所有数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_batchReload_Click(object sender, EventArgs e)
        {
            //获取所有数据
            try
            {
                string ransomOrderId = this.txb_RansomOrderId.Text.Trim();
                //if (string.IsNullOrEmpty(ransomOrderId))
                //{
                //    MessageBox.Show("请先输入ransomOrderid");
                //    return;
                //}
                //List<string> hasDoRowkeys = new List<string>();
                //if (File.Exists("BatchDebtInfos/hasDoAllthingsRowkeys.txt"))
                //{
                //    hasDoRowkeys = File.ReadAllLines("BatchDebtInfos/hasDoAllthingsRowkeys.txt").Distinct().ToList();
                //}
                ////根据ransomOrderId获取所有的newUserId
                //this.lbl_showmsg.Text = "正在准备数据,请稍等.......";
                //this.btn_DoAllThings.Enabled = false;
                //CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                //string partitionKey = "DebtToTransfer_" + ransomOrderId;
                //TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                //    .Where($"PartitionKey eq '{partitionKey}'");
                //List<DeptModel> deptModels = deptTable.ExecuteQuery(queryDeptInfo).ToList();
                //if (deptModels.Count == 0)
                //{
                //    MessageBox.Show("输入的ransomOrderId在AssetDebtToTransfer表中没有找到数据");
                //    return;
                //}
                //List<string> newUserIds = deptModels.Select(x => x.NewUserId).Distinct().ToList();
                ////查询所有的数据
                //CloudTable deptTableBatch = this.tableClient.GetTableReference("AssetBatchBookCreditAssignments");
                //string partitionKeyBatch = "BatchBookCreditAssignment";
                //TableQuery<BatchBookCreditInputDto> queryBatch = new TableQuery<BatchBookCreditInputDto>()
                //    .Where($"PartitionKey eq '{partitionKeyBatch}'");
                //List<BatchBookCreditInputDto> creditInfos = deptTableBatch.ExecuteQuery(queryBatch).Where(x => newUserIds.Contains(x.UserId)).ToList();
                //List<BatchBookCreditInputDto> newBatchBookCreditInputDtos = (from item in creditInfos let exists = item.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>().Where(p => p.RedemptionOrderId == ransomOrderId) where exists.Any() select item).ToList();
                //if (newBatchBookCreditInputDtos.Count == 0)
                //{
                //    this.lbl_showmsg.Text = "提示";
                //    this.btn_DoAllThings.Enabled = true;
                //    MessageBox.Show("没有找到数据");
                //    return;
                //}
                ////去除掉已经执行的
                //newBatchBookCreditInputDtos.RemoveAll(x => hasDoRowkeys.Contains(x.RowKey));
                //从redis中获取所有ids=
                List<BatchBookCreditInputDto> newBatchBookCreditInputDtos = new List<BatchBookCreditInputDto>();
                foreach (BatchBookCreditInputDto batchBookCreditInputDto in newBatchBookCreditInputDtos)
                {
                    this.lbl_showmsg.Text = $"正在执行Rowkey:{batchBookCreditInputDto.RowKey}的数据......";
                    //开始执行
                    //this.txb_RansomOrderId.Text = batchBookCreditInputDto.RowKey;
                    List<SubInvestOrderInputDto> listDeptsimpleInfos = batchBookCreditInputDto.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>();
                    NotifyBatchCreditRequest requeBatch = this.MapToConfirmDebt(batchBookCreditInputDto, listDeptsimpleInfos);
                    bool batchOrderSearch = await this.GetBatchordersearch(batchBookCreditInputDto.RowKey);
                    bool isReturn = false;
                    if (!batchOrderSearch)
                    {
                        //通知银行
                        for (int i = 0; i < 5; i++)
                        {
                            bool sendDebtForBankModelResult = await this.SendDebtForBank(new SendDebtForBankModel
                            {
                                Currency = batchBookCreditInputDto.Currency,
                                OrderId = batchBookCreditInputDto.OrderId,
                                Remark = batchBookCreditInputDto.Remark,
                                Status = batchBookCreditInputDto.Status,
                                SubInvestOrderList = listDeptsimpleInfos,
                                TotalAmount = batchBookCreditInputDto.TotalAmount,
                                UpdatedTime = batchBookCreditInputDto.UpdatedTime,
                                UserId = batchBookCreditInputDto.UserId
                            });
                            if (sendDebtForBankModelResult)
                            {
                                break;
                            }
                            if (i == 4)
                            {
                                isReturn = true;
                            }
                        }
                    }
                    if (isReturn)
                    {
                        this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}债转批量通知银行失败已经跳过这个";
                        Logger.LoadData(@"BatchDoAllThings\Info" + batchBookCreditInputDto.RowKey + ".txt", batchBookCreditInputDto.RowKey);
                        continue;
                    }
                    this.txb_ProcessNums.Text = listDeptsimpleInfos.Count.ToString();
                    await this.ConfirmDebtInfo(requeBatch); //债转确认
                    int tryCount = 4;
                    //再执行
                    while (listDeptsimpleInfos.Count > 0)
                    {
                        //先进行确认
                        List<string> deptIds = listDeptsimpleInfos.Select(x => x.DebtToTransferId).ToList();
                        if (tryCount == 0)
                        {
                            //记下该rowkey下还有多少suborderid没有执行
                            Logger.LoadData(@"BatchDebtInfos\errorRowkeyDeptIds_" + batchBookCreditInputDto.RowKey + ".txt", string.Join("\r\n", deptIds));
                            break;
                        }
                        await this.ConfirmDebtInfo(requeBatch);
                        //开始执行一个
                        List<DeptModel> listdeptInfos = this.GetAllDeptModels1(listDeptsimpleInfos[0].RedemptionOrderId, deptIds);
                        List<AdvanceDebtInputDto> listadvAdvanceDebtInputDtos = new List<AdvanceDebtInputDto>();
                        //如果存在数据则直接拉去所有的数据
                        if (listdeptInfos.Count > 0)
                        {
                            listadvAdvanceDebtInputDtos = this.GetAdvanceDebtInfos(deptIds);
                        }
                        foreach (DeptModel deptModel in listdeptInfos)
                        {
                            DeptModel model = deptModel;
                            SubInvestOrderInputDto deptSimpleInfo = listDeptsimpleInfos.FirstOrDefault(x => x.DebtToTransferId == model.DebtToTransferId);
                            //1.先调网关判断是否正确
                            string subOrderId = deptSimpleInfo?.SubOrderId;
                            string deptId = deptSimpleInfo?.DebtToTransferId;
                            bool getwayResult = await this.GetOrderSearch(subOrderId);
                            if (getwayResult)
                            {
                                //2.存在网关数据 获取debtToTransferId  AssetDebtRebates 是否status=1  List<string>orderIds
                                if (deptModel != null)
                                {
                                    AdvanceDebtInputDto advanceDebtInputDto = listadvAdvanceDebtInputDtos.FirstOrDefault(x => x.DebtToTransferId == deptId);
                                    if (advanceDebtInputDto == null)
                                    {
                                        continue;
                                    }
                                    AdvanceDebtRequest request = this.GetAdvanceDebtRequest(advanceDebtInputDto);
                                    //调用放款接口
                                    bool grantResult = await this.SendAdvanceDebt(request);
                                    bool result = await this.GetBatchordersearch(request.OrderId);
                                    //如果成功则正确
                                    if (grantResult || result)
                                    {
                                        //放款确认
                                        bool confirmAdvanceDebtResult = await this.ConfirmAdvanceDebt(new ConfirmAdvanceDebtRequest
                                        {
                                            MerchantId = this.loadAppSettings.MerchantId,
                                            Message = "成功",
                                            OrderId = request.OrderId,
                                            Remark = "",
                                            Status = "S"
                                        });
                                        if (confirmAdvanceDebtResult)
                                        {
                                            //通知交易两次
                                            //申购成功通知交易系统
                                            TifisfalPurchaseRequestModel triTifisfalPurchaseRequestModel = new TifisfalPurchaseRequestModel
                                            {
                                                AllotAmount = deptModel.Amount + deptModel.Interest - request.SubOrderList.Sum(x => x.Fee),
                                                IsReturnToAccount = false,
                                                ResultTime = deptModel.CreatedTime,
                                                UserIdentifier = deptModel.NewUserId,
                                                YemAssetRecordIdentifier = deptModel.NewUserAssetRatioId,
                                                YemOrderIdentifier = deptModel.BuyOrderId
                                            };
                                            TirisfalUserRedemptionInfoModel tirisfalUserRedemptionInfoModel = new TirisfalUserRedemptionInfoModel
                                            {
                                                Amount = deptModel.Amount + deptModel.Interest,
                                                ResultTime = deptModel.CreatedTime,
                                                UserIdentifier = deptModel.OldUserId,
                                                YemAssetRecordIdentifier = deptModel.DebtToTransferId,
                                                YemOrderIdentifier = deptModel.RansomOrderId,
                                                SystemAssetRecordIdentifier = Guid.Empty.ToGuidString(),
                                                ConfirmType = 1
                                            };
                                            //通知两次
                                            await this.PurchaseSendToTrader(triTifisfalPurchaseRequestModel);
                                            await this.RedemptionSendToTrader(tirisfalUserRedemptionInfoModel);
                                            listDeptsimpleInfos.Remove(deptSimpleInfo);
                                            this.txb_hasProcessNums.Text = (Convert.ToInt32(this.txb_hasProcessNums.Text) + 1).ToString();
                                        }
                                    }
                                }
                            }
                        }
                        tryCount--;
                    }
                    //调用完之后再
                    //Thread.Sleep(5000);
                    //单个rowkey执行完毕
                    //判断执行的数量
                    Logger.LoadData(@"BatchDebtInfos\hasDoAllthingsRowkeys.txt", batchBookCreditInputDto.RowKey);
                    this.btn_DoAllThings.Enabled = true;
                    this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}已经执行完毕";
                    this.txb_hasProcessNums.Text = "0";
                    this.txb_ProcessNums.Text = "0";
                }
            }
            catch (Exception exception)
            {
                Logger.LoadData(@"DebtInfo\Error.txt", exception.StackTrace);
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     做所有的事情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DoAllThings_Click(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                try
                {
                    //查询所有的cellphone
                    List<string> listRansomOrderIds = File.ReadAllLines("AllRansomOrderIds.txt").ToList();
                    if (listRansomOrderIds.Count == 0)
                    {
                        MessageBox.Show("请先输入ransomOrderid List到AllRansomOrderIds.txt");
                        return;
                    }
                    List<string> hasDoRowkeys = new List<string>();
                    if (File.Exists("BatchDebtInfos/hasCompletedRowkeys.txt"))
                    {
                        hasDoRowkeys = File.ReadAllLines("BatchDebtInfos/hasCompletedRowkeys.txt").Distinct().ToList();
                    }
                    this.lbl_showmsg.Text = "正在准备数据,请稍等.......";
                    this.btn_DoAllThings.Enabled = false;
                    foreach (string ransomOrderId in listRansomOrderIds)
                    {
                        this.txb_RansomOrderId.Text = ransomOrderId;
                        CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                        string partitionKey = "DebtToTransfer_" + ransomOrderId;
                        TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                            .Where($"PartitionKey eq '{partitionKey}'");
                        List<DeptModel> deptModels = deptTable.ExecuteQuery(queryDeptInfo).OrderBy(x => x.Status).ToList();
                        if (deptModels.Count == 0)
                        {
                            this.lbl_showmsg.Text = $"ransomOrderId:{ransomOrderId}没有获取到数据,跳转到另外一个数据";
                            continue;
                        }
                        List<string> newUserIds = deptModels.Select(x => x.NewUserId).Distinct().ToList();
                        //查询所有的数据
                        CloudTable deptTableBatch = this.tableClient.GetTableReference("AssetBatchBookCreditAssignments");
                        string partitionKeyBatch = "BatchBookCreditAssignment";
                        TableQuery<BatchBookCreditInputDto> queryBatch = new TableQuery<BatchBookCreditInputDto>()
                            .Where($"PartitionKey eq '{partitionKeyBatch}'");
                        List<BatchBookCreditInputDto> creditInfos = deptTableBatch.ExecuteQuery(queryBatch).Where(x => newUserIds.Contains(x.UserId)).ToList();
                        List<BatchBookCreditInputDto> newBatchBookCreditInputDtos = (from item in creditInfos let exists = item.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>().Where(p => p.RedemptionOrderId == ransomOrderId) where exists.Any() select item).ToList();
                        //去除掉已经执行的
                        newBatchBookCreditInputDtos.RemoveAll(x => hasDoRowkeys.Contains(x.RowKey));
                        if (newBatchBookCreditInputDtos.Count == 0)
                        {
                            this.lbl_showmsg.Text = $"ransomOrderId:{ransomOrderId}没有获取到数据,跳转到另外一个数据";
                            continue;
                        }
                        //计算一下需要执行的总数量
                        foreach (BatchBookCreditInputDto batchBookCreditInputDto in newBatchBookCreditInputDtos)
                        {
                            this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}准备开始.....";
                            //开始执行
                            //this.txb_RansomOrderId.Text = batchBookCreditInputDto.RowKey;
                            List<SubInvestOrderInputDto> listDeptsimpleInfos = batchBookCreditInputDto.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>();
                            this.txb_ProcessNums.Text = (Convert.ToInt32(this.txb_ProcessNums.Text.Trim()) + listDeptsimpleInfos.Count).ToString();
                            NotifyBatchCreditRequest requeBatch = this.MapToConfirmDebt(batchBookCreditInputDto, listDeptsimpleInfos);
                            bool batchOrderSearch = await this.GetBatchordersearch(batchBookCreditInputDto.RowKey);
                            bool isReturn = false;
                            this.lbl_showmsg.Text = "正在执行批量债转到银行.......";
                            if (!batchOrderSearch)
                            {
                                //通知银行
                                for (int i = 0; i < 5; i++)
                                {
                                    bool sendDebtForBankModelResult = await this.SendDebtForBank(new SendDebtForBankModel
                                    {
                                        Currency = batchBookCreditInputDto.Currency,
                                        OrderId = batchBookCreditInputDto.OrderId,
                                        Remark = batchBookCreditInputDto.Remark,
                                        Status = batchBookCreditInputDto.Status,
                                        SubInvestOrderList = listDeptsimpleInfos,
                                        TotalAmount = batchBookCreditInputDto.TotalAmount,
                                        UpdatedTime = batchBookCreditInputDto.UpdatedTime,
                                        UserId = batchBookCreditInputDto.UserId
                                    });
                                    if (sendDebtForBankModelResult)
                                    {
                                        break;
                                    }
                                    if (i == 4)
                                    {
                                        isReturn = true;
                                    }
                                }
                            }
                            if (isReturn)
                            {
                                this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}债转批量通知银行失败已经跳过这个";
                                Logger.LoadData(@"BatchDebtInfos\ErrorRowKeys" + batchBookCreditInputDto.RowKey + ".txt", batchBookCreditInputDto.RowKey);
                                this.txb_hasProcessNums.Text = (Convert.ToInt32(this.txb_hasProcessNums.Text) + 1).ToString();
                                continue;
                            }
                            await this.ConfirmDebtInfo(requeBatch); //债转确认
                            List<string> debtToTransferIds = listDeptsimpleInfos.Select(x => x.DebtToTransferId).ToList();
                            List<DeptModel> listDeptInfos;
                            //一直confirmdebtInfo 使status都为1
                            this.lbl_showmsg.Text = "正在执行MessageFromBank/ConfirmDebt保证DebtToTransfer status都为1";
                            do
                            {
                                await this.ConfirmDebtInfo(requeBatch);
                                //再获取所有status==1的
                                listDeptInfos = this.GetAllDeptModels1(listDeptsimpleInfos[0].RedemptionOrderId, debtToTransferIds);
                                //日志记下
                            } while (listDeptInfos.Count != debtToTransferIds.Count);
                            this.lbl_showmsg.Text = "正在开始准备债转订单进行放款和通知交易的数据,请稍等.......";
                            List<AdvanceDebtInputDto> advanceDebtInputInfos = this.GetAdvanceDebtInfos(debtToTransferIds);
                            this.lbl_showmsg.Text = "正在开始准备债转订单进行放款和通知交易.......";
                            int count = listDeptsimpleInfos.Count;
                            for (int i = 0; i < 6; i++)
                            {
                                Thread thDoOneSubOrder = new Thread(async () =>
                                {
                                    while (listDeptsimpleInfos.Count > 0)
                                    {
                                        SubInvestOrderInputDto subInvestOrderInputDtoOne = null;
                                        DeptModel debtModel = null;
                                        AdvanceDebtInputDto advanceDebtInputDto = null;
                                        if (listDeptsimpleInfos.Count > 0)
                                        {
                                            lock (this.objLock)
                                            {
                                                if (listDeptsimpleInfos.Count > 0)
                                                {
                                                    subInvestOrderInputDtoOne = listDeptsimpleInfos[0];
                                                    if (subInvestOrderInputDtoOne != null)
                                                    {
                                                        listDeptsimpleInfos.Remove(subInvestOrderInputDtoOne);
                                                        debtModel = listDeptInfos.FirstOrDefault(x => x.DebtToTransferId == subInvestOrderInputDtoOne.DebtToTransferId);
                                                        advanceDebtInputDto = advanceDebtInputInfos.FirstOrDefault(x => x.DebtToTransferId == subInvestOrderInputDtoOne.DebtToTransferId);
                                                    }
                                                }
                                            }
                                            //再判断
                                            if (subInvestOrderInputDtoOne != null)
                                            {
                                                string subOrderId = subInvestOrderInputDtoOne.SubOrderId;
                                                string debtTransferId = subInvestOrderInputDtoOne.DebtToTransferId;
                                                bool getwayResult = await this.GetOrderSearch(subOrderId);
                                                for (int j = 0; j < 3; j++)
                                                {
                                                    Thread.Sleep(500);
                                                    getwayResult = await this.GetOrderSearch(subOrderId);
                                                    if (getwayResult)
                                                    {
                                                        break;
                                                    }
                                                }
                                                //执行操作判断网关
                                                if (getwayResult)
                                                {
                                                    if (debtModel != null)
                                                    {
                                                        if (advanceDebtInputDto == null)
                                                        {
                                                            continue;
                                                        }
                                                        //调用放款接口
                                                        bool result = await this.GetBatchordersearch(advanceDebtInputDto.OrderId);
                                                        bool grantResult = false;
                                                        AdvanceDebtRequest request = this.GetAdvanceDebtRequest(advanceDebtInputDto);
                                                        if (!result)
                                                        {
                                                            for (int j = 0; j < 2; j++)
                                                            {
                                                                grantResult = await this.SendAdvanceDebt(request);
                                                                if (grantResult)
                                                                {
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        //如果成功则正确
                                                        if (grantResult || result)
                                                        {
                                                            //放款确认
                                                            bool confirmAdvanceDebtResult = await this.ConfirmAdvanceDebt(new ConfirmAdvanceDebtRequest
                                                            {
                                                                MerchantId = this.loadAppSettings.MerchantId,
                                                                Message = "成功",
                                                                OrderId = advanceDebtInputDto.OrderId,
                                                                Remark = "",
                                                                Status = "S"
                                                            });
                                                            if (confirmAdvanceDebtResult)
                                                            {
                                                                //通知交易两次
                                                                //申购成功通知交易系统
                                                                TifisfalPurchaseRequestModel triTifisfalPurchaseRequestModel = new TifisfalPurchaseRequestModel
                                                                {
                                                                    AllotAmount = debtModel.Amount + debtModel.Interest,
                                                                    IsReturnToAccount = false,
                                                                    ResultTime = debtModel.CreatedTime,
                                                                    UserIdentifier = debtModel.NewUserId,
                                                                    YemAssetRecordIdentifier = debtModel.NewUserAssetRatioId,
                                                                    YemOrderIdentifier = debtModel.BuyOrderId
                                                                };
                                                                TirisfalUserRedemptionInfoModel tirisfalUserRedemptionInfoModel = new TirisfalUserRedemptionInfoModel
                                                                {
                                                                    Amount = debtModel.Amount + debtModel.Interest - request.SubOrderList.Sum(x => x.Fee),
                                                                    ResultTime = debtModel.CreatedTime,
                                                                    UserIdentifier = debtModel.OldUserId,
                                                                    YemAssetRecordIdentifier = debtModel.DebtToTransferId,
                                                                    YemOrderIdentifier = debtModel.RansomOrderId,
                                                                    SystemAssetRecordIdentifier = Guid.Empty.ToGuidString(),
                                                                    ConfirmType = 1
                                                                };
                                                                //通知两次
                                                                await this.PurchaseSendToTrader(triTifisfalPurchaseRequestModel);
                                                                await this.RedemptionSendToTrader(tirisfalUserRedemptionInfoModel);
                                                                lock (this.objLock)
                                                                {
                                                                    this.txb_SuccessNums.Text = (Convert.ToInt32(this.txb_SuccessNums.Text) + 1).ToString();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Logger.LoadData(@"BatchDebtInfos\ErrorbyconfirmAdvanceDebtResult+" + batchBookCreditInputDto.RowKey + ".txt", new { SuborderId = subOrderId, DebtToTransferId = debtTransferId }.ToJson());
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    //直接记录下debtId
                                                    Logger.LoadData(@"BatchDebtInfos\ErrorbyBankOneDebttranfer+" + batchBookCreditInputDto.RowKey + ".txt", new { SuborderId = subOrderId, DebtToTransferId = debtTransferId }.ToJson());
                                                }
                                            }
                                            lock (this.objLock)
                                            {
                                                count = count - 1;
                                                this.txb_hasProcessNums.Text = (Convert.ToInt32(this.txb_hasProcessNums.Text) + 1).ToString();
                                            }
                                        }
                                    }
                                });
                                thDoOneSubOrder.IsBackground = true;
                                thDoOneSubOrder.Start();
                            }
                            while (count > 0)
                            {
                                Thread.Sleep(1000);
                            }
                            Logger.LoadData(@"BatchDebtInfos\hasCompletedRowkeys.txt", batchBookCreditInputDto.RowKey);
                            this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}已经执行完毕";
                            //调用完之后再
                            //Thread.Sleep(5000);
                            //单个rowkey执行完毕
                            //判断执行的数量
                        }
                    }
                    this.btn_DoAllThings.Enabled = true;
                    this.lbl_showmsg.Text = "全部执行完毕!";
                    //根据ransomOrderId获取所有的newUserId
                }
                catch (Exception exception)
                {
                    this.btn_DoAllThings.Enabled = true;
                    this.lbl_showmsg.Text = "发生错误" + exception.Message;
                    Logger.LoadData(@"DebtInfo\Error.txt", exception.StackTrace);
                }
            });
        }

        /// <summary>
        ///     批量放款操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Grant_Click(object sender, EventArgs e)
        {
            try
            {
                RedisHelperSpecial redis = new RedisHelperSpecial(9);
                //获取所有的status==0的数据
                this.lbl_ShowGrantMsg.Text = "正在准备数据......";
                CloudTable deptTable = this.tableClient.GetTableReference("AssetAdvanceDebts");
                string partitionKey = "AdvanceDebt";
                TableQuery<AdvanceDebtInputDto> queryDeptInfo = new TableQuery<AdvanceDebtInputDto>()
                    .Where($"PartitionKey eq '{partitionKey}' and Status eq 0");
                List<AdvanceDebtInputDto> advanceddAdvanceDebtInputDtos = deptTable.ExecuteQuery(queryDeptInfo).ToList();
                //List<string> debtIds = advanceddAdvanceDebtInputDtos.Select(x => x.DebtToTransferId).ToList();
                //获取所有的debtIds
                Dictionary<string, List<DeptModel>> dicDeptInfos = new Dictionary<string, List<DeptModel>>();
                //执行数据
                if (advanceddAdvanceDebtInputDtos.Count == 0)
                {
                    this.lbl_ShowGrantMsg.Text = "提示：";
                    return;
                }
                this.btn_Grant.Enabled = false;
                int count = advanceddAdvanceDebtInputDtos.Count;
                this.lbl_ShowGrantMsg.Text = "正在开始执行放款，确认放款，通知交易等操作......";
                for (int i = 0; i < this.loadAppSettings.AssetThreadNums; i++)
                {
                    Thread thread = new Thread(async () =>
                    {
                        while (advanceddAdvanceDebtInputDtos.Count > 0)
                        {
                            AdvanceDebtInputDto advanceDebtInputDto = null;
                            if (advanceddAdvanceDebtInputDtos.Count > 0)
                            {
                                lock (this.objLock)
                                {
                                    if (advanceddAdvanceDebtInputDtos.Count > 0)
                                    {
                                        advanceDebtInputDto = advanceddAdvanceDebtInputDtos[0];
                                        if (advanceDebtInputDto != null)
                                        {
                                            advanceddAdvanceDebtInputDtos.Remove(advanceDebtInputDto);
                                        }
                                    }
                                }
                                if (advanceDebtInputDto != null)
                                {
                                    //执行放款 确认放款 通知交易
                                    bool result = await this.GetBatchordersearch(advanceDebtInputDto.OrderId);
                                    bool grantResult = false;
                                    AdvanceDebtRequest request = this.GetAdvanceDebtRequest(advanceDebtInputDto);
                                    if (!result)
                                    {
                                        for (int j = 0; j < 2; j++)
                                        {
                                            grantResult = await this.SendAdvanceDebt(request);
                                            if (grantResult)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    //如果成功则正确
                                    if (grantResult || result)
                                    {
                                        //放款确认
                                        bool confirmAdvanceDebtResult = await this.ConfirmAdvanceDebt(new ConfirmAdvanceDebtRequest
                                        {
                                            MerchantId = this.loadAppSettings.MerchantId,
                                            Message = "成功",
                                            OrderId = advanceDebtInputDto.OrderId,
                                            Remark = "",
                                            Status = "S"
                                        });
                                        if (confirmAdvanceDebtResult)
                                        {
                                            //再执行数据成功了
                                            //查找
                                            DeptModel debtModel = null;
                                            if (!dicDeptInfos.ContainsKey(request.RedemptionOrderId))
                                            {
                                                //获取所有的数据
                                                CloudTable deptTableTransfer = this.tableClient.GetTableReference("AssetDebtToTransfer");
                                                TableQuery<DeptModel> queryDeptInfos = new TableQuery<DeptModel>()
                                                    .Where($"PartitionKey eq 'DebtToTransfer_{request.RedemptionOrderId}' and Status eq 1");
                                                List<DeptModel> deptModels = deptTableTransfer.ExecuteQuery(queryDeptInfos).ToList();
                                                if (deptModels.Count > 0)
                                                {
                                                    dicDeptInfos.Add(request.RedemptionOrderId, deptModels);
                                                    debtModel = deptModels.FirstOrDefault(x => x.DebtToTransferId == request.DebtToTransferId);
                                                }
                                            }
                                            else
                                            {
                                                debtModel = dicDeptInfos[request.RedemptionOrderId].FirstOrDefault(x => x.DebtToTransferId == request.DebtToTransferId);
                                            }
                                            if (debtModel != null)
                                            {
                                                //执行通知交易
                                                //通知交易两次
                                                //申购成功通知交易系统
                                                TifisfalPurchaseRequestModel triTifisfalPurchaseRequestModel = new TifisfalPurchaseRequestModel
                                                {
                                                    AllotAmount = debtModel.Amount + debtModel.Interest,
                                                    IsReturnToAccount = false,
                                                    ResultTime = debtModel.CreatedTime,
                                                    UserIdentifier = debtModel.NewUserId,
                                                    YemAssetRecordIdentifier = debtModel.NewUserAssetRatioId,
                                                    YemOrderIdentifier = debtModel.BuyOrderId
                                                };
                                                TirisfalUserRedemptionInfoModel tirisfalUserRedemptionInfoModel = new TirisfalUserRedemptionInfoModel
                                                {
                                                    Amount = debtModel.Amount + debtModel.Interest - request.SubOrderList.Sum(x => x.Fee),
                                                    ResultTime = debtModel.CreatedTime,
                                                    UserIdentifier = debtModel.OldUserId,
                                                    YemAssetRecordIdentifier = debtModel.DebtToTransferId,
                                                    YemOrderIdentifier = debtModel.RansomOrderId,
                                                    SystemAssetRecordIdentifier = Guid.Empty.ToGuidString(),
                                                    ConfirmType = 1
                                                };
                                                //通知两次
                                                Tuple<bool, string> rsult1 = await this.PurchaseSendToTrader(triTifisfalPurchaseRequestModel);
                                                Tuple<bool, string> rsult2 = await this.RedemptionSendToTrader(tirisfalUserRedemptionInfoModel);
                                                if (!rsult1.Item1)
                                                {
                                                    //写redis
                                                    await redis.SetRedisNotifyPurchaseAsync(triTifisfalPurchaseRequestModel, request.DebtToTransferId);
                                                }
                                                if (!rsult2.Item1)
                                                {
                                                    await redis.SetRedistirisfalUserRedemptionInfoModelAsync(tirisfalUserRedemptionInfoModel, request.DebtToTransferId);
                                                }
                                            }
                                        }
                                    }

                                    this.txb_GrantNumbers.Text = (Convert.ToInt32(this.txb_GrantNumbers.Text) + 1).ToString();
                                }
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                Thread thShow = new Thread(() =>
                {
                    while (advanceddAdvanceDebtInputDtos.Count > 0)
                    {
                        Thread.Sleep(2000);
                    }
                    Thread.Sleep(60000);
                    this.btn_Grant.Enabled = true;
                    this.lbl_ShowGrantMsg.Text = "全部执行完毕";
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.btn_Grant.Enabled = true;
                this.lbl_ShowGrantMsg.Text = "发生错误:" + exception.Message;
            }
        }

        /// <summary>
        ///     将数据放入到redis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Redis_Click(object sender, EventArgs e)
        {
            try
            {
                RedisHelperSpecial redisHelperSpecial = new RedisHelperSpecial(9);
                string ransomOrderId = this.txb_RansomOrderId.Text.Trim();
                if (string.IsNullOrEmpty(ransomOrderId))
                {
                    MessageBox.Show("请先输入ransomOrderid");
                    return;
                }
                List<string> hasDoRowkeys = new List<string>();
                if (File.Exists("BatchDebtInfos/hasRedisRowkeys.txt"))
                {
                    hasDoRowkeys = File.ReadAllLines("BatchDebtInfos/hasRedisRowkeys.txt").Distinct().ToList();
                }
                //根据ransomOrderId获取所有的newUserId
                this.lbl_showmsg.Text = "正在准备数据,请稍等.......";
                this.btn_DoAllThings.Enabled = false;
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                string partitionKey = "DebtToTransfer_" + ransomOrderId;
                TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                    .Where($"PartitionKey eq '{partitionKey}'");
                List<DeptModel> deptModels = deptTable.ExecuteQuery(queryDeptInfo).ToList();
                if (deptModels.Count == 0)
                {
                    MessageBox.Show("输入的ransomOrderId在AssetDebtToTransfer表中没有找到数据");
                    return;
                }
                List<string> newUserIds = deptModels.Select(x => x.NewUserId).Distinct().ToList();
                //查询所有的数据
                CloudTable deptTableBatch = this.tableClient.GetTableReference("AssetBatchBookCreditAssignments");
                string partitionKeyBatch = "BatchBookCreditAssignment";
                TableQuery<BatchBookCreditInputDto> queryBatch = new TableQuery<BatchBookCreditInputDto>()
                    .Where($"PartitionKey eq '{partitionKeyBatch}'");
                List<BatchBookCreditInputDto> creditInfos = deptTableBatch.ExecuteQuery(queryBatch).Where(x => newUserIds.Contains(x.UserId)).ToList();
                List<BatchBookCreditInputDto> newBatchBookCreditInputDtos = (from item in creditInfos let exists = item.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>().Where(p => p.RedemptionOrderId == ransomOrderId) where exists.Any() select item).ToList();
                if (newBatchBookCreditInputDtos.Count == 0)
                {
                    this.lbl_showmsg.Text = "提示";
                    this.btn_DoAllThings.Enabled = true;
                    MessageBox.Show("没有找到数据");
                    return;
                }
                //去除掉已经执行的
                newBatchBookCreditInputDtos.RemoveAll(x => hasDoRowkeys.Contains(x.RowKey));
                this.txb_ProcessNums.Text = newBatchBookCreditInputDtos.Count.ToString();
                this.lbl_showmsg.Text = "正在同步到redis并做债转批量通知";
                foreach (BatchBookCreditInputDto batchBookCreditInputDto in newBatchBookCreditInputDtos)
                {
                    this.lbl_showmsg.Text = $"正在执行Rowkey:{batchBookCreditInputDto.RowKey}的数据......";
                    //开始执行
                    //this.txb_RansomOrderId.Text = batchBookCreditInputDto.RowKey;
                    List<SubInvestOrderInputDto> listDeptsimpleInfos = batchBookCreditInputDto.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>();

                    //*****将BatchBookCreditInputDto数据信息放入到redis
                    batchBookCreditInputDto.SubInvestOrderList = null;
                    await redisHelperSpecial.SetRedisBatchCreditInfoAsync(batchBookCreditInputDto, batchBookCreditInputDto.RowKey);
                    //*****放入到redis中
                    await redisHelperSpecial.SetRedisSubOrderListInfosAsync(listDeptsimpleInfos, batchBookCreditInputDto.RowKey);
                    NotifyBatchCreditRequest requeBatch = this.MapToConfirmDebt(batchBookCreditInputDto, listDeptsimpleInfos);
                    bool batchOrderSearch = await this.GetBatchordersearch(batchBookCreditInputDto.RowKey);
                    bool isReturn = false;
                    if (!batchOrderSearch)
                    {
                        //通知银行
                        for (int i = 0; i < 5; i++)
                        {
                            bool sendDebtForBankModelResult = await this.SendDebtForBank(new SendDebtForBankModel
                            {
                                Currency = batchBookCreditInputDto.Currency,
                                OrderId = batchBookCreditInputDto.OrderId,
                                Remark = batchBookCreditInputDto.Remark,
                                Status = batchBookCreditInputDto.Status,
                                SubInvestOrderList = listDeptsimpleInfos,
                                TotalAmount = batchBookCreditInputDto.TotalAmount,
                                UpdatedTime = batchBookCreditInputDto.UpdatedTime,
                                UserId = batchBookCreditInputDto.UserId
                            });
                            if (sendDebtForBankModelResult)
                            {
                                break;
                            }
                            if (i == 4)
                            {
                                isReturn = true;
                            }
                        }
                    }
                    if (isReturn)
                    {
                        this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}债转批量通知银行失败已经跳过这个";
                        Logger.LoadData(@"BatchDebtInfos/ErrorRedisRowkeys.txt", batchBookCreditInputDto.RowKey);
                        continue;
                    }
                    await this.ConfirmDebtInfo(requeBatch); //债转确认
                    Logger.LoadData(@"BatchDebtInfos/hasRedisRowkeys.txt", batchBookCreditInputDto.RowKey);
                    this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}已经执行完毕";
                    this.txb_hasProcessNums.Text = (Convert.ToInt32(this.txb_hasProcessNums.Text) + 1).ToString();
                }
                this.btn_DoAllThings.Enabled = true;
                this.lbl_showmsg.Text = "全部执行完毕";
            }
            catch (Exception exception)
            {
                this.btn_DoAllThings.Enabled = true;
                Logger.LoadData(@"DebtInfo\Error.txt", exception.StackTrace);
                MessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        ///     Reload To Redis
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task btn_ReloadToRedis_Click(object sender, EventArgs e)
        {
            try
            {
                string ransomOrderId = this.txb_RansomOrderId.Text.Trim();
                if (string.IsNullOrEmpty(ransomOrderId))
                {
                    MessageBox.Show("请先输入ransomOrderid");
                    return;
                }
                //根据ransomOrderId获取所有的newUserId
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                string partitionKey = "DebtToTransfer_" + ransomOrderId;
                TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                    .Where($"PartitionKey eq '{partitionKey}'");
                List<DeptModel> deptModels = deptTable.ExecuteQuery(queryDeptInfo).ToList();
                if (deptModels.Count == 0)
                {
                    MessageBox.Show("输入的ransomOrderId在AssetDebtToTransfer表中没有找到数据");
                    return;
                }
                //存在后根据newUserId 去找AssetBatchBookCreditAssignments 中userid 然后再找到rowKey
                List<string> newUserIds = deptModels.Select(x => x.NewUserId).Distinct().ToList();
                //查询所有的数据
                CloudTable deptTableBatch = this.tableClient.GetTableReference("AssetBatchBookCreditAssignments");
                string partitionKeyBatch = "BatchBookCreditAssignment";
                TableQuery<BatchBookCreditInputDto> queryBatch = new TableQuery<BatchBookCreditInputDto>()
                    .Where($"PartitionKey eq '{partitionKeyBatch}'");
                List<BatchBookCreditInputDto> creditInfos = deptTableBatch.ExecuteQuery(queryBatch).Where(x => newUserIds.Contains(x.UserId)).ToList();
                List<BatchBookCreditInputDto> newBatchBookCreditInputDtos = new List<BatchBookCreditInputDto>();
                foreach (var item in creditInfos)
                {
                    var exists = item.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>().Where(p => p.RedemptionOrderId == ransomOrderId);
                    if (exists.Any())
                    {
                        newBatchBookCreditInputDtos.Add(item);
                    }
                }
                //多样
                foreach (BatchBookCreditInputDto batchBookCreditInputDto in newBatchBookCreditInputDtos)
                {
                    this.lbl_showmsg.Text = $"正在执行Rowkey:{batchBookCreditInputDto.RowKey}的数据......";
                    //开始执行
                    this.txb_RansomOrderId.Text = batchBookCreditInputDto.RowKey;
                    List<SubInvestOrderInputDto> listDeptsimpleInfos = batchBookCreditInputDto.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>();
                    NotifyBatchCreditRequest requeBatch = this.MapToConfirmDebt(batchBookCreditInputDto, listDeptsimpleInfos);
                    bool batchOrderSearch = await this.GetBatchordersearch(batchBookCreditInputDto.RowKey);
                    bool isReturn = false;
                    if (!batchOrderSearch)
                    {
                        //通知银行
                        for (int i = 0; i < 5; i++)
                        {
                            bool sendDebtForBankModelResult = await this.SendDebtForBank(new SendDebtForBankModel
                            {
                                Currency = batchBookCreditInputDto.Currency,
                                OrderId = batchBookCreditInputDto.OrderId,
                                Remark = batchBookCreditInputDto.Remark,
                                Status = batchBookCreditInputDto.Status,
                                SubInvestOrderList = listDeptsimpleInfos,
                                TotalAmount = batchBookCreditInputDto.TotalAmount,
                                UpdatedTime = batchBookCreditInputDto.UpdatedTime,
                                UserId = batchBookCreditInputDto.UserId
                            });
                            if (sendDebtForBankModelResult)
                            {
                                break;
                            }
                            if (i == 4)
                            {
                                isReturn = true;
                            }
                        }
                    }
                    if (isReturn)
                    {
                        this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}债转批量通知银行失败已经跳过这个";
                        Logger.LoadData(@"BatchDoAllThings\Info" + batchBookCreditInputDto.RowKey + ".txt", batchBookCreditInputDto.RowKey);
                        continue;
                    }
                    this.txb_ProcessNums.Text = listDeptsimpleInfos.Count.ToString();
                    await this.ConfirmDebtInfo(requeBatch); //债转确认
                    List<string> deptIds = listDeptsimpleInfos.Select(x => x.DebtToTransferId).ToList();
                    //再执行
                    while (listDeptsimpleInfos.Count > 0)
                    {
                        //先进行确认
                        await this.ConfirmDebtInfo(requeBatch);
                        //开始执行一个
                        //string subOrderId = deptSimpleInfo.SubOrderId;
                        List<DeptModel> listdeptInfos = this.GetAllDeptModels1(listDeptsimpleInfos[0].RedemptionOrderId, deptIds);
                        foreach (DeptModel deptModel in listdeptInfos)
                        {
                            DeptModel model = deptModel;
                            SubInvestOrderInputDto deptSimpleInfo = listDeptsimpleInfos.FirstOrDefault(x => x.DebtToTransferId == model.DebtToTransferId);
                            //1.先调网关判断是否正确
                            string subOrderId = deptSimpleInfo?.SubOrderId;
                            string deptId = deptSimpleInfo?.DebtToTransferId;
                            bool getwayResult = await this.GetOrderSearch(subOrderId);
                            if (getwayResult)
                            {
                                //2.存在网关数据 获取debtToTransferId  AssetDebtRebates 是否status=1  List<string>orderIds
                                if (deptModel != null)
                                {
                                    //AdvanceDebtRequest request = this.GetAdvanceDebtRequest(deptId);
                                    //调用放款接口
                                    AdvanceDebtRequest request = new AdvanceDebtRequest();
                                    bool grantResult = await this.SendAdvanceDebt(request);
                                    bool result = await this.GetBatchordersearch(request.OrderId);
                                    //如果成功则正确
                                    if (grantResult || result)
                                    {
                                        //放款确认
                                        bool confirmAdvanceDebtResult = await this.ConfirmAdvanceDebt(new ConfirmAdvanceDebtRequest
                                        {
                                            MerchantId = this.loadAppSettings.MerchantId,
                                            Message = "成功",
                                            OrderId = request.OrderId,
                                            Remark = "",
                                            Status = "S"
                                        });
                                        if (confirmAdvanceDebtResult)
                                        {
                                            listDeptsimpleInfos.Remove(deptSimpleInfo);
                                            this.txb_hasProcessNums.Text = (Convert.ToInt32(this.txb_hasProcessNums.Text) + 1).ToString();
                                            Logger.LoadData(@"BatchDebtInfos\sucess_" + batchBookCreditInputDto.RowKey + ".txt", subOrderId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //调用完之后再
                    //Thread.Sleep(5000);
                    //单个rowkey执行完毕
                    this.lbl_showmsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}已经执行完毕";
                    this.txb_hasProcessNums.Text = "0";
                    this.txb_ProcessNums.Text = "0";
                }
            }
            catch (Exception ex)
            {
                //ignore
                //
            }
        }

        private void btn_SearchBankStatus_Click(object sender, EventArgs e)
        {
        }

        private async void btn_StartProcess_Click(object sender, EventArgs e)
        {
            //根据AssetBatchBookCreditAssignments
            string rowKey = this.txb_RansomOrderId.Text.Trim();
            CloudTable deptTable = this.tableClient.GetTableReference("AssetBatchBookCreditAssignments");
            string partitionKey = "BatchBookCreditAssignment";
            TableQuery<BatchBookCreditInputDto> queryDeptInfo = new TableQuery<BatchBookCreditInputDto>()
                .Where($"PartitionKey eq '{partitionKey}' and RowKey eq '{rowKey}'");
            BatchBookCreditInputDto creditInfo = deptTable.ExecuteQuery(queryDeptInfo).FirstOrDefault();
            if (creditInfo == null)
            {
                MessageBox.Show("未找到该债转数据信息.......");
                return;
            }
            //反序列化
            //string ransomOrderId = creditInfo.OrderId;
            this.btn_StartProcess.Enabled = false;
            this.lbl_showmsg.Text = "提示:正在进行操作......";
            List<SubInvestOrderInputDto> listDeptsimpleInfos = creditInfo.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>();
            NotifyBatchCreditRequest requeBatch = this.MapToConfirmDebt(creditInfo, listDeptsimpleInfos);
            //先判断batchordersearch rowkey在银行是否成功了 没有成功就拼参数调send
            bool batchOrderSearch = await this.GetBatchordersearch(rowKey);
            if (!batchOrderSearch)
            {
                //通知银行
                for (int i = 0; i < 3; i++)
                {
                    bool sendDebtForBankModelResult = await this.SendDebtForBank(new SendDebtForBankModel
                    {
                        Currency = creditInfo.Currency,
                        OrderId = creditInfo.OrderId,
                        Remark = creditInfo.Remark,
                        Status = creditInfo.Status,
                        SubInvestOrderList = listDeptsimpleInfos,
                        TotalAmount = creditInfo.TotalAmount,
                        UpdatedTime = creditInfo.UpdatedTime,
                        UserId = creditInfo.UserId
                    });
                    if (sendDebtForBankModelResult)
                    {
                        break;
                    }
                    if (i == 4)
                    {
                        MessageBox.Show("批量债转通知银行失败,请稍后重试!");
                        return;
                    }
                }
            }
            await this.ConfirmDebtInfo(requeBatch); //债转确认
            this.txb_ProcessNums.Text = listDeptsimpleInfos.Count.ToString();
            List<string> deptIds = listDeptsimpleInfos.Select(x => x.DebtToTransferId).ToList();
            Thread threadDoAll = new Thread(async () =>
            {
                while (listDeptsimpleInfos.Count > 0)
                {
                    //先进行确认
                    await this.ConfirmDebtInfo(requeBatch);
                    //开始执行一个
                    //string subOrderId = deptSimpleInfo.SubOrderId;
                    List<DeptModel> listdeptInfos = this.GetAllDeptModels1(listDeptsimpleInfos[0].RedemptionOrderId, deptIds);
                    for (int i = 0; i < listdeptInfos.Count; i++)
                    {
                        DeptModel deptModel = listdeptInfos[i];
                        SubInvestOrderInputDto deptSimpleInfo = listDeptsimpleInfos.FirstOrDefault(x => x.DebtToTransferId == deptModel.DebtToTransferId);
                        //1.先调网关判断是否正确
                        if (deptSimpleInfo == null)
                        {
                            listDeptsimpleInfos.RemoveRange(0, listDeptsimpleInfos.Count);
                        }
                        string subOrderId = deptSimpleInfo?.SubOrderId;
                        string deptId = deptSimpleInfo?.DebtToTransferId;
                        bool getwayResult = await this.GetOrderSearch(subOrderId);
                        if (getwayResult)
                        {
                            //2.存在网关数据 获取debtToTransferId  AssetDebtRebates 是否status=1
                            if (deptModel != null)
                            {
                                //AdvanceDebtRequest request = this.GetAdvanceDebtRequest(deptId);
                                //调用放款接口
                                AdvanceDebtRequest request = new AdvanceDebtRequest();
                                //调用放款接口
                                bool grantResult = await this.SendAdvanceDebt(request);
                                bool result = await this.GetBatchordersearch(request.OrderId);
                                //如果成功则正确
                                if (grantResult || result)
                                {
                                    //放款确认
                                    bool confirmAdvanceDebtResult = await this.ConfirmAdvanceDebt(new ConfirmAdvanceDebtRequest
                                    {
                                        MerchantId = this.loadAppSettings.MerchantId,
                                        Message = "成功",
                                        OrderId = request.OrderId,
                                        Remark = "",
                                        Status = "S"
                                    });
                                    if (confirmAdvanceDebtResult)
                                    {
                                        listDeptsimpleInfos.Remove(deptSimpleInfo);
                                        this.txb_hasProcessNums.Text = (Convert.ToInt32(this.txb_hasProcessNums.Text) + 1).ToString();
                                        Logger.LoadData(@"DebtInfo\sucess_" + subOrderId + ".txt", subOrderId);
                                    }
                                }
                            }
                        }
                    }
                }
                //            this.btn_StartProcess.Enabled = false;
                this.btn_StartProcess.Enabled = true;
                this.lbl_showmsg.Text = "提示:操作完成......";
            });
            threadDoAll.IsBackground = true;
            threadDoAll.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //获取所有的数据
                this.btn_ZZ.Enabled = false;
                this.lbl_showZZMsg.Text = "正在准备数据,请稍等.......";
                CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
                TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                    .Where("Status eq 0");
                List<string> handleUserIds = deptTable.ExecuteQuery(queryDeptInfo).Select(x => x.NewUserId).Distinct().ToList();
                //处理
                //List<string> handleUserIds = newUserIds.Skip(startIndex).Take(endIndex - startIndex).ToList();
                //获取所有的数据
                if (handleUserIds.Count == 0)
                {
                    this.btn_ZZ.Enabled = true;
                    this.lbl_showZZMsg.Text = "未找到数据";
                    return;
                }
                CloudTable deptTableBatch = this.tableClient.GetTableReference("AssetBatchBookCreditAssignments");
                string partitionKeyBatch = "BatchBookCreditAssignment";
                TableQuery<BatchBookCreditInputDto> queryBatch = new TableQuery<BatchBookCreditInputDto>()
                    .Where($"PartitionKey eq '{partitionKeyBatch}'");
                List<BatchBookCreditInputDto> creditInfos = deptTableBatch.ExecuteQuery(queryBatch).Where(x => handleUserIds.Contains(x.UserId)).ToList();
                if (handleUserIds.Count == 0)
                {
                    this.btn_ZZ.Enabled = true;
                    this.lbl_showZZMsg.Text = "未找到数据";
                    return;
                }
                this.txb_TotalNums.Text = creditInfos.Count.ToString();
                this.txb_ZZNums.Text = "0";
                this.lbl_showZZMsg.Text = "正在开始做批量债转以及债转确认操作.......";
                int count = creditInfos.Count;
                int i;
                for (i = 0; i < this.loadAppSettings.AssetThreadNums; i++)
                {
                    Thread thread = new Thread(async () =>
                    {
                        while (creditInfos.Count > 0)
                        {
                            BatchBookCreditInputDto batchBookCreditInputDto = null;
                            if (creditInfos.Count > 0)
                            {
                                lock (this.objLock)
                                {
                                    if (creditInfos.Count > 0)
                                    {
                                        batchBookCreditInputDto = creditInfos[0];
                                        if (batchBookCreditInputDto != null)
                                        {
                                            creditInfos.Remove(batchBookCreditInputDto);
                                        }
                                    }
                                }
                                if (batchBookCreditInputDto != null)
                                {
                                    //执行数据
                                    this.lbl_showZZMsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}准备开始执行批量债转以及确认债转.....";
                                    //开始执行
                                    List<SubInvestOrderInputDto> listDeptsimpleInfos = batchBookCreditInputDto.SubInvestOrderList.FromJson<List<SubInvestOrderInputDto>>();
                                    NotifyBatchCreditRequest requeBatch = this.MapToConfirmDebt(batchBookCreditInputDto, listDeptsimpleInfos);
                                    bool batchOrderSearch = await this.GetBatchordersearch(batchBookCreditInputDto.RowKey);
                                    bool isReturn = false;
                                    this.lbl_showZZMsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}执行批量债转";
                                    if (!batchOrderSearch)
                                    {
                                        //通知银行
                                        for (i = 0; i < 5; i++)
                                        {
                                            bool sendDebtForBankModelResult = await this.SendDebtForBank(new SendDebtForBankModel
                                            {
                                                Currency = batchBookCreditInputDto.Currency,
                                                OrderId = batchBookCreditInputDto.OrderId,
                                                Remark = batchBookCreditInputDto.Remark,
                                                Status = batchBookCreditInputDto.Status,
                                                SubInvestOrderList = listDeptsimpleInfos,
                                                TotalAmount = batchBookCreditInputDto.TotalAmount,
                                                UpdatedTime = batchBookCreditInputDto.UpdatedTime,
                                                UserId = batchBookCreditInputDto.UserId
                                            });
                                            if (sendDebtForBankModelResult)
                                            {
                                                break;
                                            }
                                            if (i == 4)
                                            {
                                                isReturn = true;
                                            }
                                        }
                                    }
                                    if (isReturn)
                                    {
                                        count = count - 1;
                                        this.lbl_showZZMsg.Text = $"Rowkey:{batchBookCreditInputDto.RowKey}债转批量通知银行失败已经跳过这个";
                                        Logger.LoadData(@"BatchZZInfos\ErrorRowKeys" + batchBookCreditInputDto.RowKey + ".txt", batchBookCreditInputDto.RowKey);
                                        continue;
                                    }
                                    await this.ConfirmDebtInfo(requeBatch); //债转确认
                                    List<string> debtToTransferIds = listDeptsimpleInfos.Select(x => x.DebtToTransferId).ToList();
                                    //一直confirmdebtInfo 使status都为1
                                    this.lbl_showZZMsg.Text = "正在执行MessageFromBank/ConfirmDebt保证DebtToTransfer status都为1";
                                    for (int j = 0; j < 5; j++)
                                    {
                                        await this.ConfirmDebtInfo(requeBatch);
                                        //再获取所有status==1的
                                        List<DeptModel> listDeptInfos = this.GetAllDeptModels1(listDeptsimpleInfos[0].RedemptionOrderId, debtToTransferIds);
                                        //日志记下
                                        if (listDeptInfos.Count == debtToTransferIds.Count)
                                        {
                                            break;
                                        }
                                    }
                                    this.txb_ZZNums.Text = (Convert.ToInt32(this.txb_ZZNums.Text) + 1).ToString();
                                    count = count - 1;
                                }
                            }
                        }
                    });
                    thread.IsBackground = true;
                    thread.Start();
                }
                Thread thShow = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(2000);
                    }
                    //Thread.Sleep(15000);
                    //this.btn_ZZ.Enabled = true;
                    //this.lbl_showZZMsg.Text = "全部执行完毕";
                });
                thShow.IsBackground = true;
                thShow.Start();
            }
            catch (Exception exception)
            {
                this.btn_ZZ.Enabled = true;
                this.lbl_showZZMsg.Text = "发生错误:" + exception.Message;
            }
        }

        //确认放款成功
        private async Task<bool> ConfirmAdvanceDebt(ConfirmAdvanceDebtRequest request)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}MessageFromBank/ConfirmAdvanceDebt", request);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                if (response.IsTrue)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        /// <summary>
        ///     债转确认消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<bool> ConfirmDebt(NotifyBatchCreditRequest request)
        {
            //MessageFromBank/ConfirmDebt
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}MessageFromBank/ConfirmDebt", request);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                if (response.IsTrue)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        private async Task ConfirmDebtInfo(NotifyBatchCreditRequest requeBatch)
        {
            //执行债转确认
            //先调用债转确认信息
            for (int i = 0; i < 5; i++)
            {
                bool confirmDebtResult = await this.ConfirmDebt(requeBatch);
                if (confirmDebtResult)
                {
                    break;
                }
            }
        }

        //private List<AdvanceDebtRequest> GetAdvancedDebtRequests(string redeemOrderId)
        //{
        //    string sql = $"select * from AssetAdvanceDebts where redemptionOrderId='{redeemOrderId}'";
        //    DataTable dtAdvanceRequsts = SqlHelper.SqlHelper.ExecuteDataTable(sql);
        //    List<AdvanceDebtRequest> listResult=new List<AdvanceDebtRequest>();
        //    for (int i = 0; i < dtAdvanceRequsts.Rows.Count; i++)
        //    {
        //        DataRow dr = dtAdvanceRequsts.Rows[i];
        //            var subOrderList = JsonConvert.DeserializeObject<List<CreditAssignmentOrderModel>>(dr["SubOrderList"].ToString())
        //                .Select(subOrder => new CreditAssignmentOrderModel
        //                {
        //                    CreditAssignOrderId = subOrder.CreditAssignOrderId,
        //                    Fee = subOrder.Fee,
        //                    ReceiveUserId = subOrder.ReceiveUserId,
        //                    SubOrderId = subOrder.SubOrderId
        //                })
        //                .ToList();
        //            var request = new AdvanceDebtRequest
        //            {
        //                //Currency = dr["Currency"].ToString(),
        //                //InvestOrderId = dr["Currency"].ToString(),
        //                //OrderId = entity.OrderId,
        //                //TotalAmount = entity.TotalAmount,
        //                //TotalFee = entity.TotalFee,
        //                //TotalNum = entity.TotalNum,
        //                //UserId = entity.UserId,
        //                //SubOrderList = subOrderList,
        //                //DebtId = entity.DebtId,
        //                //DebtToTransferId = entity.DebtToTransferId,
        //                //RedemptionOrderId = entity.RedemptionOrderId,
        //                //Status = entity.Status,
        //                //UpdatedTime = entity.UpdatedTime
        //            };
        //    }
        //}

        private AdvanceDebtInputDto GetAdvanceDebtInfo(string deptId)
        {
            try
            {
                CloudTable deptTable = this.tableClient.GetTableReference("AssetAdvanceDebts");
                string partitionKey = "AdvanceDebt";
                TableQuery<AdvanceDebtInputDto> queryDeptInfo = new TableQuery<AdvanceDebtInputDto>()
                    .Where($"PartitionKey eq '{partitionKey}' and DebtToTransferId eq '{deptId}'");
                AdvanceDebtInputDto entity = deptTable.ExecuteQuery(queryDeptInfo).FirstOrDefault();
                return entity;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"DebtInfo\Error.txt", e.Message);
                return null;
            }
        }

        private List<AdvanceDebtInputDto> GetAdvanceDebtInfos(List<string> deptIds)
        {
            try
            {
                CloudTable deptTable = this.tableClient.GetTableReference("AssetAdvanceDebts");
                string partitionKey = "AdvanceDebt";
                TableQuery<AdvanceDebtInputDto> queryDeptInfo = new TableQuery<AdvanceDebtInputDto>()
                    .Where($"PartitionKey eq '{partitionKey}'");
                List<AdvanceDebtInputDto> entitys = deptTable.ExecuteQuery(queryDeptInfo).Where(x => deptIds.Contains(x.DebtToTransferId)).ToList();
                return entitys;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"DebtInfo\Error.txt", e.Message);
                return new List<AdvanceDebtInputDto>();
            }
        }

        private List<AdvanceDebtInputDto> GetAdvanceDebtInfosBySql(List<string> deptIds)
        {
            try
            {
                CloudTable deptTable = this.tableClient.GetTableReference("AssetAdvanceDebts");
                string partitionKey = "AdvanceDebt";
                TableQuery<AdvanceDebtInputDto> queryDeptInfo = new TableQuery<AdvanceDebtInputDto>()
                    .Where($"PartitionKey eq '{partitionKey}'");
                List<AdvanceDebtInputDto> entitys = deptTable.ExecuteQuery(queryDeptInfo).Where(x => deptIds.Contains(x.DebtToTransferId)).ToList();
                return entitys;
            }
            catch (Exception e)
            {
                Logger.LoadData(@"DebtInfo\Error.txt", e.Message);
                return new List<AdvanceDebtInputDto>();
            }
        }

        //获取放款对象
        private AdvanceDebtRequest GetAdvanceDebtRequest(AdvanceDebtInputDto entity)
        {
            //AssetAdvanceDebts
            try
            {
                //CloudTable deptTable = this.tableClient.GetTableReference("AssetAdvanceDebts");
                //string partitionKey = "AdvanceDebt";
                //TableQuery<AdvanceDebtInputDto> queryDeptInfo = new TableQuery<AdvanceDebtInputDto>()
                //    .Where($"PartitionKey eq '{partitionKey}' and DebtToTransferId eq '{deptToTransferId}'");
                //AdvanceDebtInputDto entity = deptTable.ExecuteQuery(queryDeptInfo).FirstOrDefault();
                if (entity != null)
                {
                    var subOrderList = JsonConvert.DeserializeObject<List<CreditAssignmentOrderModel>>(entity.SubOrderList)
                        .Select(subOrder => new CreditAssignmentOrderModel
                        {
                            CreditAssignOrderId = subOrder.CreditAssignOrderId,
                            Fee = subOrder.Fee,
                            ReceiveUserId = subOrder.ReceiveUserId,
                            SubOrderId = subOrder.SubOrderId
                        })
                        .ToList();
                    var request = new AdvanceDebtRequest
                    {
                        Currency = entity.Currency,
                        InvestOrderId = entity.InvestOrderId,
                        OrderId = entity.OrderId,
                        TotalAmount = entity.TotalAmount,
                        TotalFee = entity.TotalFee,
                        TotalNum = entity.TotalNum,
                        UserId = entity.UserId,
                        SubOrderList = subOrderList,
                        DebtId = entity.DebtId,
                        DebtToTransferId = entity.DebtToTransferId,
                        RedemptionOrderId = entity.RedemptionOrderId,
                        Status = entity.Status,
                        UpdatedTime = entity.UpdatedTime
                    };
                    return request;
                }
                return null;
            }
            catch (Exception ex)
            {
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message);
                return null;
            }
        }

        //判断status是否为1
        private DeptModel GetAllDeptModels(string ransomOrderId, string debtId)
        {
            CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
            TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                .Where($"PartitionKey eq 'DebtToTransfer_{ransomOrderId}' and DebtToTransferId eq '{debtId}'");
            DeptModel deptModel = deptTable.ExecuteQuery(queryDeptInfo).FirstOrDefault();
            return deptModel;
        }

        private List<DeptModel> GetAllDeptModels1(string ransomOrderId, List<string> deptIds)
        {
            CloudTable deptTable = this.tableClient.GetTableReference("AssetDebtToTransfer");
            TableQuery<DeptModel> queryDeptInfo = new TableQuery<DeptModel>()
                .Where($"PartitionKey eq 'DebtToTransfer_{ransomOrderId}'");
            List<DeptModel> deptModel = deptTable.ExecuteQuery(queryDeptInfo).Where(x => deptIds.Contains(x.DebtToTransferId) && x.Status == 1).ToList();
            return deptModel;
        }

        /// <summary>
        ///     获取网关数据
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetBatchordersearch(string orderId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.BankGatewayUrl}api/business/batchordersearch", new { orderId });
                BatchOrderSearchResponse response = await httpResponseMesage.Content.ReadAsAsync<BatchOrderSearchResponse>();
                if (response.Status == "S")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        /// <summary>
        ///     获取网关数据/api/business/batchordersearch
        /// </summary>
        /// <returns></returns>
        private async Task<bool> GetOrderSearch(string orderId)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.BankGatewayUrl}api/business/ordersearch", new { orderId });
                OrderSearchResponse response = await httpResponseMesage.Content.ReadAsAsync<OrderSearchResponse>();
                if (response.Status == "S")
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        private void lbl_showmsg_Click(object sender, EventArgs e)
        {
        }

        private NotifyBatchCreditRequest MapToConfirmDebt(BatchBookCreditInputDto batchBookCreditInputDto, List<SubInvestOrderInputDto> sublistDtos)
        {
            NotifyBatchCreditRequest request = new NotifyBatchCreditRequest
            {
                MerchantId = this.loadAppSettings.MerchantId,
                OrderId = batchBookCreditInputDto.OrderId,
                Remark = "成功",
                Signature = ""
            };
            List<BatchCreditCreateInputRequest> list = new List<BatchCreditCreateInputRequest>();
            for (int i = 0; i < sublistDtos.Count; i++)
            {
                BatchCreditCreateInputRequest batchCreditCreateInputRequest = new BatchCreditCreateInputRequest
                {
                    FailReason = "成功",
                    Status = "S",
                    SubOrderId = sublistDtos[i].SubOrderId
                };
                list.Add(batchCreditCreateInputRequest);
            }
            request.ResultList = list;
            return request;
        }

        //购买债转通知交易系统
        private async Task<Tuple<bool, string>> PurchaseSendToTrader(TifisfalPurchaseRequestModel request)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.AssetApiUrl}MessageToTifisfal/PurchaseSendToTrader", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return new Tuple<bool, string>(true, "成功");
                }
                return new Tuple<bool, string>(false, response.Message);
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        //赎回通知交易系统/MessageToTifisfal/RedemptionSendToTrader
        private async Task<Tuple<bool, string>> RedemptionSendToTrader(TirisfalUserRedemptionInfoModel request)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.AssetApiUrl}MessageToTifisfal/RedemptionSendToTrader", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return new Tuple<bool, string>(true, "成功");
                }
                return new Tuple<bool, string>(false, response.Message);
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        //
        //调用放款
        private async Task<bool> SendAdvanceDebt(AdvanceDebtRequest request)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.AssetApiUrl}DebtForBank/SendAdvanceDebt", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        //
        //POST /DebtForBank/SendDebtForBank 债转批量通知
        private async Task<bool> SendDebtForBank(SendDebtForBankModel request)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //DebtForBank/SendAdvanceDebt
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.AssetApiUrl}DebtForBank/SendDebtForBank", request);
                CommonResult<bool> response = await httpResponseMesage.Content.ReadAsAsync<CommonResult<bool>>();
                if (response.Result)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"DebtInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        private async Task<bool> UpdateYemRedeemInfo(string userid, string orderId, long alreadyDealtAmount, long remainingAmount, int serviceCharge, int status)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                var modifyUserRedeemRequest = new { UserId = userid, OrderId = orderId, alreadyDealtAmount, remainingAmount, serviceCharge, status };
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/ModifyRedemptionOrderByDisk", modifyUserRedeemRequest);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"UpdateYemRedeemInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }

        private async Task<bool> UserRedeemYemProduct(string userid, string orderId, long alreadyDealtAmount, long remainingAmount, int serviceCharge, int status)
        {
            HttpResponseMessage httpResponseMesage = new HttpResponseMessage();
            try
            {
                //记录下需要插入的资产ids
                var modifyUserRedeemRequest = new { UserId = userid, OrderId = orderId, alreadyDealtAmount, remainingAmount, serviceCharge, status };
                httpResponseMesage = await this.httpClient.PostAsJsonAsync($"{this.loadAppSettings.YemApiUrl}YEMAssetPool/ModifyRedemptionOrderByDisk", modifyUserRedeemRequest);
                ResultResponse<string> response = await httpResponseMesage.Content.ReadAsAsync<ResultResponse<string>>();
                return response.IsTrue;
            }
            catch (Exception ex)
            {
                //log记录日志
                Logger.LoadData(@"UpdateYemRedeemInfo\Error.txt", ex.Message + "---------" + await httpResponseMesage.Content.ReadAsStringAsync());
                return false;
            }
        }
    }
}