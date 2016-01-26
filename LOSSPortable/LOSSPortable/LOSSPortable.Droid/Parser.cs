using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Parse;

namespace LOSSPortable.Droid
{
    public class Parser : IParseStorage
    {
        static Parser serviceInstance = new Parser();

        public static Parser Default { get { return serviceInstance; } }

        /*
         * Add List of items you added here
         */
        public List<SampleItem> SampleItems { get; private set; }
        public Quote quoteOfDay { get; private set; }

        protected Parser()
        {
            /*
             * Add initialization of list created above, here.
             */
            SampleItems = new List<SampleItem>();
            quoteOfDay = new Quote();

            ParseClient.Initialize(Constants.ApplicationID,Constants.Key);
        }

        ParseObject ToParseObject(SampleItem item)
        {
            var parseobject = new ParseObject("SampleItem");
            if (item.ID != string.Empty)
            {
                parseobject.ObjectId = item.ID;
            }
            parseobject["sample1"] = item.Sample1;
            parseobject["sample2"] = item.Sample2;
            //parseobject.ACL = new ParseACL(ParseUser.CurrentUser);

            return parseobject;
        }

        static SampleItem FromParseObject(ParseObject po)
        {
            var tmp = new SampleItem();
            tmp.ID = po.ObjectId;
            tmp.Sample1 = Convert.ToString(po["sample1"]);
            tmp.Sample2 = Convert.ToString(po["sample2"]);
            //tmp.Sample3 = Convert.ToBoolean(po["checkmark"]);
            return tmp;
        }

        public async Task<List<SampleItem>> RefreshDataAsync()
        {
            var query = ParseObject.GetQuery("SampleItem");
            var result = await query.FindAsync();

            var Items = new List<SampleItem>();
            foreach (var item in result)
            {
                //if (item.ACL != null){
                Items.Add(FromParseObject(item));
                //}
            }
            return Items;
        }

        public async Task SaveSampleItemAsync(SampleItem item)
        {
            try
            {
                await ToParseObject(item).SaveAsync();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(@"Error {0}", e.Message);
            }

        }

        ParseObject ToParseObject(Quote quote)
        {
            var parseobject = new ParseObject("Quote");
            if (quote.ID != string.Empty)
            {
                parseobject.ObjectId = quote.ID;
            }
            parseobject["message"] = quote.inspirationalQuote;
            parseobject["image"] = quote.picture;
            //parseobject.ACL = new ParseACL(ParseUser.CurrentUser);

            return parseobject;
        }

        static Quote FromParseObject(ParseObject po)
        {
            var tmp = new Quote();
            tmp.ID = po.ObjectId;
            tmp.Sample1 = Convert.ToString(po["message"]);
            tmp.Sample2 = Convert.ToString(po["image"]);
            //tmp.Sample3 = Convert.ToBoolean(po["checkmark"]);
            return tmp;
        }

        public async Task<Quote> RefreshQuoteAsync()
        {
            var query = ParseObject.GetQuery("Quote");
            var result = await query.FindAsync();

            var 
            //var Items = new List<SampleItem>();
            //foreach (var item in result)
            //{
            //    //if (item.ACL != null){
            //    Items.Add(FromParseObject(item));
            //    //}
            //}
            return Items;
        }

    }
}