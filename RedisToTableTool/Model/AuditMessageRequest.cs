using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToTableTool.Model
{
    /// <summary>
    ///     流程审核操作参数
    /// </summary>
    public class AuditMessageRequest
    {
        /// <summary>
        ///     审核附加消息
        /// </summary>
        /// <value>The audit message.</value>
        public string Comment { get; set; }

        public string FlowStatus { get; set; }

        /// <summary>
        ///     业务主键
        /// </summary>
        public string Primarykey { get; set; }

        ///// <summary>
        /////     审核状态
        ///// </summary>
        //[JsonProperty("Status")]
        //public string Status { get; set; }
        /// <summary>
        ///     审核人
        /// </summary>
        /// <value>The updated by.</value>
        public string UpdatedBy { get; set; }

        /// <summary>
        ///     审核时间
        /// </summary>
        /// <value>The updated time.</value>
        public DateTime UpdatedTime { get; set; }
    }
}