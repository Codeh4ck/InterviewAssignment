using System.Threading.Tasks;
using DataParser.Models;

namespace DataParser.BaseComponents
{
    public interface IDataParser
    {
        string ProviderName { get; set; }
        string SearchTerm { get; set; }
        DataModel ParseResult(string json);
        Task<bool> StoreData(DataModel model);
    }
}
