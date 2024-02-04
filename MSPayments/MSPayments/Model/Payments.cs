using System.Runtime.Serialization;

namespace MSPayments.Model
{
    [DataContract]
    public class Payments
    {
        [DataMember]
        public string PaymentId { get; set; }

        [DataMember]
        public string RepairId { get; set; }

        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public string StateId { get; set; }

        [DataMember]
        public string ServiceTypeId { get; set; }

        [DataMember]
        public int Price { get; set; }

        [DataMember]
        public string Date { get; set; }

        public Payments(string paymentId, string repairId, string licensePlate, string stateId, string serviceTypeId, int price, string date)
        {
            PaymentId = paymentId;
            RepairId = repairId;
            LicensePlate = licensePlate;
            StateId = stateId;
            ServiceTypeId = serviceTypeId;
            Price = price;
            Date = date;
        }
    }
}
