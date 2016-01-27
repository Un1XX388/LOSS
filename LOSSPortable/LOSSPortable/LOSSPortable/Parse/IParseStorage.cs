using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOSSPortable
{

    /*
     * Add item specific methods protypes here.
     * Examples:
     * Task<bool> IsUserLoggedIn();
     * Task DeleteTodoitemAsync(Sample item);
     */
    public interface IParseStorage
    {
        Task<List<SampleItem>> RefreshDataAsync();
        Task<Quote> RefreshQuoteAsync();
        Task SaveSampleItemAsync(SampleItem item);
    }
}
