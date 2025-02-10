using ProjectPos.Data.AggregateRoots;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPos.Data.EntityModels
{
    public class PurchaceInvoiceLine : BasicAggregateRoot<int>
    {
        public int ProductId { get; set; }
        public int InvoiceNumber { get; set; }
        public int Quantity { get; set; }

        [Column(TypeName = "double(12, 2)")]
        public double UnitPrice { get; set; }

        [Column(TypeName = "double(12, 2)")]
        public decimal TotalPrice { get; set; }

        [ForeignKey("InvoiceNumber")]
        public PurchaceInvoice? Invoice { get; set; }

        [ForeignKey("ProductId")]
        public ProductInventory? Product { get; set; }
    }
}
