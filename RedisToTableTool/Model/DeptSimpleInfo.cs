namespace RedisToTableTool.Model
{
    public class DeptSimpleInfo
    {
        //"Amount": 20535,
        //"AssignorUserId": "AA60D25D96904FB09E45015B3BE3E910",
        //"DebtToTransferId": "BA00BDB8AEA745F9806FBBBD37CAD342",
        //"Discount": 0,
        //"Interest": 78,
        //"InvestOrderId": "26386A7665C64BE68FE642A5BEC19BA8",
        //"RedemptionOrderId": "72FD38FA7BD24A2782F13A80C6CD9799",
        //"Status": 0,
        //"SubOrderId": "3321C29DBD584C44B6CB8E260A8571D8"

        public long Amount { get; set; }
        public string AssignorUserId { get; set; }

        public string DebtToTransferId { get; set; }

        public long Discount { get; set; }

        public long Interest { get; set; }

        public string InvestOrderId { get; set; }

        public string RedemptionOrderId { get; set; }

        public int Status { get; set; }

        public string SubOrderId { get; set; }
    }
}