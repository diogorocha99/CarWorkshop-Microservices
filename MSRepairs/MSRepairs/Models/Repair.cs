#region Usings

using System.Runtime.Serialization;

#endregion

namespace MSRepairs.Models
{
    [DataContract]
    public class Repair
    {
        [DataMember]
        public string RepairId { get; set; }

        [DataMember]
        public string RequestId { get; set;}

        [DataMember]
        public string ManagerUserId { get; set;}

        [DataMember]
        public int GarageId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string ServiceTypeId { get; set; }

        [DataMember]
        public int WorkTime { get; set; }

        [DataMember]
        public string Notes { get; set; }

        public Repair(string repairId, string requestId, string managerUserId, int garageId, int userId, string serviceTypeId, int workTime, string notes)
        {
            RepairId = repairId;
            RequestId = requestId;
            ManagerUserId = managerUserId;
            GarageId = garageId;  
            UserId = userId;
            ServiceTypeId = serviceTypeId;
            WorkTime = workTime;
            Notes = notes;
        }
    }
}
