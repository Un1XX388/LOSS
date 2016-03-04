using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda;

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
        
        public static AmazonLambdaClient _lambdaClient;

        public static AmazonLambdaClient LambdaClient
        {//"AKIAIP5E5KYETNCXDSCA", "tRhWsuOFIND4DIbvijc4HD5QPjeuTr6h6f9kgUP"
            get
            {
                if (_lambdaClient == null)
                    _lambdaClient = new AmazonLambdaClient(Credentials, Amazon.RegionEndpoint.USEast1);
                return _lambdaClient;
            }
        }
    }
}
