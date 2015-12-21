using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Parse;

namespace LOSS.Android
{
    public class ParseClass : ParseClassInterface
    {
        static ParseClass lossServiceInstance = new ParseClass();

        public static ParseClass Default { get { return lossServiceInstance; } }

        public List<ResourceItem> Items { get; private set; }

        protected ParseClass()
        {
            Items = new List<ResourceItem>();
            ParseClient.Initialize(Constants.ApplicationId, Constants.Key);
        }

        ParseObject ToParseObject(ResourceItem todo)
        {
            var po = new ParseObject("Resource");
            if (todo.objectID != string.Empty)
            {
                po.ObjectId = todo.objectID;
            }
            po["description"] = todo.description;
            po["type"] = todo.type;
            po["url"] = todo.url;

            return po;
        }

        static ResourceItem FromParseObject(ParseObject po)
        {
            var t = new ResourceItem();
            t.objectID = po.ObjectId;
            t.description = Convert.ToString(po["description"]);
            t.type = Convert.ToString(po["type"]);
            t.url = Convert.ToString(po["url"]);
            return t;
        }

        public async Task<List<ResourceItem>> RefreshDataAsync()
        {
            var query = ParseObject.GetQuery("Resource");
            var results = await query.FindAsync();

            var Items = new List<ResourceItem>();
            foreach (var item in results)
            {
                Items.Add(FromParseObject(item));
            }
            return Items;
        }

        public async Task SaveTodoItemAsync(ResourceItem todoItem)
        {
            try
            {
                await ToParseObject(todoItem).SaveAsync();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"				ERROR {0}", e.Message);
            }
        }

        public async Task DeleteTodoItemAsync(ResourceItem item)
        {
            try
            {
                await ToParseObject(item).DeleteAsync();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"				ERROR {0}", e.Message);
            }
        }
    }
}