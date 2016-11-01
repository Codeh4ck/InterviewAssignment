using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataConnector.BaseComponents
{    
    public abstract class DataConnector : IDisposable, IDataConnector
    {
        public string ProviderName { get; protected set; }
        protected virtual string APIUrl { get; set; } = "http://localhost:60918/Data";

        protected DataConnector(string providerName)
        {
            ProviderName = providerName;
        }

        public abstract Task<int> RetrieveData(params object[] parameters);
       
        public virtual async Task<bool> SendToParser(string jsonString, string searchTerm)
        {
            using (HttpClient _Client = new HttpClient())
            {
                StringContent Content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                HttpResponseMessage Result = await _Client.PostAsync($"{APIUrl}?provider={ProviderName}&searchTerm={searchTerm}", Content);
                return Result.IsSuccessStatusCode;
            }
        }
            
        public abstract void Dispose();
    }
}
