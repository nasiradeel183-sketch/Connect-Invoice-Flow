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

        // FLOWSTEP: Entry point – invoice received from Rillion
        public void Process(Invoice invoice)
        {
            // FLOWSTEP: Determine invoice source
            var source = invoice.Source;

            // FLOWSTEP: Apply source-specific mapping
            var mapped = _mapping.Map(invoice);

            // FLOW DECISION: Is supplier active in ERP?
            var isValid = _validation.Validate(mapped);

            if (!isValid)
            {
                // FLOW NO: Reject invoice with error
                Console.WriteLine("Invoice rejected");
                return;
            }

            // FLOW YES: Continue processing
            SendToErp(mapped);
        }

        private void SendToErp(MappedInvoice invoice)
        {
            // FLOWSTEP: ERP API call
            Console.WriteLine("Sending invoice to ERP...");
        }
    }

    public class Invoice
    {
        public string Source { get; set; }
    }
}
