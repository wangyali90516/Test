using System;
using System.ComponentModel;
using Microsoft.WindowsAzure.Storage.Table;

namespace JymTool
{
    /// <summary>
    ///     资产Dto基类
    /// </summary>
    [Description("资产Dto基类")]
    public class AssetBaseDto: TableEntity
    {
        /// <summary>
        ///     创建人
        /// </summary>
        [Description("创建人")]
        public string CreatedBy { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        ///     删除标记
        /// </summary>
        [Description("删除标记")]
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     更新人
        /// </summary>
        [Description("更新人")]
        public string UpdatedBy { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Description("更新时间")]
        public DateTime? UpdatedTime { get; set; }
    }
}