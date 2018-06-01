using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToTableTool.Model
{
    public class ConfirmAdvanceDebtRequest
    {
        public string MerchantId { get; set; }

        public string Message { get; set; }

        public string OrderId { get; set; }

        public string Remark { get; set; }

        public string Status { get; set; }
    }
}