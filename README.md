# InterviewAssignment
A project provided as a solution to an interview assignment. 

# Description & Overview
The project is layered in 3 modules, the **connector**, the **parser** and the **storage handler**. Each module is responsible for fullfilling the following tasks:

* The **connector** extracts data from the data source.
* The **parser** receives the data from the connector and formats it to a common data structure.
* The **storage handler** receives data from the parser and stores it into the database. Additionally, it provides us with the stored data.

All modules communicate between them using JSON objects. The **parser** and the **storage handler** are JSON services, written using ASP .NET WebAPI 2, Newtonsoft JSON and Entity Framework. 

# Description of the Connector
The connector is a console application that receives input from the user (either inside the console, or through the command line arguments). It then queries the data source and retrieves the data. In this case, we are querying Twitter to receive the latest tweets, using a given hashtag as a search term. The connector uses Twitter's REST API to get the latest tweets and returns a maximum of 15 results. After retrieving the data, it sends each JSON object representing a tweet to the parser. All functionallity is done asynchronously.

# Description of the Parser
The parser is a RESTful JSON web service (ASP .NET WebAPI 2). It receives data from the connector and formats it into a common data structure. In this project, the common data structure, which is shared between the parser and the storage handler, is model with the following properties:

* The name of the queried service
* The search term (e.g a hashtag)
* The name of the poster
* The post message (e.g a tweet message)
* The date that the message was posted

The parser retrieves the data given by the connector, formats it into a JSON representation of the aforementioned model and sends it to the storage handler for storing. 

# Description of the Storage
The storage is a RESTful JSON web service (ASP .NET WebAPI 2) as well. receives the formatted objects from the parser in JSON format. It validates if the structure of the model is the one we're expecting and stores it into the database. Additionally, the storage provides us with the ability to retrieve data from the database in 3 ways:

* Retrieve all the data at once
* Retrieve an x amount of items
* Retrieve an x amount of items, of the y data source

# Extending the application to include more data sources

To extend the application, the following steps must be followed:

### 1. Extending the **connector**:
You must first create a class in the **DataConnector.Connectors** namespace which must inherit the [DataConnector](DataConnector/BaseComponents/DataConnector.cs) class. In your new class constructor, call the DataConnector's constructor and pass the name of your provider as an argument. Then implement these 2 abstract methods of the [DataConnector](DataConnector/BaseComponents/DataConnector.cs):

* public abstract Task<int> RetrieveData(params object[] parameters);
* public abstract void Dispose();

**For instance:**

```csharp
using System;
using System.Text;
using System.Threading.Tasks;
using DataConnector.HTTP;

namespace DataConnector.Connectors
{
    public sealed class FacebookConnector : BaseComponents.DataConnector
    {
        public FacebookConnector() : base("Facebook")
        {                        

        }

        public override void Dispose()
        {
        }

        public override async Task<int> RetrieveData(params object[] parameters)
        {
           
        }
    }
}
```

The **RetrieveData()** should be used to write the logic of how to retrieve data from the data source. Any number of parameters can be used. In the current project, we only use 1 string parameter which is the Twitter hashtag to use as a search term. After retrieving the data, format it into a JSON object of your choice and send it to the parser, through the **SendToParser()** method, which takes 2 arguments, the JSON object and the search term. Make sure to await the SendToParser() method, as it is an asynchronous method. **RetrieveData()** returns an integer that represents the amount of data processed.

The **Dispose()** method should be used to dispose of any resources used in the connector class.

Your connector class can include any property of your choise and any method. The only requirement is to override the above 2 methods.

After adding your new connector, edit the [DataConnectorFactory](DataConnector/BaseComponents/DataConnectorFactory.cs) class, and add a new case in the **CreateDataConnector()** switch statement. Your new statement, taking into account the previous example class, should look like this:

```csharp
using System;
using DataConnector.Connectors;

namespace DataConnector.BaseComponents
{
    public class DataConnectorFactory
    {
        public static DataConnector CreateDataConnector(string providerName)
        {
            switch (providerName.ToLower())
            {
                case "twitter":
                    return new TwitterConnector();
				case "facebook":
					return new FacebookConnector();
                default:
                    throw new NotImplementedException("Data connector has not been implemented yet.");
            }
        }
    }
}
```

### 2. Extending the **parser**:

In order to support parsing for your new connector, you must also add the new parsing logic to the **parser**. First, create a new class in the **DataParser.DataParsers** namespace and inherit the (DataParserBase)[DataParser/BaseComponents/DataParserBase.cs] class. Your new parser class must call the base constructor as well, and pass 2 arguments: the **parser name** and the **search term** used to get the data we're going to process. Then, implement the following abstract method:

* public abstract DataModel ParseResult(string json);

**For instance:**

```csharp
using System;
using System.Globalization;
using DataParser.BaseComponents;
using DataParser.Models;

namespace DataParser.DataParsers
{
    public class FacebookParser : DataParserBase
    {
        public FacebookParser(string searchTerm) : base("Facebook", searchTerm)
        {
        }

        public override DataModel ParseResult(string json)
        {
            
        }
    }
}
```

In your new class, the **ParseResult()** method should implement the logic of transforming the json object (the parameter of **ParseResult**) into a **DataModel** object. Then, you must return the **DataModel** object, so the parser can send it to the storage. Your **DataModel** object should have all its properties filled, to ensure integrity and that full information is stored into the database later. 

After you implement the **ParseResult()** method, edit the [DataParserFactory](DataParser/BaseComponents/DataParserFactory.cs) class and add your parser to the switch statement, inside the **CreateParser()** method. Your modified **CreateParser()** method should now look like this:

```csharp
using System;
using DataParser.DataParsers;

namespace DataParser.BaseComponents
{
    public class DataParserFactory
    {
        public static IDataParser CreateParser(string providerName, string searchTerm)
        {
            switch (providerName.ToLower())
            {
                case "twitter":
                    return new TwitterParser(searchTerm);
				case "facebook":
                    return new FacebookParser(searchTerm);					
                default:
                    throw new IndexOutOfRangeException("The parser for the provider given is not yet implemented.");
            }
        }
    }
}
```

### A few guidelines on extending the application:

* Ensure that your connector's provider name and the parser's name are the same.
* The **SendToParser()** method is virtual and can be overriden with different logic for each connector.
* If your new data source does not provide a JSON service, format the data into a JSON object in the **RetrieveData()** method

# Hosting each module in different servers
The **Parser** and the **Storage** handler can be hosted in different servers. All modules communicate with each other through RESTful JSON APIs. The only settings that must be modified to reflect these changes are each service's URL. 

# Retrieving data from the storage handler
You can retrieve data from the storage handler in the following ways:

1. REST GET http://urltostorage/News (retrieves all entries)
2. REST GET http://urltostorage/News/10 (retrieves 10 most recent entries)
3. REST GET http://urltostorage/News/10/?provider=Twitter (retrieves 10 most recent entries of Twitter provider)

# Run the application on your own development environment
If you forked this project and wish to run it on your own environment, make sure that each module points to the right **Parser** and **Storage** URLs.

1. Open your **Parser** properties (Project -> DataParser Properties) and copy the Project URL. Then set the URL in the [DataConnector](DataConnector/BaseComponents/DataConnector.cs) class, in the **APIUrl** property (it can be overriden as well in each connector).

2. Open your **Storage** properties (Project -> DataStorage Properties) and copy the Project URL. Then set the URL in the [Web.config](DataParser/Web.config) file of the Parser, in the <appSettings> XML node (in the **StorageUrl** element).
