using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WCFService
{
    [DataContract]
    public class MyData
    {
        [DataMember]
        public int Id;
        [DataMember]
        public string Desc;
        [DataMember]
        public DateTime? StartDate;
    }
}