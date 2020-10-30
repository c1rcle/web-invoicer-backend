using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using WebInvoicer.Core.Gus;
using WebInvoicer.Core.Utility;

namespace WebInvoicer.Core.Services
{
    public class GusService : CancellableOnRequestAbort, IGusService
    {
        private readonly IHttpClientFactory clientFactory;

        private readonly GusConfiguration config;

        private enum RequestType
        {
            Login,
            Query,
            Logout
        }

        public GusService(IHttpClientFactory clientFactory, GusConfiguration config,
            IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            this.clientFactory = clientFactory;
            this.config = config;
        }

        public async Task<ResultHandler> GetCounterpartyDetails(string nip)
        {
            try
            {
                var session = await OpenSession();
                var details = await GetCounterpartyDetails(nip, session.SessionId);
                await CloseSession(session.SessionId);

                return details.Name == null 
                    ? new ResultHandler(HttpStatusCode.NotFound) 
                    : new ResultHandler(HttpStatusCode.OK, details);
            }
            catch (Exception)
            {
                return new ResultHandler(HttpStatusCode.ServiceUnavailable);
            }
        }

        private async Task<LoginResponse> OpenSession()
        {
            var content = File.ReadAllText("RequestTemplates/Login.xml")
                .Replace("{KEY}", config.Key);

            return await HandleSoapRequest<LoginResponse>(content, RequestType.Login);
        }

        private async Task<QueryResponse> GetCounterpartyDetails(string nip, string sessionId)
        {
            var content = File.ReadAllText("RequestTemplates/GetCounterpartyDetails.xml")
                .Replace("{NIP}", nip);

            return await HandleSoapRequest<QueryResponse>(content, RequestType.Query,
                new Dictionary<string, string> { { "sid", sessionId } });
        }

        private async Task<LogoutResponse> CloseSession(string sessionId)
        {
            var content = File.ReadAllText("RequestTemplates/Logout.xml")
                .Replace("{SID}", sessionId);

            return await HandleSoapRequest<LogoutResponse>(content, RequestType.Logout);
        }

        private async Task<T> HandleSoapRequest<T>(string content, RequestType type,
            Dictionary<string, string> headers = null)
        {
            var client = clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, config.Url);
            request.Content = new StringContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/soap+xml");

            headers?.ToList()
                .ForEach(header => request.Content.Headers.Add(header.Key, header.Value));

            var response = await client.SendAsync(request, GetCancellationToken());
            var xmlData = ParseMtomString(await response.Content.ReadAsStringAsync(), type);
            var serializer = new XmlSerializer(typeof(T));

            if (type == RequestType.Query)
            {
                xmlData = XElement.Parse(HttpUtility.HtmlDecode(xmlData.FirstNode.ToString()));
            }

            return (T)serializer.Deserialize(type == RequestType.Query
                ? xmlData.FirstNode.CreateReader()
                : xmlData.CreateReader());
        }

        private XElement ParseMtomString(string mtomResponse, RequestType type)
        {
            var closingTag = "</s:Envelope>";
            var xmlString = mtomResponse.Substring(mtomResponse.IndexOf("<s:Envelope"));
            xmlString = xmlString.Substring(0, xmlString.IndexOf(closingTag) + closingTag.Length);


            var xmlDocument = XElement.Parse(xmlString);
            var nameSpace = (XNamespace)"http://CIS/BIR/PUBL/2014/07";
            var nodeName = type switch
            {
                RequestType.Login => "ZalogujResponse",
                RequestType.Query => "DaneSzukajPodmiotyResult",
                _ => "WylogujResponse"
            };

            return xmlDocument.Descendants(nameSpace + nodeName).First();
        }
    }
}
