namespace RedisToTableTool
{
    public class LoadAppSettings
    {
        public string AssetApiUrl { get; set; }
        public string AssetDebtToTransfer { get; set; }
        public int AssetThreadNums { get; set; }
        public string BankGatewayUrl { get; set; }
        public int BatchAssetNums { get; set; }
        public int BatchUserNums { get; set; }
        public int CheckAssetInfoThreadNums { get; set; }
        public int DbNumber { get; set; }
        public int EndDbNumber { get; set; }
        public string MerchantId { get; set; }
        public int ReloadTableDataToDiskThreadNums { get; set; }
        public string SearchAssetAzureTable { get; set; }
        public string SearchUserInfoAzureTable { get; set; }
        public string ServiceBusConnectionString { get; set; }
        public int UserThreadNums { get; set; }
        public string WriteAssetUserTableName { get; set; }
        public string WriteUserAssetTableName { get; set; }
        public string YemApiUrl { get; set; }
    }
}