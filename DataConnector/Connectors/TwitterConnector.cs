using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using DataConnector.HTTP;

namespace DataConnector.Connectors
{
    internal sealed class TwitterConnector : BaseComponents.DataConnector
    {
        private string ConsumerKey { get; }
        private string ConsumerSecret { get; }
        private string BearerToken { get; set; }
        private RestClient Client { get; }

        public TwitterConnector() : base("Twitter")
        {                        
            ConsumerKey = "y4LpXhKWtEkfLTnaC8bpNErTf";
            ConsumerSecret = "wAOABi64RDrD6eKNWhPIaqTStlZfSe9P5BMGWyrvsq305UYezG";

            Client = new RestClient();
        }

        public override void Dispose()
        {
            Client.Dispose();
        }

        public override async Task<int> RetrieveData(params object[] parameters)
        {
            if (parameters.Length != 1)
                throw new TargetParameterCountException($"{nameof(TwitterConnector)}:{nameof(RetrieveData)}(): Expected parameter count: 1. Expected parameter type: string.");

            BearerToken = await ObtainBearerToken();

            string Query = parameters[0].ToString();
            string EncodedQuery = HttpUtility.UrlEncode(Query);

            HttpResponseMessage ResponseMessage = await Client.GetBodyAsync(
                $"https://api.twitter.com/1.1/search/tweets.json?q={EncodedQuery}&result_type=recent",
                "",
                new[]
                {
                    new RequestHeader {HeaderKey = "Authorization", HeaderValue = $"Bearer {BearerToken}"}
                },
                new[]
                {
                    new MediaTypeWithQualityHeaderValue("application/json") {CharSet = Encoding.UTF8.WebName},
                }
            );

            JavaScriptSerializer Serialzier = new JavaScriptSerializer();
            dynamic DeserializedObjects =
                Serialzier.Deserialize<dynamic>(await ResponseMessage.Content.ReadAsStringAsync())["statuses"];

            int DataLength = DeserializedObjects.Length;

            // I am casting the start index to a long here because Visual Studio is complaining that it can't tell whether I want the 
            // Parallel.For(long, long, ...) overload or the Parallel.For(int, int, ...)
            // Apparently redundantly casting to (int) does not help.

            Parallel.For((long)0, DataLength, async x =>
            {
                dynamic SubObject = DeserializedObjects[x];
                await SendToParser(Serialzier.Serialize(SubObject), EncodedQuery);
            });

            return DataLength;
        }

        private async Task<string> ObtainBearerToken()
        {

            if (!string.IsNullOrEmpty(BearerToken))
                return BearerToken;

            const string OATH_TOKEN_URL = "https://api.twitter.com/oauth2/token";

            byte[] TokenCredentialsBytes = new UTF8Encoding().GetBytes($"{ConsumerKey}:{ConsumerSecret}");
            string TokenCredentials = Convert.ToBase64String(TokenCredentialsBytes);

            HttpResponseMessage ResponseMessage = await Client.PostBodyAsync(OATH_TOKEN_URL,
                "grant_type=client_credentials",
                new[]
                {
                    new RequestHeader {HeaderKey = "Authorization", HeaderValue = $"Basic {TokenCredentials}"}
                },
                new[]
                {
                    new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded")
                    {
                        CharSet = Encoding.UTF8.WebName
                    }
                });

            string JsonResponse = await ResponseMessage.Content.ReadAsStringAsync();

            JavaScriptSerializer Serializer = new JavaScriptSerializer();

            dynamic Token = Serializer.Deserialize<dynamic>(JsonResponse);
            dynamic TokenAccess = Token["access_token"];

            return Convert.ToString(TokenAccess);

        }
    }
}
