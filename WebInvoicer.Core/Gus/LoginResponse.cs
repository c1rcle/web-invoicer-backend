using System.Xml.Serialization;

namespace WebInvoicer.Core.Gus
{
    [XmlRoot("ZalogujResponse", Namespace = "http://CIS/BIR/PUBL/2014/07")]
    public class LoginResponse
    {
        [XmlElement("ZalogujResult")]
        public string SessionId { get; set; }
    }
}
