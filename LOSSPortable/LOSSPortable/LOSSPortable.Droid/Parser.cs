using System;

using Parse;

namespace LOSSPortable.Droid
{
    public class Parser
    {
        static Parser serviceInstance = new Parser();

        public static Parser Default { get { return serviceInstance; } }

        protected Parser()
        {
            ParseClient.Initialize(Constants.ApplicationID,Constants.Key);
        }
    }
}