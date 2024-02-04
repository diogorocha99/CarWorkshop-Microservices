#region Usings

using System.Runtime.Serialization;

#endregion

#region Requests

namespace MSRequests.Models
{

    [DataContract]
    public class Requests
    {

        [DataMember]
        public string RequestId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string LicensePlate { get; set; }

        [DataMember]
        public int GarageId { get; set; }


        public Requests(string requestId, int userId, string licensePlate, int garageId)
        {

            RequestId = requestId;
            UserId = userId;
            LicensePlate = licensePlate;
            GarageId = garageId;
        }

    }

}

#endregion
