using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToTableTool.Model
{
    public class BaseRequest
    {
        /// <summary>
        ///     创建人
        /// </summary>
        /// <value>The created by.</value>
        public string CreatedBy { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///     删除标记
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     更新人
        /// </summary>
        /// <value>The updated by.</value>
        public string UpdatedBy { get; set; }

        /// <summary>
        ///     更新时间
        /// </summary>
        public DateTime? UpdatedTime { get; set; }
    }
}