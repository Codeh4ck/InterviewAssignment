using System;
using System.Threading.Tasks;

namespace DataConnector.BaseComponents
{
    public interface IDataConnector
    {
         Task<int> RetrieveData(params object[] parameters);
         Task<bool> SendToParser(string jsonString, string searchTerm);
    }
}
