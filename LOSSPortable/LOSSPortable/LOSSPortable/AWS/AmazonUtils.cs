using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.Lambda;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Specialized;

namespace LOSSPortable
{
    public class AmazonUtils
    {
        private static CognitoAWSCredentials _credentials;

        public static CognitoAWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(Constants.COGNITO_IDENTITY_POOL_ID, Constants.COGNITO_REGION);
                return _credentials;
            }
        }

        public static AmazonDynamoDBClient _client;

        public static AmazonDynamoDBClient DynamoDBClient
        {
            get
            {
                if (_client == null)
                    _client = new AmazonDynamoDBClient(Credentials, Constants.DYNAMODB_REGION);
                return _client;
            }
        }


        private static DynamoDBContext _context;

        public static DynamoDBContext DDBContext
        {
            get
            {
                if (_context == null)
                    _context = new DynamoDBContext(DynamoDBClient);
                return _context;
            }
        }

        private static AmazonS3Client _s3client;

        public static AmazonS3Client S3Client
        {
            get
            {
                if (_s3client == null)
                    _s3client = new AmazonS3Client(Credentials, Constants.COGNITO_REGION);
                return _s3client;
            }
        }

        private static TransferUtility _transferUtility;
        
        private static TransferUtilityConfig _transferConfig = new TransferUtilityConfig
        {
            ConcurrentServiceRequests = 10,
            MinSizeBeforePartUpload = 16 * 1024 * 1024
        };

        public static TransferUtility transferUtility {
            get
            {
                if (_transferUtility == null)
                    _transferUtility = new TransferUtility(S3Client, _transferConfig);
                return _transferUtility;
            }
        }

        public TransferUtilityDownloadRequest transferUtilityRequest(string key){
            
            return new TransferUtilityDownloadRequest()
            {
                FilePath = "",
                BucketName = "idansbucket",
                Key = key,
            };
        }
        
        private static AmazonLambdaClient _lambdaClient;

        public static AmazonLambdaClient LambdaClient
        {//"AKIAIP5E5KYETNCXDSCA", "tRhWsuOFIND4DIbvijc4HD5QPjeuTr6h6f9kgUP"
            get
            {
                if (_lambdaClient == null)
                    _lambdaClient = new AmazonLambdaClient(Credentials, Constants.COGNITO_REGION);
                return _lambdaClient;
            }
        }


        public enum Platform
        {
            Android,
            IOS,
            WindowsPhone
        }

        private static IAmazonSimpleNotificationService _snsClient;

        private static IAmazonSimpleNotificationService SNSClient
        {
            get
            {
                if (_snsClient == null)
                    _snsClient = new AmazonSimpleNotificationServiceClient(Credentials, Constants.COGNITO_REGION);
                return _snsClient;
            }
        }

        public static async Task RegisterDevice(Platform platform, string registrationID)
        {
            var arn = string.Empty;
            string _endpointArn = string.Empty;
            switch (platform)
            {
                case Platform.Android:
                    arn = Constants.AndroidPlatformApplicationArn;
                    break;
                case Platform.IOS:
                    arn = Constants.iOSPlatformApplicationArn;
                    break;
            }

            var response = await SNSClient.CreatePlatformEndpointAsync(new CreatePlatformEndpointRequest
                {
                    Token = registrationID,
                    PlatformApplicationArn = arn
                }
            );
            
            _endpointArn = response.EndpointArn;
            Helpers.Settings.EndpointArnSetting = _endpointArn;
        }

        private static Task<List<InspirationalQuote>> queryQuotesList()
        {
            var context = AmazonUtils.DDBContext;
            List<ScanCondition> conditions = new List<ScanCondition>();
            var SearchBar = context.ScanAsync<InspirationalQuote>(conditions);
            return SearchBar.GetNextSetAsync();
        }

        public static void updateInspirationalQuoteList()
        {
            queryQuotesList().ContinueWith(task =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    quotesList.AddRange(queryQuotesList().Result);
                });
            });
        }


        private static RangeObservableCollection<InspirationalQuote> quotesList = new RangeObservableCollection<InspirationalQuote>();

        public static RangeObservableCollection<InspirationalQuote> getQuotesList
        {
            get
            {
                return quotesList;
            }
            set
            {
                quotesList = value;
            }
        }


        private static Task<List<OnlineRViewModel>> queryOnlineRList()
        {
            var context = AmazonUtils.DDBContext;
            List<ScanCondition> conditions = new List<ScanCondition>();
            var SearchBar = context.ScanAsync<OnlineRViewModel>(conditions);
            return SearchBar.GetNextSetAsync();

            System.Diagnostics.Debug.WriteLine("inside queryOnlineRList()");
        }

        public static void updateOnlineRList()
        {
            queryOnlineRList().ContinueWith(task =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    onlineRList.AddRange(queryOnlineRList().Result);
                    System.Diagnostics.Debug.WriteLine("UpdateOnlineRList()");
                });
            });
        }

        private static RangeObservableCollection<OnlineRViewModel> onlineRList = new RangeObservableCollection<OnlineRViewModel>();

        public static RangeObservableCollection<OnlineRViewModel> getOnlineRList
        {
            get
            {
                return onlineRList;
            }
            set
            {
                onlineRList = value;
            }
        }

    }//end of class AmazonUtils

    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        private bool surpressEvents = false;

        public void AddRange(IEnumerable<T> items)
        {
            surpressEvents = true;
            foreach (var item in items)
            {
                base.Add(item);
            }
            this.surpressEvents = false;
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items.ToList()));

        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!this.surpressEvents)
            {
                base.OnCollectionChanged(e);
            }
        }
    }
}
