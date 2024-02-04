using System.Runtime.Serialization;
using static MSUsers.Tools.MSUsersEnums;

namespace MSUsers.Models
{
    [DataContract]
    public class Vehicle
    {

        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public string VehicleType { get; set; }

        public Vehicle(string licensePlate, string vehicleType)
        {
            LicensePlate = licensePlate;
            VehicleType = vehicleType;
        }
    }

}
