using DataParser.BaseComponents;
using DataParser.DataParsers;
using DataParser.Models;
using DataStorage.Database;
using DataStorage.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
        public void TestDatabaseContext()
        {
            DatabaseContext Context = DatabaseContext.Create();

            Assert.IsNotNull(Context);
            Assert.IsNotNull(Context.NewsData);
            Assert.IsNotNull(Context.Database.Connection.ConnectionString);
        }
    }
}
