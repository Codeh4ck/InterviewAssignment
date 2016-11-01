using System;
using System.Diagnostics;
using System.Linq;
using DataParser.BaseComponents;
using DataParser.DataParsers;
using DataParser.Models;
using DataStorage.Database;
using DataStorage.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataStorage.Tests
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public async Task TestTwitterStorage()
        {
            IDataParser Parser = DataParserFactory.CreateParser("Twitter", "#freebandnames");
            Assert.IsNotNull(Parser);

            Assert.IsInstanceOfType(Parser, typeof(IDataParser), $"{nameof(Parser)} must inherit {nameof(IDataParser)}");
            Assert.IsInstanceOfType(Parser, typeof(DataParserBase), $"{nameof(Parser)} must inherit {nameof(DataParserBase)}");
            Assert.IsInstanceOfType(Parser, typeof(TwitterParser), $"{nameof(Parser)} must be a {nameof(DataParser.DataParsers.TwitterParser)}");

            Assert.IsNotNull(Parser.ProviderName, $"{nameof(Parser.ProviderName)} must have a value assigned (\"Twitter\").");
            Assert.AreEqual(Parser.ProviderName, "Twitter", $"{nameof(Parser.ProviderName)} must have a value (\"Twitter\").");

            Assert.IsNotNull(Parser.SearchTerm, $"{nameof(Parser.SearchTerm)} must have a value assigned (\"#freebandnames\").");
            Assert.AreEqual("#freebandnames", Parser.SearchTerm, $"{Parser.SearchTerm} must have a value (\"#freebandnames\").");

            TwitterParser TwitterParser = (TwitterParser)Parser;

            DataModel Model = TwitterParser.ParseResult(Resources.SampleTwitterJson);
            Assert.IsNotNull(Model, $"{nameof(Model)} must not be null. This indicates that {nameof(TwitterParser.ParseResult)} failed.");

            using (HttpClient Client = new HttpClient())
            {
                string JsonModel = new JavaScriptSerializer().Serialize(Model);
                Assert.IsNotNull(JsonModel, $"{JsonModel} must be a valid JSON string.");

                StringContent Content = new StringContent(JsonModel, Encoding.UTF8, "application/json");
                HttpResponseMessage ResponseMessage = await Client.PostAsync("http://localhost:46778/News", Content);

                Assert.IsTrue(ResponseMessage.IsSuccessStatusCode, $"{ResponseMessage.IsSuccessStatusCode} must be true. Otherwise it indicates a failed REST POST request.");
            }
        }

        [TestMethod]
        public async Task TestGetResults()
        {
            using (HttpClient Client = new HttpClient())
            {            
                HttpResponseMessage ResponseMessage = await Client.GetAsync("http://localhost:46778/News");
                Assert.IsTrue(ResponseMessage.IsSuccessStatusCode, $"{ResponseMessage.IsSuccessStatusCode} must be true. Otherwise it indicates a failed REST GET request.");

                string ResponseMessageString = await ResponseMessage.Content.ReadAsStringAsync();

                try
                {
                    JArray JsonArray = JArray.Parse(ResponseMessageString);
                    Assert.IsNotNull(JsonArray, $"{nameof(JsonArray)}");
                }
                catch (JsonException)
                {
                    Assert.Fail($"Invalid JSON array contained in {nameof(ResponseMessageString)}.");
                }                                
            }
       }

        [TestMethod]
        public async Task TestGetWithIdResults()
        {
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage ResponseMessage = await Client.GetAsync("http://localhost:46778/News/5");
                Assert.IsTrue(ResponseMessage.IsSuccessStatusCode, $"{ResponseMessage.IsSuccessStatusCode} must be true. Otherwise it indicates a failed REST GET request.");

                string ResponseMessageString = await ResponseMessage.Content.ReadAsStringAsync();

                try
                {
                    JArray JsonArray = JArray.Parse(ResponseMessageString);
                    Assert.IsNotNull(JsonArray, $"{nameof(JsonArray)}");
                    Assert.IsTrue(JsonArray.Count == 5, $"{nameof(JsonArray.Count)} must have a value of 5 since we requested 5 news posts.");
                }
                catch (JsonException)
                {
                    Assert.Fail($"Invalid JSON array contained in {nameof(ResponseMessageString)}.");
                }
            }
        }

        [TestMethod]
        public async Task TestGetWithIdAndProviderResults()
        {
            using (HttpClient Client = new HttpClient())
            {
                HttpResponseMessage ResponseMessage = await Client.GetAsync("http://localhost:46778/News/5?provider=Twitter");
                Assert.IsTrue(ResponseMessage.IsSuccessStatusCode, $"{ResponseMessage.IsSuccessStatusCode} must be true. Otherwise it indicates a failed REST GET request.");

                string ResponseMessageString = await ResponseMessage.Content.ReadAsStringAsync();

                //This is a little hack to see if the JSON array contained provider: Twitter 5 times (since we want 5 results).
                
                int OccurenceCount = new Regex(Regex.Escape("\"Provider\":\"Twitter\"")).Matches(ResponseMessageString.Trim()).Count;
                Assert.IsTrue(OccurenceCount == 5, $"{nameof(ResponseMessageString)} must contain 5 JSON objects with Provider: \"Twitter\". Current count: {OccurenceCount}.");

                try
                {
                    JArray JsonArray = JArray.Parse(ResponseMessageString);
                    Assert.IsNotNull(JsonArray, $"{nameof(JsonArray)}");
                    Assert.IsTrue(JsonArray.Count == 5, $"{nameof(JsonArray.Count)} must have a value of 5 since we requested 5 news posts.");                    
                }
                catch (JsonException)
                {
                    Assert.Fail($"Invalid JSON array contained in {nameof(ResponseMessageString)}.");
                }
            }
        }

        [TestMethod]
        public void TestDatabaseContext()
        {
            DatabaseContext Context = DatabaseContext.Create();

            Assert.IsNotNull(Context);
            Assert.IsNotNull(Context.NewsData);
            Assert.IsNotNull(Context.Database.Connection.ConnectionString);
        }
    }
}
