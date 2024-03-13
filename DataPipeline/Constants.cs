using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline
{
    public class Constants
    {
        //nullable fields exception constants 
        public const string ERROR_BROWSER_IS_EMPTY = "Browser is empty";
        public const string ERROR_IP_IS_EMPTY = "Ip is empty";
        public const string ERROR_USER_ID_IS_EMPTY = "User id is empty";
        public const string ERROR_POST_ID_IS_EMPTY = "Post id is empty";
        public const string ERROR_DEVICE_IS_EMPTY = "Device is empty";
        public const string ERROR_SUBSCRIPTION_ID_IS_EMPTY = "Subscription id is empty";
        public const string ERROR_DATE_IS_EMPTY = "Date is empty";
        public const string ERROR_AUTHOR_IS_EMPTY = "Author is empty";
        public const string ERROR_OPERATING_IS_EMPTY = "Operating is empty";
        public const string ERROR_COLLECTION_IS_EMPTY = "Collection is empty";


        public const string ERROR_INVALID_IP = "Invalid Ip";
        public const string GEOLITEDB = "Invalid Ip";





        //MongoDB constants

        public static string POST_AUTHOR = "PostAuthor";
        public static string DOMAIN = "Domain";
        public static string DATE = "Date";
        public static string Category = "PostCategory";
        public static string USERID = "UserId";
        public static string POST_TITLE = "PostTitle";
        public static string FORMATTED_DATE = "Formatted_Date";
        public static string COUNTRY_NAME = "Country_Name";

    }
}
