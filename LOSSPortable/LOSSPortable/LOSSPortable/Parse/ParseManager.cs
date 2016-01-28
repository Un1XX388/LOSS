using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOSSPortable
{

    /*
     * For each method you created in the interface,
     * create a method that calls the method prototype 
     * from the interface.
     */
    public class ParseManager
    {
        IParseStorage storage;

        public ParseManager(IParseStorage parseStorage)
        {
            storage = parseStorage;
        }

        public Task<List<SampleItem>> GetTaskAsync()
        {
            return storage.RefreshDataAsync();
        }

        public Task SaveTaskAsync(SampleItem item)
        {
            return storage.SaveSampleItemAsync(item);
        }

        public Task<List<Quote>> GetTaskQuoteAsync()
        {
            return storage.RefreshQuoteAsync();
        }
    }
}
