﻿using Amazon.DynamoDBv2.DataModel;
using System;
using Amazon.Lambda.Model;

namespace eLOSSTeam
{

    [DynamoDBTable("UnregisteredUser")]
    public class UserInfo
    { //Latitude = , Longitude=, Nickname=, SNSID= , UserType=  
        [DynamoDBProperty("Latitude")]
        public double Latitude { get; set; }

        [DynamoDBProperty("Longitude")]
        public double Longitude { get; set; }

        [DynamoDBProperty("Nickname")]
        public string Nickname { get; set; }

        [DynamoDBProperty("Arn")]
        public string Arn { get; set; }

        [DynamoDBProperty("ID")]
        public string ID { get; set; }

        public bool Available { get; set; }

    }
}
