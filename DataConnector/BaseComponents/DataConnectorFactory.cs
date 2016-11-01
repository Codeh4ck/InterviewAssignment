using System;
using DataConnector.Connectors;

namespace DataConnector.BaseComponents
{
    class DataConnectorFactory
    {
        public static DataConnector CreateDataConnector(string providerName)
        {
            /* We can simulate MVC's Controller loading system here using reflection.
             * For instance, we could require connectors to be in the DataConnector.Connectors namespace and
             * each time a connector is requested, we can load the {providerName}Provider class.
             * 
             * e.g: providerName = "Twitter" ->  load TwitterConnector class           
             */

            switch (providerName.ToLower())
            {
                case "twitter":
                    return new TwitterConnector();
                default:
                    throw new NotImplementedException("Data connector has not been implemented yet.");
            }
        }
    }
}
