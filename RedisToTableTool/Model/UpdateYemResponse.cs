namespace RedisToTableTool.Model
{
    public class UpdateYemResponse
    {
        public int BizCode { get; set; }

        public bool IsTrue { get; set; }

        public string Message { get; set; }

        public OnSellAssetResponse Result { get; set; }
    }
}