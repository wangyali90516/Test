namespace RedisToTableTool
{
    public class ModifyUserAssetRatioRequest
    {
        public string AssetId { get; set; }
        public long Capital { get; set; }
        public long Denominator { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsInvestSuccess { get; set; }
        public bool IsNotifyTradingSuccess { get; set; }
        public bool IsReturned { get; set; }
        public long Numerator { get; set; }
        public string OriginalUserAssetRatioId { get; set; }
        public string Reserve { get; set; }

        public long Status { get; set; }
        public string UserAssetRatioId { get; set; }
        public string UserId { get; set; }
    }
}