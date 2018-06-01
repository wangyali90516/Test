using System;
using System.Windows.Forms;
using WindowsFormsMigrationData;
using Microsoft.WindowsAzure.Storage.Table;

namespace RedisToTableTool
{
    public partial class SearchRedisForm : Form
    {
        private readonly LoadAppSettings loadAppSettings;
        private readonly object objLock = new object();
        private readonly CloudTableClient tableClient;

        public SearchRedisForm(LoadAppSettings loadAppSettings, CloudTableClient tableClient)
        {
            this.loadAppSettings = loadAppSettings;
            this.tableClient = tableClient;
            CheckForIllegalCrossThreadCalls = false;
            this.InitializeComponent();
        }

        /// <summary>
        ///     拉去redis内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_checkAllAsset_Click(object sender, EventArgs e)
        {
            try
            {
                int redisNumber = Convert.ToInt32(this.txb_RedisNumber.Text.Trim());
                string redisKeyname = this.txb_RedisKey.Text.Trim();
                if (string.IsNullOrEmpty(redisKeyname))
                {
                    MessageBox.Show("请输入redisKeyName.......");
                    return;
                }
                //生成redisHelper
                RedisHelperSpecial redisHelperSpecial = new RedisHelperSpecial(redisNumber);
                //获取redisContent
                string content = await redisHelperSpecial.GetRedisContentAsync(redisKeyname);
                //存入到文本文件中
                Logger.LoadData(@"RedisContent/content_" + redisKeyname + ".txt", content);
                MessageBox.Show("获取rediscontent成功,请到bin目录下RedisContent文件夹中查看");
            }
            catch (Exception exception)
            {
                MessageBox.Show("获取失败,失败错误信息为" + exception.Message);
            }
        }
    }
}