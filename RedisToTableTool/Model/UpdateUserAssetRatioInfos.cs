namespace RedisToTableTool.Model
{
    public class UpdateUserAssetRatioInfos
    {
        public string AssetId { get; set; }
        public long Capital { get; set; }
        public long DealAmount { get; set; }
        public long Denominator { get; set; }
        public long Id { get; set; }
        public long Numerator { get; set; }
        public string OriginalUserAssetRatioId { get; set; }
        public string UserAssetRatioId { get; set; }
        public string UserId { get; set; }
    }
}