namespace Shared
{
    using System;
    public class Command
    {
        public Guid Id { get; set; }
        public string commandId { get; set; }
        public string userId { get; set; }
        public string userMail { get; set; }
        public string product { get; set; }
        public string price { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string PaymentId { get; set; }
    }
}