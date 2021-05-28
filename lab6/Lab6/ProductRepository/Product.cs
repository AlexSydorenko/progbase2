using System;

namespace lab6
{
    public class Product
    {
        public long id;
        public string name;
        public string description;
        public double price;
        public bool isExist;
        public DateTime createdAt;

        public Product()
        {
            this.createdAt = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format($"[{id}] - {name} ({price} UAH)");
        }
    }
}
