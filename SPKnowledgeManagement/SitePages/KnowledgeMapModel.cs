using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SPKnowledgeManagement.SitePages
{

    [DataContractAttribute]
    class KnowledgeMapModel
    {
        //成员需要标记为 DataMember   
        [DataMember]
        public string name { set; get; }
        [DataMember]
        public string id { set; get; }
        [DataMember]
        public string value { set; get; }
        [DataMember]
        public List<KnowledgeMapModel> children { get; set; }

        [DataMember]
        public string topic { get; set; }
        [DataMember]
        public string direction { get; set; }
    }
}
