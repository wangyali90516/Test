using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moe.Lib;

namespace UpdateRegularAssetToProduct
{
    public class RegularAssetService
    {
        static readonly string logAddress = Environment.CurrentDirectory + '\\' +
                                            DateTime.UtcNow.ToChinaStandardTime().Year + '-' +
                                            DateTime.UtcNow.ToChinaStandardTime().Month + '-' +
                                            DateTime.UtcNow.ToChinaStandardTime().Day;
        /// <summary>
        ///查询产品资产表
        /// </summary>
        /// <returns>Task&lt;List&lt;Enterprise&gt;&gt;.</returns>
        public static List<ProductAsset> GetProductAssetList()
        {
            using (AssetDbContext context = new AssetDbContext())
            {
                var ids = context.ReadonlyQuery<ProductAsset>().GroupBy(p => p.ProductId).Where(p => p.Count() > 1).Select(x => x.Key);
                foreach (var item in ids)
                {
                    Log.WriteLog(item, logAddress + "_NoContainsAsset.log");
                }
                return context.ReadonlyQuery<ProductAsset>().Where(p => p.IsDeleted == false && p.ProductId!=p.AssetId && !ids.Contains(p.ProductId)).ToList();
            }
        }

        /// <summary>
        ///批量更新
        /// </summary>
        /// <returns>Task&lt;List&lt;Enterprise&gt;&gt;.</returns>
        public static async Task<bool> UpdateDbByAssetIdAsync(DraftBill draftBill, Asset asset, EnterpriseLoan enterpriseLoan, FinancierPlayMoney financierPlayMoney, FinancierReturnedMoney financierReturnedMoney, FinancierReturnedMoneyDetail financierReturnedMoneyDetail, InvestProjectAsset investProjectAsset, MerchantBill merchantBill, PeerToPeer peerToPeer, PlayMoneyBodyMessage playMoneyBodyMessage, ProductAsset productAsset, UserReturnedMoney userReturnedMoney)
        {
            using (AssetDbContext context = new AssetDbContext())
            {
                using (var tran = context.Database.BeginTransaction())
                {
                    if (draftBill != null)
                    {
                        context.DraftBills.AddOrUpdate(draftBill);
                    }
                    if (asset != null)
                    {
                        context.Assets.AddOrUpdate(asset);
                    }
                    if (enterpriseLoan != null)
                    {
                        context.EnterpriseLoans.AddOrUpdate(enterpriseLoan);
                    }
                    if (financierPlayMoney != null)
                    {
                        context.FinancierPlayMoneys.AddOrUpdate(financierPlayMoney);
                    }
                    if (financierReturnedMoney != null)
                    {
                        context.FinancierReturnedMoneys.AddOrUpdate(financierReturnedMoney);
                    }
                    if (financierReturnedMoneyDetail != null)
                    {
                        context.FinancierReturnedMoneyDetails.AddOrUpdate(financierReturnedMoneyDetail);
                    }
                    if (investProjectAsset != null)
                    {
                        context.InvestProjectAssets.AddOrUpdate(investProjectAsset);
                    }
                    if (merchantBill != null)
                    {
                        context.MerchantBills.AddOrUpdate(merchantBill);
                    }
                    if (peerToPeer != null)
                    {
                        context.PeerToPeers.AddOrUpdate(peerToPeer);
                    }
                    if (playMoneyBodyMessage != null)
                    {
                        context.PlayMoneyBodyMessages.AddOrUpdate(playMoneyBodyMessage);
                    }
                    if (productAsset != null)
                    {
                        context.ProductAssets.AddOrUpdate(productAsset);
                    }
                    if (userReturnedMoney != null)
                    {
                        context.UserReturnedMoneys.AddOrUpdate(userReturnedMoney);
                    }
                    var result = await context.SaveChangesAsync() > 0;
                    if (result)
                    {
                        tran.Commit();
                    }
                    return result;
                }
            }
        }

        /// <summary>
        ///查询银票资产
        /// </summary>
        /// <returns>Task&lt;List&lt;Enterprise&gt;&gt;.</returns>
        public static DraftBill GetDraftBillByAssetId(string assetId)
        {
            using (AssetDbContext context = new AssetDbContext())
            {
                return context.ReadonlyQuery<DraftBill>().FirstOrDefault(p => p.IsDeleted == false);
            }
        }

    }
}
