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
        
        public List<Quote> InspirationalQuotes { get; private set; }

        protected Parser()
        {
            /*
             * Add initialization of list created above, here.
             */
            SampleItems = new List<SampleItem>();
            InspirationalQuotes = new List<Quote>();

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

        static Quote FromParseObj(ParseObject po)
        {
            var tmp = new Quote();
            tmp.ID = po.ObjectId;
            tmp.inspirationalQuote = Convert.ToString(po["Message"]);
            //tmp.picture = Convert.ToString(po["image"]);
            //tmp.Sample3 = Convert.ToBoolean(po["checkmark"]);
            return tmp;
        }
        ParseObject ToParseObject(Quote quote)
        {
            var parseobject = new ParseObject("InspirationalQuote");
            if (quote.ID != string.Empty)
            {
                parseobject.ObjectId = quote.ID;
            }
            parseobject["Message"] = quote.inspirationalQuote;
            parseobject["image"] = quote.picture;
            //parseobject.ACL = new ParseACL(ParseUser.CurrentUser);


            return parseobject;
        }

        public async Task <List<Quote>> RefreshQuoteAsync()
        {
            var query = ParseObject.GetQuery("InspirationalQuote");
            var result = await query.FindAsync();

            var quotesList = new List<Quote> ();
            foreach (var quote in result) {
         //       String quoteOfDay = IQuotes.inspirationalQuote;
         //       System.Diagnostics.Debug.WriteLine(quoteOfDay, Quotes.ID);

                quotesList.Add(FromParseObj(quote));

            }

            //var Items = new List<SampleItem>();
            //foreach (var item in result)
            //{
            //    //if (item.ACL != null){
            //    Items.Add(FromParseObject(item));
            //    //}
            //}

            return quotesList;
        }

    }
}