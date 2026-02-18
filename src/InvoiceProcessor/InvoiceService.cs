using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.InvoiceProcessor
{
    public class InvoiceService
    {
        private readonly ValidationService _validation;
        private readonly MappingService _mapping;

        public InvoiceService()
        {
            _validation = new ValidationService();
            _mapping = new MappingService();
        }

        // FLOW START: Invoice Received from Rillion
        public void Process(Invoice invoice)
        {
            // FLOW STEP: Determine invoice source
            var source = invoice.Source;

            // FLOW STEP: Check supplier in ERP
            var mapped = _mapping.Map(invoice);

            // FLOW DECISION: Supplier exists?
            var isValid = _validation.Validate(mapped);

            if (!isValid)
            {
                // FLOW NO: Create supplier sync task
                Console.WriteLine("Invoice rejected");
                return;
            }

            // FLOW YES: Submit invoice to ERP
            SendToErp(mapped);
        }

        private void SendToErp(MappedInvoice invoice)
        {
            // FLOW STEP: ERP API call
            Console.WriteLine("Sending invoice to ERP...");
        }
    }

    public class Invoice
    {
        public string Source { get; set; }
    }
}
