using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DataParser.BaseComponents;
using DataParser.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DataParser.Controllers
{
    public class DataController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok("Nothing to see here.");
        }

        public async Task<IHttpActionResult> Post([FromUri]string provider, [FromUri]string searchTerm, HttpRequestMessage json)
        {
            if (string.IsNullOrEmpty(provider))
                return BadRequest("Provider must be set.");

            if (string.IsNullOrEmpty(searchTerm))
                return BadRequest("Search term must be set.");

            string JSON = await json.Content.ReadAsStringAsync();
            
            if (string.IsNullOrEmpty(JSON))
                return BadRequest("JSON data must be set.");

            try
            {
                JsonSchema Schema = JsonSchema.Parse(JSON);
                JObject JsonObject = JObject.Parse(JSON);

                if (!JsonObject.IsValid(Schema))
                    return BadRequest("JSON provided is invalid.");
            }
            catch (JsonReaderException )
            {
                return BadRequest("JSON provided is invalid.");
            }

            IDataParser Parser = DataParserFactory.CreateParser(provider, searchTerm);
            DataModel Model = Parser.ParseResult(JSON);

            if (Model != null)
            {
                if (await Parser.StoreData(Model))
                {
                    return Ok("Data sent to storage successfully.");
                }
            }


            return BadRequest("Data has not been stored.");
        }
    }
}
