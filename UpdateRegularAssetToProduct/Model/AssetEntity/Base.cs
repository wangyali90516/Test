using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moe.Lib;

namespace UpdateRegularAssetToProduct
{
    /// <summary>
    ///     Class EntityBase.
    /// </summary>
    public abstract class Base<TKey>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Base{TKey}" /> class.
        /// </summary>
        protected Base()
        {
            //this.CreatedBy = "System";
            this.CreatedTime = DateTime.UtcNow.ToChinaStandardTime();
        }

        /// <summary>
        ///     创建人
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        [Column(TypeName = "DateTime2")]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        ///     主键标识
        /// </summary>
        [Key]
        public TKey Id { get; set; }
    }
}
