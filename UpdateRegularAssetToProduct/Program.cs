using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moe.Lib;


namespace UpdateRegularAssetToProduct
{
    /// <summary>
    /// 将定期的AssetId改成ProductId，通过ProductAsset关联
    /// </summary>
    class Program
    {
        static readonly string logAddress = Environment.CurrentDirectory + '\\' +
                                            DateTime.UtcNow.ToChinaStandardTime().Year + '-' +
                                            DateTime.UtcNow.ToChinaStandardTime().Month + '-' +
                                            DateTime.UtcNow.ToChinaStandardTime().Day;
        static void Main(string[] args)
        {
            var productAssetList = RegularAssetService.GetProductAssetList();
            Console.WriteLine("总共需要处理的条数=" + productAssetList.Count);
            int i = 0;
            foreach (var item in productAssetList)
            {
                i++;
                Console.WriteLine("当前处理的是第" + i + "条");
                using (AssetDbContext context = new AssetDbContext())
                {
                    List<DraftBill> draftBills = context.ReadonlyQuery<DraftBill>().Where(p => p.IsDeleted == false && p.DraftBillId == item.AssetId || p.DraftBillId == item.ProductId).ToList();
                    DraftBill draftBill = null;
                    if (draftBills.Count == 1)
                    {
                        draftBill = draftBills.FirstOrDefault(p => p.DraftBillId == item.AssetId);
                        if (draftBill != null)
                            draftBill.DraftBillId = item.ProductId;
                    }

                    List<Asset> assets = context.ReadonlyQuery<Asset>().Where(p => p.IsDeleted == false && p.AssetId == item.AssetId || p.AssetId == item.ProductId).ToList();
                    Asset asset = null;
                    if (assets.Count == 1)
                    {
                        asset = assets.FirstOrDefault(p => p.AssetId == item.AssetId);
                        if (asset != null)
                        {
                            asset.AssetId = item.ProductId;
                        }
                    }

                    List<EnterpriseLoan> enterpriseLoans = context.ReadonlyQuery<EnterpriseLoan>().Where(p => p.IsDeleted == false && p.EnterpriseLoanId == item.AssetId && p.EnterpriseLoanId != item.ProductId).ToList();
                    EnterpriseLoan enterpriseLoan = null;
                    if (enterpriseLoans.Count == 1)
                    {
                        enterpriseLoan = enterpriseLoans.FirstOrDefault(p => p.EnterpriseLoanId == item.AssetId);
                        if (enterpriseLoan != null)
                        {
                            enterpriseLoan.EnterpriseLoanId = item.ProductId;
                        }
                    }

                    List<FinancierPlayMoney> financierPlayMoneys = context.ReadonlyQuery<FinancierPlayMoney>().Where(p => p.IsDeleted == false && p.AssetId == item.AssetId || p.AssetId == item.ProductId).ToList();
                    FinancierPlayMoney financierPlayMoney = null;
                    if (financierPlayMoneys.Count == 1)
                    {
                        financierPlayMoney = financierPlayMoneys.FirstOrDefault(p => p.AssetId == item.AssetId);
                        if (financierPlayMoney != null)
                        {
                            financierPlayMoney.AssetId = item.ProductId;
                        }
                    }


                    List<FinancierReturnedMoney> financierReturnedMoneys = context.ReadonlyQuery<FinancierReturnedMoney>().Where(p => p.IsDeleted == false && p.AssetId == item.AssetId || p.AssetId == item.ProductId).ToList();
                    FinancierReturnedMoney financierReturnedMoney = null;
                    if (financierReturnedMoneys.Count == 1)
                    {
                        financierReturnedMoney = financierReturnedMoneys.FirstOrDefault(p => p.AssetId == item.AssetId);
                        if (financierReturnedMoney != null)
                        {
                            financierReturnedMoney.AssetId = item.ProductId;
                        }
                    }

                    List<FinancierReturnedMoneyDetail> financierReturnedMoneyDetails = context.ReadonlyQuery<FinancierReturnedMoneyDetail>().Where(p => p.IsDeleted == false && p.AssetId == item.AssetId || p.AssetId == item.ProductId).ToList();
                    FinancierReturnedMoneyDetail financierReturnedMoneyDetail = null;
                    if (financierReturnedMoneyDetails.Count == 1)
                    {
                        financierReturnedMoneyDetail = financierReturnedMoneyDetails.FirstOrDefault(p => p.AssetId == item.AssetId);
                        if (financierReturnedMoneyDetail != null)
                        {
                            financierReturnedMoneyDetail.AssetId = item.ProductId;
                        }
                    }

                    List<InvestProjectAsset> investProjectAssets = context.ReadonlyQuery<InvestProjectAsset>().Where(p => p.IsDeleted == false && p.AssetId == item.AssetId || p.AssetId == item.ProductId).ToList();
                    InvestProjectAsset investProjectAsset = null;
                    if (investProjectAssets.Count == 1)
                    {
                        investProjectAsset = investProjectAssets.FirstOrDefault(p => p.AssetId == item.AssetId);
                        if (investProjectAsset != null)
                        {
                            investProjectAsset.AssetId = item.ProductId;
                        }
                    }

                    List<MerchantBill> merchantBills = context.ReadonlyQuery<MerchantBill>().Where(p => p.IsDeleted == false && p.MerchantBillId == item.AssetId || p.MerchantBillId == item.ProductId).ToList();
                    MerchantBill merchantBill = null;
                    if (merchantBills.Count == 1)
                    {
                        merchantBill = merchantBills.FirstOrDefault(p => p.MerchantBillId == item.AssetId);
                        if (merchantBill != null)
                        {
                            merchantBill.MerchantBillId = item.ProductId;
                        }

                    }

                    List<PlayMoneyBodyMessage> playMoneyBodyMessages = context.ReadonlyQuery<PlayMoneyBodyMessage>().Where(p => p.IsDeleted == false && p.AssetId == item.AssetId || p.AssetId == item.ProductId).ToList();
                    PlayMoneyBodyMessage playMoneyBodyMessage = null;
                    if (playMoneyBodyMessages.Count == 1)
                    {
                        playMoneyBodyMessage = playMoneyBodyMessages.FirstOrDefault(p => p.AssetId == item.AssetId);
                        if (playMoneyBodyMessage != null)
                        {
                            playMoneyBodyMessage.AssetId = item.ProductId;
                        }
                    }

                    List<PeerToPeer> peerToPeers = context.ReadonlyQuery<PeerToPeer>().Where(p => p.IsDeleted == false && p.PeerToPeerId == item.AssetId || p.PeerToPeerId == item.ProductId).ToList();
                    PeerToPeer peerToPeer = null;
                    if (peerToPeers.Count == 1)
                    {
                        peerToPeer = peerToPeers.FirstOrDefault(p => p.PeerToPeerId == item.AssetId);
                        if (peerToPeer != null)
                        {
                            peerToPeer.PeerToPeerId = item.ProductId;
                        }
                    }

                    List<ProductAsset> productAssets = context.ReadonlyQuery<ProductAsset>().Where(p => p.IsDeleted == false && p.AssetId == item.AssetId || p.AssetId == item.ProductId).ToList();
                    ProductAsset productAsset = null;
                    if (productAssets.Count == 1)
                    {
                        productAsset = productAssets.FirstOrDefault(p => p.AssetId == item.AssetId);
                        if (productAsset != null)
                        {
                            productAsset.AssetId = item.ProductId;
                        }
                    }

                    List<UserReturnedMoney> userReturnedMoneys = context.ReadonlyQuery<UserReturnedMoney>().Where(p => p.IsDeleted == false && p.AssetId == item.AssetId || p.AssetId == item.ProductId).ToList();
                    UserReturnedMoney userReturnedMoney = null;
                    if (productAssets.Count == 1)
                    {
                        userReturnedMoney = userReturnedMoneys.FirstOrDefault(p => p.AssetId == item.AssetId);
                        if (userReturnedMoney != null)
                        {
                            userReturnedMoney.AssetId = item.ProductId;
                        }
                    }

                    bool dbResult = RegularAssetService.UpdateDbByAssetIdAsync(draftBill, asset, enterpriseLoan, financierPlayMoney, financierReturnedMoney, financierReturnedMoneyDetail, investProjectAsset, merchantBill, peerToPeer, playMoneyBodyMessage, productAsset, userReturnedMoney).Result;
                    if (dbResult == false)
                    {
                        Log.WriteLog(item.AssetId, logAddress + "_ErrorAsset.log");
                        break;
                    }
                    Log.WriteLog(item.AssetId, logAddress + "_SuccessAsset.log");
                }
            }
            Console.WriteLine("处理完成");
            Console.ReadKey();
        }
    }
}
