using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RedisToTableTool.Model
{
    public class AssetUserRelationResponse
    {
        /// <summary>
        ///     资产Id
        /// </summary>
        /// <value>The asset identifier.</value>
        public string AssetId { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        /// <value>The created time.</value>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///     本金之和
        /// </summary>
        public long SumCapital { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        /// <value>The updated time.</value>
        public DateTime UpdatedTime { get; set; }

        /// <summary>
        ///     用户资产比例列表
        /// </summary>
        /// <value>The user asset ratio dtos.</value>
        public List<UserAssetRatioResponse> UserAssetRatioDtos { get; set; }
    }

    /// <summary>
    ///     用户资产比例
    /// </summary>
    public class UserAssetRatioResponse
    {
        /// <summary>
        ///     本金
        /// </summary>
        /// <value>The capital.</value>
        [Description("本金")]
        public long Capital { get; set; }
    }
}