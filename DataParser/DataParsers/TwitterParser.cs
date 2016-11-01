using System;
using System.Globalization;
using DataParser.BaseComponents;
using DataParser.Models;
using Newtonsoft.Json;

namespace DataParser.DataParsers
{
    public class TwitterParser : DataParserBase
    {
        public TwitterParser(string searchTerm) : base("Twitter", searchTerm)
        {
        }

        public override DataModel ParseResult(string json)
        {
            try
            {
                // I deserialize the json data here to a dynamic object since I do not want
                // to hardcore a Twitter JSON object. The values we need to take are known,
                // so it should cause no problems. The only issue would be twitter changing
                // their API and the returned JSON data object structure.

                dynamic TwitterDataObject = JsonConvert.DeserializeObject<dynamic>(json);
                dynamic UserData = TwitterDataObject["user"];

                string ScreenName = UserData["screen_name"];
                string DatePlainText = TwitterDataObject["created_at"];
                string Text = TwitterDataObject["text"];

                string TwitterUsername = $"@{ScreenName}";
                
                DateTime Date = DateTime.ParseExact(DatePlainText, "ddd MMM dd HH:mm:ss +ffff yyyy", new CultureInfo("en-US"));

                return new DataModel
                {
                    Date = Date,
                    Message = Text,
                    Poster = TwitterUsername,
                    Provider = ProviderName,
                    SearchTerm = SearchTerm
                };
            }
            catch
            {
                return null;
            }
        }
    }
}