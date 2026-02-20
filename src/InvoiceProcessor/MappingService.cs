using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.InvoiceProcessor
{
    
    public class MappingService
    {
        // FLOW STEP: Map invoice fields based on source system
        public MappedInvoice Map(Invoice invoice)
        {
            return new MappedInvoice
            {
                Supplier = "ABC"
            };
        }
    }


}
