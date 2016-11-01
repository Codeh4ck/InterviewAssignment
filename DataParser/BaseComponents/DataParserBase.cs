using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataParser.Models;
using Newtonsoft.Json;

namespace DataParser.BaseComponents
{
    public abstract class DataParserBase : IDataParser
    {
        public string ProviderName { get; set; }
        public string SearchTerm { get; set; }
        public abstract DataModel ParseResult(string json);

        protected DataParserBase(string providerName, string searchTerm)
        {
            ProviderName = providerName;
            SearchTerm = searchTerm;
        }

        public async Task<bool> StoreData(DataModel model)
        {
            if (model == null)
                return false;

            string StorageURL = ConfigurationManager.AppSettings["StorageUrl"];

            using (HttpClient Client = new HttpClient())
            {
                string JsonData = JsonConvert.SerializeObject(model);
                StringContent Content = new StringContent(JsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage ResponseMessage = await Client.PostAsync(StorageURL, Content);
                return ResponseMessage.IsSuccessStatusCode;
            }
        }
    }    
}