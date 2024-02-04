#region Usings

using System.Runtime.Serialization;

#endregion

namespace MSRepairs.Models
{
    [DataContract]
    public class Parts
    {
        [DataMember]
        public int PartsId { get; set; }

        [DataMember]
        public string PartName { get; set; }


    }
}
