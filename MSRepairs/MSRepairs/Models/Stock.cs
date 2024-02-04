using System.Runtime.Serialization;

namespace MSRepairs.Models
{
    [DataContract]
    public class Stock
    {

        [DataMember]
        public string StockId { get; set; }

        [DataMember]
        public string PartName { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public float Price { get; set; }

        public Stock(string stockId, string partName, int quantity, float price)
        {
            StockId = stockId;
            PartName = partName;
            Quantity = quantity;
            Price = price;
        }
    }
}
