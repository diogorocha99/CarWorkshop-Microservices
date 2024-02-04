#region Usings

using System.Runtime.Serialization;

#endregion

#region Requests

namespace MSRepairs.Models
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

        public Requests(string requestId, int userId, string licensePlate)
        {

            RequestId = requestId;
            UserId = userId;
            LicensePlate = licensePlate;

        }

    }

}

#endregion
