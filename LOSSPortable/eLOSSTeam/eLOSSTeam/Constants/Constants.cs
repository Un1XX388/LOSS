using Amazon;
using System;
using Xamarin.Forms;


namespace eLOSSTeam
{
    public static class Constants
    {
        public const string COGNITO_IDENTITY_POOL_ID = "us-east-1:1e9f9de8-f5c8-406b-bacb-58afdd3c8b4b";

        public const string AWSLambdaID = "arn:aws:lambda:us-east-1:138075618342:function:Test_Backend";

        //GOOGLE ACCOUNT : rtpraetoriusUTA@gmail.com
        public const string AndroidPlatformApplicationArn = "arn:aws:sns:us-east-1:138075618342:app/GCM/eLOSSTeam";

        //NEEDS TO BE UPDATED : rtpraetoriusUTA@gmail.com
        public const string iOSPlatformApplicationArn = "";

        //GOOGLE ACCOUNT : rtpraetoriusUTA@gmail.com : project : elossteam
        public const string GoogleConsoleProjectId = "306176610411";

        public static RegionEndpoint COGNITO_REGION = RegionEndpoint.USEast1;

        public static RegionEndpoint DYNAMODB_REGION = RegionEndpoint.USEast1;

        public static DateTime date;
        public static Conversation conv = new Conversation();
        public static Boolean internetConnection = true;
        public static Color backGroundColor = Color.FromHex("FEFCEC");
        public static Color leftMessageColor = Color.FromHex("E7DFE7");
        public static Color rightMessageColor = Color.FromHex("DEE6D6");
    }
}
