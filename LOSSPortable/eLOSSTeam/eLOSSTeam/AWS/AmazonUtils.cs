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

namespace eLOSSTeam
{
    public class AmazonUtils
    {
        //AWS Cognito credentials generated from server, based off r
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

        //setups up client ID for DynamoDB
        private static AmazonDynamoDBClient _client;

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
        {
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
            IOS
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
        /**
         * Part of the SNS device registration, generates arn endpoint for use with SNS push
         * notifications, implemented for iOS and Android
         */
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
            System.Diagnostics.Debug.WriteLine(_endpointArn);
            Helpers.Settings.EndpointArnSetting = _endpointArn;
        }

        /**
         * Method that retrieves all the quotes stored in the quotes 
         * table on dynamoDB
         */
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

        //temporary caching of the quotes pulled from the server, stays till app shut down
        private static RangeObservableCollection<InspirationalQuote> quotesList = new RangeObservableCollection<InspirationalQuote>();

        public static RangeObservableCollection<InspirationalQuote> getQuotesList
        {
            get
            {
                return quotesList;
            }

        }

        //==============================================================================
        /**
         * Method that retrieves all the miscellaneous items stored in the Miscellaneous 
         * table on dynamoDB
         */
        private static Task<List<Miscellaneous>> queryMiscList()
        {
            var context = AmazonUtils.DDBContext;
            List<ScanCondition> conditions = new List<ScanCondition>();
            var SearchBar = context.ScanAsync<Miscellaneous>(conditions);
            return SearchBar.GetNextSetAsync();
        }

        public static void updateMiscellaneousList()
        {
            queryMiscList().ContinueWith(task =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    miscList.AddRange(queryMiscList().Result);
                });
            });
        }

        //temporary caching of the quotes pulled from the server, stays till app shut down
        private static RangeObservableCollection<Miscellaneous> miscList = new RangeObservableCollection<Miscellaneous>();

        public static RangeObservableCollection<Miscellaneous> getMiscList
        {
            get
            {
                return miscList;
            }

        }

        //===============================================================================

        /**
         * retrieval of the online resources from the dynamoDB
         * pulls the entire table
         */
        private static Task<List<OnlineRViewModel>> queryOnlineRList()
        {
            var context = AmazonUtils.DDBContext;
            List<ScanCondition> conditions = new List<ScanCondition>();
            var SearchBar = context.ScanAsync<OnlineRViewModel>(conditions);
            return SearchBar.GetNextSetAsync();

            //System.Diagnostics.Debug.WriteLine("inside queryOnlineRList()");
        }

        //main driving method for retrieving and storing online resource information
        public static void updateOnlineRList()
        {
            if (getOnlineRList != null)
            {
                getOnlineRList.Clear();
            }
            onlineRList = new RangeObservableCollection<OnlineRViewModel>();
            queryOnlineRList().ContinueWith(task =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    onlineRList.AddRange(queryOnlineRList().Result);
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
        }

        //-----------------------------------------------------------------------------------------
        private static Task<List<OnlineVViewModel>> queryOnlineVList()
        {
            var context = AmazonUtils.DDBContext;
            List<ScanCondition> conditions = new List<ScanCondition>();
            var SearchBar = context.ScanAsync<OnlineVViewModel>(conditions);
            return SearchBar.GetNextSetAsync();

            //System.Diagnostics.Debug.WriteLine("inside queryOnlineRList()");
        }

        //Main driver method for retrieving video resources from the DB
        public static void updateOnlineVList()
        {
            if (onlineVList != null)
            {
                onlineVList.Clear();
            }
            onlineVList = new RangeObservableCollection<OnlineVViewModel>();
            queryOnlineVList().ContinueWith(task =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    onlineVList.AddRange(queryOnlineVList().Result);
                    });
            });
        }

        private static RangeObservableCollection<OnlineVViewModel> onlineVList = new RangeObservableCollection<OnlineVViewModel>();

        public static RangeObservableCollection<OnlineVViewModel> getOnlineVList
        {
            get
            {
                return onlineVList;
            }
        }

        private static Task<List<OnlinePlaylistModel>> queryOnlinePlaylist()
        {
            var context = AmazonUtils.DDBContext;
            List<ScanCondition> conditions = new List<ScanCondition>();
            var SearchBar = context.ScanAsync<OnlinePlaylistModel>(conditions);
            return SearchBar.GetNextSetAsync();

        }

        //Main driver function for updating playlist objects
        public static void updateOnlinePlaylist()
        {
            if (onlinePlaylist != null)
            {
                onlinePlaylist.Clear();
            }
            onlinePlaylist = new RangeObservableCollection<OnlinePlaylistModel>();
            queryOnlinePlaylist().ContinueWith(task =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    onlinePlaylist.AddRange(queryOnlinePlaylist().Result);
                    //System.Diagnostics.Debug.WriteLine("UpdateOnlineRList()");
                });
            });
        }

        private static RangeObservableCollection<OnlinePlaylistModel> onlinePlaylist = new RangeObservableCollection<OnlinePlaylistModel>();

        public static RangeObservableCollection<OnlinePlaylistModel> getOnlinePlaylist
        {
            get
            {
                return onlinePlaylist;
            }
        }
    }
}

    /**
     * Class that extends observable collection that allows a range of objects to be added to the list
     * without trigger OnCollectionChanged repetitively.
     */
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

