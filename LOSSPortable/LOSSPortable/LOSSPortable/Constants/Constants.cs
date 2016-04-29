using Amazon;
using System;

namespace LOSSPortable
{
    public static class Constants
    {
        public const string COGNITO_IDENTITY_POOL_ID = "us-east-1:5f25945a-80d3-4897-ba9b-a492d1adefcb";

        public const string AndroidPlatformApplicationArn = "arn:aws:sns:us-east-1:987221224788:app/GCM/LossPortable";

        public const string iOSPlatformApplicationArn = "arn:aws:sns:us-east-1:987221224788:app/APNS_SANDBOX/LossPortable";

        public const string GoogleConsoleProjectId = "889411264837";

        public static RegionEndpoint COGNITO_REGION = RegionEndpoint.USEast1;

        public static RegionEndpoint DYNAMODB_REGION = RegionEndpoint.USEast1;

        public static DateTime date;
        public static Conversation conv = new Conversation();
    }
}
