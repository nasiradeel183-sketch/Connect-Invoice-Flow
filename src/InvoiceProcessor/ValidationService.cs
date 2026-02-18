using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.InvoiceProcessor
{
    public class ValidationService
    {
        // FLOW LINK: Validate header, supplier, totals, currency
        public bool Validate(MappedInvoice invoice)
        {
            return true;
        }
    }

    public class MappedInvoice
    {
        public string Supplier { get; set; }
    }

}
