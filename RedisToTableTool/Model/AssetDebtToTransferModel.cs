using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToTableTool.Model
{
    public class AssetDebtToTransferModel
    {
        /// <summary>
        ///     债转关系主键
        /// </summary>
        [Description("债转关系主键")]
        public string DebtToTransferId { get; set; }

        /// <summary>
        ///     老用户-资产标识
        /// </summary>
        [Description("老用户-资产标识")]
        public string OldUserAssetRatioId { get; set; }

        /// <summary>
        ///     老买用户
        /// </summary>
        [Description("老买用户")]
        public string OldUserId { get; set; }
    }
}