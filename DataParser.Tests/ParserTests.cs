using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataParser.BaseComponents;
using DataParser.DataParsers;
using DataParser.Models;
using DataParser.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataParser.Tests
{
    [TestClass]
    public class ParserTests
    {
        
        [TestMethod]
        public void TestTwitterParser()
        {
            IDataParser Parser = DataParserFactory.CreateParser("Twitter", "#freebandnames");
            Assert.IsNotNull(Parser);

            Assert.IsInstanceOfType(Parser, typeof(IDataParser), $"{nameof(Parser)} must inherit {nameof(IDataParser)}");
            Assert.IsInstanceOfType(Parser, typeof(DataParserBase), $"{nameof(Parser)} must inherit {nameof(DataParserBase)}");
            Assert.IsInstanceOfType(Parser, typeof(TwitterParser), $"{nameof(Parser)} must be a {nameof(DataParsers.TwitterParser)}");

            Assert.IsNotNull(Parser.ProviderName, $"{nameof(Parser.ProviderName)} must have a value assigned (\"Twitter\").");
            Assert.AreEqual(Parser.ProviderName, "Twitter", $"{nameof(Parser.ProviderName)} must have a value (\"Twitter\").");

            Assert.IsNotNull(Parser.SearchTerm, $"{nameof(Parser.SearchTerm)} must have a value assigned (\"#freebandnames\").");
            Assert.AreEqual("#freebandnames", Parser.SearchTerm, $"{Parser.SearchTerm} must have a value (\"#freebandnames\").");

            TwitterParser TwitterParser = (TwitterParser) Parser;

            DataModel Model = TwitterParser.ParseResult(Resources.SampleTwitterJson);
            Assert.IsNotNull(Model, $"{nameof(Model)} must not be null. This indicates that {nameof(TwitterParser.ParseResult)} failed.");

            Assert.IsNotNull(Model.SearchTerm, $"{nameof(Model.SearchTerm)} must have a value assigned (\"#freebandnames\").");
            Assert.AreEqual(Model.SearchTerm, TwitterParser.SearchTerm, $"{nameof(Model.SearchTerm)} and {nameof(TwitterParser.SearchTerm)} must have equal values.");

            Assert.IsNotNull(Model.Message, $"{nameof(Model.Message)} must have a value (\"Aggressive Ponytail #freebandnames\").");
            Assert.AreEqual(Model.Message, "Aggressive Ponytail #freebandnames");

            Assert.IsNotNull(Model.Date, $"{nameof(Model.Date)} must have a value (Mon Sep 24 03:35:21 +0000 2012).");
            Assert.AreEqual(Model.Date,
                DateTime.ParseExact("Mon Sep 24 03:35:21 +0000 2012", "ddd MMM dd HH:mm:ss +ffff yyyy",
                    new CultureInfo("en-US")), $"{nameof(Model.Date)} must match the value (Mon Sep 24 03:35:21 +0000 2012).");

            Assert.AreEqual(Model.Provider, TwitterParser.ProviderName, $"{nameof(Model.Provider)} and {nameof(TwitterParser.ProviderName)} must have equal values.");

            Assert.IsNotNull(Model.Poster, $"{nameof(Model.Poster)} must have a value assigned (\"@sean_cummings\")");
            Assert.AreEqual("@sean_cummings", Model.Poster, $"{nameof(Model.Poster)} must have (\"@sean_cummings\") as its value.");            
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException), "A HttpRequestException was thrown. Host is possibly offline.")]
        public async Task TestInvalidParserREST()
        {
            using (HttpClient Client = new HttpClient())
            {
                using (StringContent Content = new StringContent(Resources.SampleTwitterJson, Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage Message = await Client.PostAsync("http://localhost:60918/Data", Content);

                    Assert.IsNotNull(Message, $"{nameof(HttpResponseMessage)} failed to be received. Possibly a bad API URL.");

                    // This should pass. Our URL is wrong and expects 2 parameters (Provider and SearchTerm)
                    Assert.IsFalse(Message.IsSuccessStatusCode, $"{nameof(Message.IsSuccessStatusCode)} must be false as the API URL was invalid.");                    
                }
            }
        }

        [TestMethod]
        public async Task TestValidParserREST()
        {
            using (HttpClient Client = new HttpClient())
            {
                using (StringContent Content = new StringContent(Resources.SampleTwitterJson, Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage Message = await Client.PostAsync("http://localhost:60918/Data?provider=Twitter&searchTerm=%23freebandnames", Content);

                    Assert.IsNotNull(Message, $"{nameof(HttpResponseMessage)} failed to be received. Possibly a bad API URL.");
                    Assert.IsTrue(Message.IsSuccessStatusCode, $"{nameof(Message.IsSuccessStatusCode)} must be true to indicate a successful POST in the REST API.");
                }
            }
        }
    }
}
