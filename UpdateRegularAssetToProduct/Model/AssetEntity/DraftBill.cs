using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateRegularAssetToProduct
{
    //[NotMapped]
    [Description("银票")]
    public class DraftBill : EntityBase<long>
    {
        /// <summary>
        ///     票背面扫描件
        /// </summary>
        [StringLength(300)]
        public string BackOfScanning { get; set; }

        /// <summary>
        ///     出票-出票日期
        /// </summary>
        /// <value>The bill date.</value>
        [Column(TypeName = "DateTime2")]
        public DateTime BillDate { get; set; }

        /// <summary>
        ///     持有天数
        /// </summary>
        /// <value>The bill own days.</value>
        public long BillOwnDays { get; set; }

        /// <summary>
        ///     实际就是AssetId
        /// </summary>
        /// <value>The draft bill identifier.</value>
        [StringLength(32)]
        public string DraftBillId { get; set; }

        /// <summary>
        ///     出票人全称
        /// </summary>
        [StringLength(100)]
        public string DrawerName { get; set; }

        /// <summary>
        ///     票面扫描件
        /// </summary>
        [StringLength(1024)]
        public string EndorserFin { get; set; }

        /// <summary>
        ///     收票日期
        /// </summary>
        /// <value>The get bill date.</value>
        [Column(TypeName = "DateTime2")]
        [Required]
        public DateTime GetBillDate { get; set; }

        /// <summary>
        ///     收票利息
        /// </summary>
        [Required]
        public long Interest { get; set; }

        /// <summary>
        ///     付款行全称
        /// </summary>
        public string PayBankFullName { get; set; }

        /// <summary>
        ///     付款行行号
        /// </summary>
        [StringLength(100)]
        public string PayingBankNo { get; set; }

        /// <summary>
        ///     收款人账号
        /// </summary>
        /// <value>The receiver account.</value>
        public string ReceiverAccount { get; set; }

        /// <summary>
        ///     收款人开户银行
        /// </summary>
        [StringLength(100)]
        public string ReceiverBank { get; set; }

        /// <summary>
        ///     收款人全称
        /// </summary>
        [StringLength(100)]
        public string ReceiverName { get; set; }

        /// <summary>
        ///     收款方全称
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ReceivingSideFullName { get; set; }

        /// <summary>
        ///     收款方ID
        /// </summary>
        [Required]
        [StringLength(100)]
        public string ReceivingSideId { get; set; }
    }
}
