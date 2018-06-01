using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moe.Lib;

namespace UpdateRegularAssetToProduct
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EntityBase<TKey> : Base<TKey>
    {
        /// <summary>
        /// </summary>
        protected EntityBase()
        {
            this.IsDeleted = false;

            this.UpdatedTime = DateTime.UtcNow.ToChinaStandardTime();
        }

        /// <summary>
        ///     删除标记
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     更新人
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        [Column(TypeName = "DateTime2")]
        public DateTime? UpdatedTime { get; set; }
    }
}