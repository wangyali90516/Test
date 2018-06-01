using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moe.Lib.Data;

namespace UpdateRegularAssetToProduct
{
    public sealed class AssetDbContext : DbContextBase
    {
        private static readonly Lazy<string> assetDbContext = new Lazy<string>(() => ConfigurationManager.AppSettings["assetConn"]);

        /// <summary>
        ///     Initializes a new instance of the <see cref="assetDbContextConnectionString" /> class.
        /// </summary>
        public AssetDbContext()
            : base(assetDbContextConnectionString)
        {
        }


        public DbSet<Asset> Assets { get; set; }
        public DbSet<DraftBill> DraftBills { get; set; }
        public DbSet<EnterpriseLoan> EnterpriseLoans { get; set; }
        public DbSet<FinancierPlayMoney> FinancierPlayMoneys { get; set; }
        public DbSet<FinancierReturnedMoney> FinancierReturnedMoneys { get; set; }
        public DbSet<FinancierReturnedMoneyDetail> FinancierReturnedMoneyDetails { get; set; }
        public DbSet<InvestProjectAsset> InvestProjectAssets { get; set; }
        public DbSet<MerchantBill> MerchantBills { get; set; }
        public DbSet<PeerToPeer> PeerToPeers { get; set; }
        public DbSet<PlayMoneyBodyMessage> PlayMoneyBodyMessages { get; set; }
        public DbSet<ProductAsset> ProductAssets { get; set; }
        public DbSet<UserReturnedMoney> UserReturnedMoneys { get; set; }

        private static string assetDbContextConnectionString
        {
            get { return assetDbContext.Value; }
        }

        /// <summary>
        ///     Gets or sets the user information.
        /// </summary>
        /// <value>The user information.</value>
        /// <summary>
        ///     Called when [model creating].
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
