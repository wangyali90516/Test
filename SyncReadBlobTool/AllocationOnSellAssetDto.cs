using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncReadBlobTool
{
    public class AllocationOnSellAssetDto
    {
        /// <summary>
        ///     业务主键
        /// </summary>
        [Description("业务主键")]
        public string OnSellAssetId { get; set; }

        /// <summary>
        ///     该资产剩余金额
        /// </summary>
        public long RemainderTotal { get; set; }
    }
}
