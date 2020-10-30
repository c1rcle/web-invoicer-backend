using System.Xml.Serialization;

namespace WebInvoicer.Core.Gus
{
    [XmlRoot("WylogujResponse", Namespace = "http://CIS/BIR/PUBL/2014/07")]
    public class LogoutResponse
    {
        [XmlElement("WylogujResult")]
        public bool SessionId { get; set; }
    }
}
