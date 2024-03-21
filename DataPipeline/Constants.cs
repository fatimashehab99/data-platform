using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.EntitySql;
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
        public const string ERROR_POST_URL_IS_EMPTY = "Post url  is empty";



        public const string ERROR_INVALID_IP = "Invalid Ip";
        public const string GEOLITEDB = "Invalid Ip";





        //MongoDB constants

        public static string USER_ID = "UserId";
        public static string POST_TAG = "PostTags";
        public static string POST_AUTHOR = "PostAuthor";
        public static string POST_URL = "PostUrl";
        public static string POST_IMAGE = "PostImage";
        public static string DOMAIN = "Domain";
        public static string DATE = "Date";
        public static string Category = "PostCategory";
        public static string USERID = "UserId";
        public static string POST_TITLE = "PostTitle";
        public static string FORMATTED_DATE = "FormattedDate";
        public static string COUNTRY_NAME = "CountryName";
        public static string POST_ID = "PostId";
        public static string ID = "_id";


        public static string TAG_INDEX = "tag_index";
        public static string TAGS_COUNT = "tags_count";
        public static string AUTHORS = "authors";
        public static string ARTICLES = "articles";
        public static string USERS = "users";
        public static string TOTAL_PAGE_VIEWS = "TotalPageviews";
        public static string TOTAL_AUTHORS = "TotalAuthors";
        public static string TOTAL_ARTICLES = "TotalArticles";
        public static string TOTAL_USERS = "TotalUsers";


        //agg functions
        public static string IN = "$in";
        public static string OR = "$or";
        public static string FIRST = "$first";
        public static string UN_WIND = "$unwind";
        public static string MATCH = "$match";
        public static string GROUP = "$group";
        public static string COUNT = "$count";
        public static string SUM = "$sum";
        public static string SORT = "$sort";
        public static string LIMIT = "$limit";
        public static string NOT = "$ne";
        public static string GREATER = "$gt";
        public static string SIZE = "$size";
        public static string PROJECT = "$project";
        public static string ADD_TO_SET = "$addToSet";



    }
}
