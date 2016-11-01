using System.Threading.Tasks;
using DataConnector.BaseComponents;
using DataConnector.Connectors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataConnector.Tests
{
    [TestClass]
    public class ConnectorTest
    {
        [TestMethod]
        public async Task TestConnector()
        {
            IDataConnector Connector = DataConnectorFactory.CreateDataConnector("twitter");
            Assert.IsNotNull(Connector);

            Assert.IsInstanceOfType(Connector, typeof(IDataConnector), $"{nameof(Connector)} must inherit {nameof(IDataConnector)}");
            Assert.IsInstanceOfType(Connector, typeof(BaseComponents.DataConnector), $"{nameof(Connector)} must inherit {nameof(BaseComponents.DataConnector)}");
            Assert.IsInstanceOfType(Connector, typeof(TwitterConnector), $"{nameof(Connector)} must be a {nameof(Connectors.TwitterConnector)}");

            TwitterConnector TwitterConnector = (TwitterConnector) Connector;

            Assert.IsNotNull(TwitterConnector.ProviderName, $"{nameof(TwitterConnector)} must have a value (\"Twitter\") assigned.");
            Assert.AreEqual("Twitter", TwitterConnector.ProviderName, $"{nameof(TwitterConnector.ProviderName)} is not the expected value (\"Twitter\").");

            int DataCount = await TwitterConnector.RetrieveData("#felixbaumgartner");
            Assert.IsTrue(DataCount > 0, $"{nameof(DataCount)} must have a value greater than 0. If the value is 0, it means no search results where returned.");           
        }
    }
}
