using System;
using System.Collections.Generic;
using System.Text;

namespace myvsdurablefunctions
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public string RowKey { get; set; }
        public string PartitionKey { get; set; }
        public DateTime PaymentDate { get; set; }
        public double Amount { get; set; }
    }
}
