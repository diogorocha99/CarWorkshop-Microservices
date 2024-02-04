using System.Runtime.Serialization;

namespace MSGarages.Models
{
    public class Garage
    {
        [DataMember]
        public int GarageId { get; set; }

        [DataMember]
        public string Adress { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Postal_code { get; set; }


        public Garage(int garageId, string adress, string name, string postal_code )
        {

            GarageId = garageId;
            Adress = adress;
            Name = name;
            Postal_code = postal_code;

        }
    }
}
