using Amazon;
using System;
using Xamarin.Forms;


namespace eLOSSTeam
{
    public static class Constants
    {
        public const string COGNITO_IDENTITY_POOL_ID = "us-east-1:5f25945a-80d3-4897-ba9b-a492d1adefcb";

        //GOOGLE ACCOUNT : rtpraetoriusUTA@gmail.com
        public const string AndroidPlatformApplicationArn = "arn:aws:sns:us-east-1:987221224788:app/GCM/eLOSSTeam";

        //NEEDS TO BE UPDATED : rtpraetoriusUTA@gmail.com
        public const string iOSPlatformApplicationArn = "arn:aws:sns:us-east-1:987221224788:app/APNS_SANDBOX/LossPortable";

        //GOOGLE ACCOUNT : rtpraetoriusUTA@gmail.com : project : elossteam
        public const string GoogleConsoleProjectId = "306176610411";

        public static RegionEndpoint COGNITO_REGION = RegionEndpoint.USEast1;

        public static RegionEndpoint DYNAMODB_REGION = RegionEndpoint.USEast1;

        public static DateTime date;
        public static Conversation conv = new Conversation();

        public static Color backGroundColor = Color.FromHex("FEFCEC");
        public static Color leftMessageColor = Color.FromHex("E7DFE7");
        public static Color rightMessageColor = Color.FromHex("DEE6D6");
    }
}
