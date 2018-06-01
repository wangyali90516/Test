using System.ComponentModel;

namespace RedisToTableTool.Model
{
    /// <summary>
    ///     Class OnSellAssetResponse.
    /// </summary>
    public class OnSellAssetResponse
    {
        /// <summary>
        ///     已经计算过后的融资金额
        /// </summary>
        [Description("已经计算过后的融资金额")]
        public long CalculatedAmount { get; set; }

        /// <summary>
        ///     该资产剩余金额
        /// </summary>
        public long RemainderTotal { get; set; }
    }
}