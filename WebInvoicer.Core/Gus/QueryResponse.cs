using System.Xml.Serialization;

namespace WebInvoicer.Core.Gus
{
    [XmlRoot("dane")]
    public class QueryResponse
    {
        [XmlElement("Nazwa")]
        public string Name { get; set; }

        [XmlElement("Ulica")]
        public string Street { get; set; }

        [XmlElement("NrNieruchomosci")]
        public string PropertyNumber { get; set; }

        [XmlElement("NrLokalu")]
        public string ApartmentNumber { get; set; }

        [XmlElement("KodPocztowy")]
        public string PostalCode { get; set; }

        [XmlElement("Miejscowosc")]
        public string City { get; set; }
    }
}
