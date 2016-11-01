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
[WIP]
