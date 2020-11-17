using System;
using System.Collections.Generic;
using System.Text;

namespace myvsdurablefunctions
{
    public class Order
    {
        public Order()
        {
            OrderId = Guid.NewGuid();
        }
        public Guid OrderId { get; set; }
        public User User { get; set; }
        public Product Product { get; set; }
        public string Status { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
    }

    public class User
    {
        public string Name { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class Product
    {
        public Product()
        {
            ProductId = Guid.NewGuid();
        }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public double PriceHt { get; set; }
        public double TotalHt => Quantity * PriceHt;
        public double TotalTtc => Quantity * PriceHt * 1.2;
    }
}
