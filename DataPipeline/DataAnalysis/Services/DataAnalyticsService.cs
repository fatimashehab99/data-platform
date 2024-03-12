using DataPipeline.DataAnalysis.Models;
using DataPipeline.DataCollection.Models;
using LinqKit;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DataPipeline.DataAnalysis.Services
{


    public class DataAnalyticsService : IDataAnalyticsService
    {
        private readonly IMongoCollection<MongoDbPageView> _collection;
        private readonly IDashboardStatisticsService _dashboardStatisticsService;


        public DataAnalyticsService(IOptions<DatabaseConnecting> DatabaseSettings, IDashboardStatisticsService dashboardStatisticsService)
        {
            //mongo DB connection 
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);
            ///get pageView collection 
            _collection = mongoDatabase.GetCollection<MongoDbPageView>(DatabaseSettings.Value.CollectionName);
            _dashboardStatisticsService = dashboardStatisticsService;
        }

        public List<AuthorPageView> AnalyseByAuthor(SearchCriteria criteria)
        {
            // Define the aggregation pipeline stages
            var matchStage = new BsonDocument("$match", new BsonDocument
             {
               { Constants.DOMAIN, criteria.Domain },
                { "PostAuthor", new BsonDocument("$ne", BsonNull.Value) }
                     });

            var groupStage = new BsonDocument("$group", new BsonDocument
                        {
                            { "_id", "$" + Constants.POST_AUTHOR }, // Group by author
                            { "PageView", new BsonDocument("$sum", 1) } // Sum the PageView values for each author
                        });
            var orderByStage = new BsonDocument("$sort", new BsonDocument("pageviews", -1));
            var limitStage = new BsonDocument("$limit", 10);

            var pipeline = new[] { matchStage, groupStage, orderByStage, limitStage };

            List<BsonDocument> pResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();

            var results = new List<AuthorPageView>();

            foreach (BsonDocument pResult in pResults)
            {
                results.Add(new AuthorPageView
                {
                    Author = pResult["_id"].AsString, // Get the author from the _id field
                    PageViews = pResult["PageView"].AsInt32 // Get the PageView value
                });
            }

            return results;
        }
        public List<DatePageView> AnalyzePageViewsByDate(SearchCriteria criteria)
        {
            //Define the aggregation pipeline stages
            //first we need to filter data by domain
            var matchStage = new BsonDocument("$match", new BsonDocument
                {
                  { Constants.DOMAIN, criteria.Domain },
                  { Constants.FORMATTED_DATE, new BsonDocument("$gt", "2024-03-02") }
                   });
            //now we need to get the count of page views for each day
            var groupStage = new BsonDocument("$group", new BsonDocument
            {
                {"_id","$"+Constants.FORMATTED_DATE},//group by date
                {"pageviews",new BsonDocument("$sum",1) }//count the pageviews

            });
            ///now we need to order the results by date
            var orderByStage = new BsonDocument("$sort", new BsonDocument("_id", -1));
            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, orderByStage };
            //execute the pipeline then store the results in list 
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();
            var results = new List<DatePageView>();
            //loop over the results to store them in DatePageView List
            foreach (BsonDocument pipelineResult in pipelineResults)
            {
                results.Add(new DatePageView
                {
                    Date = pipelineResult["_id"].AsString,
                    PageViews = pipelineResult["pageviews"].AsInt32

                });
            }
            return results;

        }

        public List<CategoryPageView> AnalyzePageViewsByCategory(SearchCriteria criteria)
        {
            //Define the aggregation pipeline stages
            //first we need to filter data by domain
            var matchStage = new BsonDocument("$match", new BsonDocument(Constants.DOMAIN, criteria.Domain));
            //now we need to get the count of pageviews for each category 
            var groupStage = new BsonDocument("$group", new BsonDocument
            {
                {"_id","$" + Constants.Category },//group by date
                {"pageviews",new BsonDocument("$sum",1) }//count the pageviews

            });
            var orderByStage = new BsonDocument("$sort", new BsonDocument("pageviews", -1));
            var limitStage = new BsonDocument("$limit", 10);
            //initialize the pipeline
            var pipeline = new[] { matchStage, groupStage, orderByStage, limitStage };
            //execute the pipeline then store the results in list 
            List<BsonDocument> pipelineResults = _collection.Aggregate<BsonDocument>(pipeline).ToList();
            var results = new List<CategoryPageView>();
            //loop over the results to store them in DatePageView List
            foreach (BsonDocument pipelineResult in pipelineResults)
            {
                results.Add(new CategoryPageView
                {
                    Category = pipelineResult["_id"].AsString,
                    PageViews = pipelineResult["pageviews"].AsInt32

                });
            }
            return results;
        }
        public DashboardStatistics getDashboardStatisticsData(SearchCriteria criteria)
        {
            int authors = _dashboardStatisticsService.getTotalAuthors(criteria);
            int pageViews = _dashboardStatisticsService.getTotalPageViews(criteria);
            int articles = _dashboardStatisticsService.getTotalArticles(criteria);
            int users = _dashboardStatisticsService.getTotalUsers(criteria);
            DashboardStatistics data = new DashboardStatistics()
            {
                authors = authors,
                pageViews = pageViews,
                articles = articles,
                users = users
            };
            return data;

        }





        //-----------Post Page Start----------------------------------------------------------------------------------
        public class PageViewsAggregateResult
        {
            public string Date { get; set; }
            public int Counter { get; set; }
        }
        /// <summary>
        /// get specific page view
        /// </summary>
        /// <param name="data"></param>
        /// <param name="subId"></param>
        /// <param name="pid"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public dynamic AnalyzePost(SearchCriteria data, string subId, string pid, DateTime start, DateTime end, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            var projection = Builders<MongoDbPageView>.Projection
           .Expression(x => new
           {
               Date = new BsonDocument("$dateToString", new BsonDocument {
                    { "format", "%Y-%m-%d" },
                    { "date", x.Date }
               }),
               Count = 1
           });
            //Group
            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument("$dateToString", new BsonDocument {
                            { "format", "%Y-%m-%d" },
                            { "date", "$Date" }
                        })},
                        { "Counter", new BsonDocument("$sum", 1) }
                    }
                }
            };


            var query = _collection.Aggregate()
                .Match(x => x.Domain == subId && x.PostId == pid)
                .Match(predicate)
                .Project(projection)
                .AppendStage<BsonDocument>(group)
                .SortBy(x => x["_id"])
                .ToList()
                .Select(x => new PageViewsAggregateResult
                {
                    Date = x["_id"].AsString,
                    Counter = x["Counter"].AsInt32
                })
                .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query;
        }

        //Gets this Post Locations by PostId
        public List<dynamic> PostgetLocations(SearchCriteria data, string subId, string pid, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            //Aggregate Mng-DB
            var query =
               _collection.Aggregate()
                             .Match(predicate)
                             .Match(x => x.Domain == subId && x.PostId == pid)
                             .Group(e => e.Country_Code, g => new { Location = g.Key, Views = g.Count() })
                             .SortByDescending(x => x.Views)
                             .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //Gets this Post Devices  --> grouped by PostId
        public List<dynamic> PostgetDevices(SearchCriteria data, string subId, string pid, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            //Aggregate Mng-DB
            var query =
               _collection.Aggregate()
                             .Match(predicate)
                             .Match(x => x.Domain == subId && x.PostId == pid)
                             .Group(e => e.Device, g => new { Device = g.Key, Views = g.Count() })
                             .SortByDescending(x => x.Views)
                             .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //-----------Post Page End----------------------------------------------------------------------------------

        //------------------Shared Filter predicate
        /// <summary>
        /// This function is used for filtering data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Expression<Func<MongoDbPageView, bool>> SharedCategory(SearchCriteria data)
        {
            var predicate = PredicateBuilder.True<MongoDbPageView>();
            if (data.Domain != null)
                predicate = predicate.And(c => c.Domain == data.Domain);
            if (data.Category != null && data.Category != "All")
                predicate = predicate.And(c => c.PostCategory == data.Category);
            if (data.Author != null && data.Author != "All")
                predicate = predicate.And(c => c.PostAuthor == data.Author);
            if (data.Device != null && data.Device != "All")
                predicate = predicate.And(c => c.Device == data.Device);
            if (data.Domain != null)
                predicate = predicate.And(c => c.Domain == data.Domain);
            if (data.From != DateTime.MinValue && data.To != DateTime.MinValue)
            {
                predicate = predicate.And(c => c.Date >= data.From);
                predicate = predicate.And(c => c.Date <= data.To);
            }
            return predicate;
        }

        public int CalculateMonthIndex(int currentMonthCounter, int previousMonthCounter)
        {
            // Calculate the percentage change in the data
            double percentageChange = 0;

            if (previousMonthCounter > 0 && currentMonthCounter > 0)
            {
                percentageChange = (currentMonthCounter - previousMonthCounter) / (double)previousMonthCounter * 100;
            }
            else if (previousMonthCounter > 0)
            {
                percentageChange = -100;
            }
            else if (currentMonthCounter > 0)
            {
                percentageChange = 100;
            }

            int roundedPercentage = (int)Math.Round(percentageChange, 0);
            int finalPercentage = Math.Clamp(roundedPercentage, -100, 100);

            return finalPercentage;
        }

        //---------- Page Analytics----------------------------------------------------------------------------------

        //All PageViews according to subId
        public dynamic PagebySubId(SearchCriteria data, string subID, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            var projection = Builders<MongoDbPageView>.Projection
            .Expression(x => new
            {
                Date = new BsonDocument("$dateToString", new BsonDocument {
                    { "format", "%Y-%m-%d" },
                    { "date", x.Date }
                }),
                Count = 1
            });
            //Group
            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument("$dateToString", new BsonDocument {
                            { "format", "%Y-%m-%d" },
                            { "date", "$Date" }
                        })},
                        { "Counter", new BsonDocument("$sum", 1) }
                    }
                }
            };
            var recent = DateTime.Now.Date.AddMonths(-5);

            var query = _collection.Aggregate()
                .Match(x => x.Domain == subID)
                .Match(predicate)
                .Match(x => x.Date >= recent)
                .Project(projection)
                .AppendStage<BsonDocument>(group)
                .SortBy(x => x["_id"])
                .ToList()
                .Select(x => new PageViewsAggregateResult
                {
                    Date = x["_id"].AsString,
                    Counter = x["Counter"].AsInt32
                })
                .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query;
        }

        //All Locations according to subId
        public List<dynamic> PagegetLocations(SearchCriteria data, string subId, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            //Aggregate Mng-DB
            var query =
               _collection.Aggregate()
                             .Match(x => x.Domain == subId)
                            .Match(predicate)
                             .Group(e => e.Country_Code, g => new { Location = g.Key, Views = g.Count() })
                             .SortByDescending(x => x.Views)
                             .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //All Devices  according to subId
        public List<dynamic> PagegetDevices(SearchCriteria data, string subId, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            //Aggregate Mng-DB
            var query =
               _collection.Aggregate()
                             .Match(x => x.Domain == subId)
                             .Match(predicate)
                             .Group(e => e.Device, g => new { Device = g.Key, Views = g.Count() })
                             .SortByDescending(x => x.Views)
                             .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //ALL authors according
        public List<dynamic> GetPageAuthors(SearchCriteria data, string subId, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            var query =
            _collection.Aggregate()
                             .Match(x => x.Domain == subId && x.PostAuthor != null && x.PostAuthor != "")
                             .Match(predicate)
                             .Group(e => e.PostAuthor, g => new { Author = g.Key, Posted = g.Count() })
                             .SortByDescending(x => x.Posted)
                             .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //Filter Dropdown Category and Author 
        // Filter List Category
        public List<dynamic> SetCatergoryFiltering(string subId, out AnalyticsErrorEnum error)
        {
            var query =
               _collection.Aggregate()
                             .Match(x => x.Domain == subId && x.PostCategory != "")
                             .Group(e => e.PostCategory, g => new { Category = g.Key })
                             .SortBy(x => x.Category)
                             .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        // Filter List Authors
        public List<dynamic> SetAuthorFiltering(string subId, out AnalyticsErrorEnum error)
        {
            var query =
               _collection.Aggregate()
                             .Match(x => x.Domain == subId && x.PostAuthor != "")
                             .Group(e => e.PostAuthor, g => new { Author = g.Key })
                             .SortBy(x => x.Author)
                             .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        // Filter List Devices
        public List<dynamic> SetDeviceFiltering(string subId, out AnalyticsErrorEnum error)
        {
            var query =
               _collection.Aggregate()
                             .Match(x => x.Domain == subId && x.Device != "")
                             .Group(e => e.Device, g => new { Device = g.Key })
                             .SortBy(x => x.Device)
                             .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //Statistics
        public List<dynamic> Statistics(string subId, out AnalyticsErrorEnum error)
        {
            //Date
            var today = DateTime.Today.ToLocalTime();
            //yesterday
            var yesterday = today.AddDays(-1);
            //week
            var week = today.AddDays(-7);
            //month
            var month = today.AddDays(-31);


            var yesterdayUsers = _collection.Find(x => x.Date >= yesterday && x.Date < today)
            .Project(x => x.UserId)
            .ToList();
            var yesterdayUsersGrouped = yesterdayUsers.GroupBy(x => x).ToList();
            var yesterdayUserIds = yesterdayUsersGrouped.Select(g => g.Key).ToList();


            //all time Views Counter
            var projection = Builders<MongoDbPageView>.Projection
            .Expression(x => new
            {
                Date = new BsonDocument("$dateToString", new BsonDocument {
                    { "format", "%Y-%m-%d" },
                    { "date", x.Date }
                }),
                Count = 1
            });

            //Group Date
            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument("$dateToString", new BsonDocument {
                            { "format", "%Y-%m-%d" },
                            { "date", "$Date" }
                        })},
                        { "Counter", new BsonDocument("$sum", 1) }
                    }
                }
            };


            var allViews = _collection.Aggregate()
                .Match(x => x.Domain == subId)
                .Project(projection)
                .AppendStage<BsonDocument>(group)
                .ToList()
                .Select(x => new PageViewsAggregateResult
                {
                    Date = x["_id"].AsString,
                    Counter = x["Counter"].AsInt32
                })
                .ToList();
            var totalViews = allViews.Sum(x => x.Counter);


            //today new visitors
            var todayNewUsers = _collection.Aggregate()
                            .Match(s => s.Domain == subId)
                            .Match(x => x.Date >= today && !yesterdayUserIds.Contains(x.UserId))
                            .Group(e => e.UserId, g => new { TodayNewUsers = g.Count() }).ToList();
            var totalNewVisitors = todayNewUsers.Count;
            //today old visitors
            var todayOldUsers = _collection.Aggregate()
                .Match(s => s.Domain == subId)
                .Match(x => x.Date >= today && yesterdayUserIds.Contains(x.UserId))
                .Group(e => e.UserId, g => new { TodayOldUsers = g.Count() }).ToList();
            var totalOldVisitors = todayOldUsers.Count;

            //Visits today
            var todayVisits = _collection.Aggregate()
                            .Match(s => s.Domain == subId)
                            .Match(x => x.Date >= today)
                            .Group(e => e.UserId, g => new { Todayvisits = g.Count() }).ToList();
            var TodayVisitsCounter = todayVisits.Count;

            //Visits last week
            var weekVisit = _collection.Aggregate()
                .Match(s => s.Domain == subId)
                .Match(x => x.Date > week && x.Date <= yesterday)
                .Match(x => x.UserId != null)
                .Group(e => e.UserId, g => new { week = g.Count() })
                .ToList();
            var WeekVisits = weekVisit.Count;

            //Visits last month
            var monthVisit = _collection.Aggregate()
                .Match(s => s.Domain == subId)
                .Match(x => x.Date > month && x.Date <= yesterday)
                .Match(x => x.UserId != null)
                .Group(e => e.UserId, g => new { month = g.Count() })
                .ToList();
            var MonthVisits = monthVisit.Count;

            //all time visitors
            var uniqueVisitors = _collection.Aggregate()
            .Match(s => s.Domain == subId)
            .Match(x => x.UserId != null)
            .Group(e => e.UserId, g => new { UniqueVisitors = g.Count() })
            .ToList();
            var totalVisitors = uniqueVisitors.Count;

            //Total Published
            var publishedTotal = _collection.Aggregate()
                .Match(s => s.Domain == subId && s.PostPublishDate != null)
                .Group(e => e.PostPublishDate, g => new { published = g.Key, Counter = g.Count() })
                .ToList();
            var totalPublished = publishedTotal.Count;


            var query = new List<dynamic>
            {
                new { TotalVisitors = totalVisitors,TodayNewVisitors = totalNewVisitors, TodayOldVisitors = totalOldVisitors,TodayVisits=TodayVisitsCounter,WeekVisit=WeekVisits,MonthVisit=MonthVisits,TotalViews = totalViews,TotalPublished=totalPublished}
            };

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }
        public class TagAlt
        {
            public string tag { get; set; }
            public int weight { get; set; }
        }

        //Top 10 
        public List<dynamic> Top(SearchCriteria data, string subId, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            //Category
            var Category =
               _collection.Aggregate()
                             .Match(x => x.Domain == subId && x.PostCategory != "Default Category")
                             .Match(predicate)
                             .Group(e => e.PostCategory, g => new { Category = g.Key, Count = g.Count() })
                             .SortByDescending(x => x.Count)
                             .Limit(10)
                             .ToList();
            //Countries
            var Country =
               _collection.Aggregate()
                             .Match(x => x.Domain == subId && x.Country_Code != "")
                             .Match(predicate)
                             .Group(e => e.Country_Code, g => new { Country = g.Key, Count = g.Count() })
                             .SortByDescending(x => x.Count)
                             .Limit(10)
                             .ToList();
            //Countries
            var Pages =
               _collection.Aggregate()
                             .Match(x => x.Domain == subId && x.PostCategory == "Default Category")
                             .Match(predicate)
                             .Group(e => e.PostTitle, g => new { Title = g.Key, Count = g.Count() })
                             .SortByDescending(x => x.Count)
                             .Limit(10)
                             .ToList();

            var query = new List<dynamic>
            {
                new { CategoryList=Category,CountryList=Country,PagesList=Pages}
            };

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //Tags Cloud
        public List<dynamic> TagsCloud(SearchCriteria data, string subId, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);

            var query = _collection.Aggregate()
                .Match(x => x.Domain == subId)
                .Match(predicate)
                .Unwind("tags")
                .Group(new BsonDocument { { "_id", "$tags" }, { "weight", new BsonDocument("$sum", 1) } })
                .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            var result = query.Select(x => new TagAlt
            {
                tag = x["_id"].AsString,
                weight = x["weight"].AsInt32
            }).OrderByDescending(x => x.weight).Take(100).ToList();

            return result.Cast<dynamic>().ToList(); ;
        }

        //Category Cloud
        public List<dynamic> CategoryCloud(SearchCriteria data, string subId, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            var query =
               _collection.Aggregate()
               .Match(x => x.Domain == subId && x.PostCategory != "")
               .Match(predicate)
               .Group(e => e.PostCategory, g => new { Category = g.Key, weight = g.Count() })
               .SortByDescending(x => x.weight)
               .ToList();

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //CategoryList
        public List<dynamic> CategoryList(SearchCriteria data, string subID, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);
            //Aggregate Mng-DB
            var query = _collection.Aggregate()
                .Match(x => x.Domain == subID)
                             .Match(predicate)
                             .Match(x => x.PostCategory != "Default Category")
                             .Group(e => e.PostCategory, g => new { Category = g.Key, Views = g.Count() })
                             .SortByDescending(x => x.Views)
                             .Limit(20)
                             .ToList();
            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //Published List
        public dynamic PublishedList(SearchCriteria data, string subID, out AnalyticsErrorEnum error)
        {

            var predicate = SharedCategory(data);

            var projection = Builders<MongoDbPageView>.Projection
            .Expression(x => new
            {
                PublishDate = new BsonDocument("$dateToString", new BsonDocument {
                    { "format", "%Y-%m-%d" },
                    { "date", x.PostPublishDate }
                }),
                Count = 1
            });

            //Group
            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument("$dateToString", new BsonDocument {
                            { "format", "%Y-%m-%d" },
                            { "date", "$PublishDate" }
                        })},
                        { "Counter", new BsonDocument("$sum", 1) }
                    }
                }
            };

            var recent = DateTime.Now.Date.AddMonths(-1);
            var query = _collection.Aggregate()
                .Match(x => x.Domain == subID)
                .Match(x => x.PostPublishDate != DateTime.MinValue)
                .Match(x => x.PostPublishDate >= recent)
                .Project(projection)
                .AppendStage<BsonDocument>(group)
                .SortBy(x => x["_id"])
                .ToList()
                .Select(x => new PageViewsAggregateResult
                {
                    Date = x["_id"].AsString,
                    Counter = x["Counter"].AsInt32
                })
                .ToList();
            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //Active Users
        public List<dynamic> ActiveVisitors(string subId, out AnalyticsErrorEnum error)
        {
            // Define the time frames
            var activeFive = DateTime.Now.AddMinutes(-5);
            var activeTen = DateTime.Now.AddMinutes(-10);
            var activeHour = DateTime.Now.AddHours(-1);


            var pipeline = _collection.Aggregate()
                .Match(x => x.Domain == subId)
                .Group(e => e.UserId, g => new
                {
                    LastFive = g.Sum(x => x.Date >= activeFive ? 1 : 0),
                    LastTen = g.Sum(x => x.Date >= activeTen ? 1 : 0),
                    LastHour = g.Sum(x => x.Date >= activeHour ? 1 : 0)
                });


            var results = pipeline.ToList();

            // Extract the counts from the results
            var active5Counter = results.Count(r => r.LastFive > 0);
            var active10Counter = results.Count(r => r.LastTen > 0);
            var activeHourCounter = results.Count(r => r.LastHour > 0);

            var query = new List<dynamic>
            {
                new { LastFive =active5Counter,LastTen=active10Counter, LastHour=activeHourCounter }
            };

            // Return the output list and set the error to NO_ERROR
            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //Geo Map
        public List<dynamic> GetMap(string subId, SearchCriteria data, out AnalyticsErrorEnum error)
        {
            var predicate = SharedCategory(data);

            var query = _collection.Aggregate()
            .Match(x => x.Domain == subId)
            .Match(predicate)
            .Match(x => x.Country_Code != null)
            .Group(e => e.Country_Code, g => new { Country = g.Key, Values = g.Count() })
            .SortBy(x => x.Values)
            .ToList();

            var mappedQuery = query.Select(q => new
            {
                name = q.Country,
                value = q.Values,
                // id = _countryCodeMapping.TryGetValue(q.Country, out string code) ? code : "N/A"
            }).Where(x => x.value > 2);

            error = AnalyticsErrorEnum.NO_ERROR;
            return mappedQuery.Cast<dynamic>().ToList();
        }

        //Increase/Decrease Metric Percentage
        public List<dynamic> StatAnalyze(string subId, out AnalyticsErrorEnum error)
        {
            //Date
            var today = DateTime.Today.ToLocalTime();
            // last yesterday
            var yesterday = today.AddDays(-1);
            //last week
            var week = today.AddDays(-7);
            //last two week
            var weekTwo = today.AddDays(-14);
            //last month 
            var month = today.AddDays(-31);

            //2 month previous
            var monthTwo = today.AddDays(-61);

            //Views Index
            var projection = Builders<MongoDbPageView>.Projection
            .Expression(x => new
            {
                Date = new BsonDocument("$dateToString", new BsonDocument {
                    { "format", "%Y-%m-%d" },
                    { "date", x.Date }
                }),
                Count = 1
            });

            //Group Date
            var group = new BsonDocument
            {
                {
                    "$group", new BsonDocument
                    {
                        { "_id", new BsonDocument("$dateToString", new BsonDocument {
                            { "format", "%Y-%m-%d" },
                            { "date", "$Date" }
                        })},
                        { "Counter", new BsonDocument("$sum", 1) }
                    }
                }
            };

            var weekViews = _collection.Aggregate()
                .Match(x => x.Domain == subId)
                .Match(x => x.Date > week && x.Date <= today)
                .Project(projection)
                .AppendStage<BsonDocument>(group)
                .ToList()
                .Select(x => new PageViewsAggregateResult
                {
                    Date = x["_id"].AsString,
                    Counter = x["Counter"].AsInt32
                })
                .ToList();

            var lastweekViews = weekViews.Sum(x => x.Counter);

            var TwoweeksViews = _collection.Aggregate()
                .Match(x => x.Domain == subId)
                .Match(x => x.Date > weekTwo && x.Date < week)
                .Project(projection)
                .AppendStage<BsonDocument>(group)
                .ToList()
                .Select(x => new PageViewsAggregateResult
                {
                    Date = x["_id"].AsString,
                    Counter = x["Counter"].AsInt32
                })
                .ToList();

            var lastTwoweekViews = TwoweeksViews.Sum(x => x.Counter);

            //Weekly Index
            var ViewsIndex = CalculateMonthIndex(lastweekViews, lastTwoweekViews);


            //Visitors Index
            var lastMonthVisitors = _collection.Aggregate()
            .Match(s => s.Domain == subId)
            .Match(x => x.Date > month && x.Date <= yesterday)
            .Match(x => x.UserId != null)
            .Group(e => e.UserId, g => new { UniqueVisitors = g.Count() })
            .ToList();
            var monthVisitors = lastMonthVisitors.Count;

            var lastTwoMonthVisitors = _collection.Aggregate()
            .Match(s => s.Domain == subId)
            .Match(x => x.Date > monthTwo && x.Date <= month)
            .Match(x => x.UserId != null)
            .Group(e => e.UserId, g => new { UniqueVisitors = g.Count() })
            .ToList();
            var TwomonthVisitors = lastTwoMonthVisitors.Count;

            var VisitsIndex = CalculateMonthIndex(monthVisitors, TwomonthVisitors);

            //month Published
            var lastMonthPublished = _collection.Aggregate()
                .Match(s => s.Domain == subId)
                .Match(x => x.PostPublishDate > month && x.PostPublishDate <= yesterday)
                .Group(e => e.PostPublishDate, g => new { published = g.Key, Counter = g.Count() })
                .ToList();
            var monthPublished = lastMonthPublished.Count;

            var lastTwoMonthPublished = _collection.Aggregate()
                .Match(s => s.Domain == subId)
                .Match(x => x.PostPublishDate > monthTwo && x.PostPublishDate <= month)
                .Group(e => e.PostPublishDate, g => new { published = g.Key, Counter = g.Count() })
                .ToList();
            var TwomonthPublished = lastTwoMonthPublished.Count;

            var PublishIndex = CalculateMonthIndex(monthPublished, TwomonthPublished);

            var query = new List<dynamic>
            {
                new { ViewsIndicator = ViewsIndex, PublishIndicator=PublishIndex, VisitsIndicator=VisitsIndex}
            };

            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //Active Live Users
        public List<dynamic> LiveVisitors(string subId, out AnalyticsErrorEnum error)
        {
            //last 1 minutes from now 
            var live = DateTime.Now.AddMinutes(-1);

            //last 2 minutes from now 
            var livePrevious = DateTime.Now.AddMinutes(-2);

            var activecurrent =
              _collection.Aggregate()
                            .Match(x => x.Domain == subId)
                            .Match(x => x.Date >= live)
                            .Group(e => e.UserId, g => new { User = g.Key }).ToList();
            var activeCounter = activecurrent.Count;

            var activeprevious =
              _collection.Aggregate()
                            .Match(x => x.Domain == subId)
                            .Match(x => x.Date >= livePrevious && x.Date < live)
                            .Group(e => e.UserId, g => new { User = g.Key }).ToList();

            var activepreviousCounter = activeprevious.Count;

            var percentage = CalculateMonthIndex(activeCounter, activepreviousCounter);

            var query = new List<dynamic>
            {
                new { Active=activeCounter,Index=percentage }
            };
            error = AnalyticsErrorEnum.NO_ERROR;
            return query.Cast<dynamic>().ToList();
        }

        //Today Calc
        public dynamic TodayViews(string subId, out AnalyticsErrorEnum error)
        {
            //Date
            var today = DateTime.Today.ToLocalTime();

            //Visits today
            var views = _collection.Aggregate()
                            .Match(s => s.Domain == subId)
                            .Match(x => x.Date >= today)
                            .Group(e => e.UserId, g => new { views = g.Count() })
                            .ToList();
            var query = views.Count;
            error = AnalyticsErrorEnum.NO_ERROR;
            return query;
        }
        public dynamic TodayPublished(string subId, out AnalyticsErrorEnum error)
        {
            //Date
            var today = DateTime.Today.ToLocalTime();

            //Published today
            var publishes = _collection.Aggregate()
                .Match(s => s.Domain == subId)
                .Match(x => x.PostPublishDate >= today)
                .Group(e => e.PostPublishDate, g => new { published = g.Count() })
                .ToList();
            var query = publishes.Count;

            error = AnalyticsErrorEnum.NO_ERROR;
            return query;
        }
        public dynamic TodayUsers(string subId, out AnalyticsErrorEnum error)
        {
            //Date
            var today = DateTime.Today.ToLocalTime();

            //Users today
            var uniqueUsers = _collection.Aggregate()
            .Match(s => s.Domain == subId)
            .Match(x => x.Date >= today)
            .Group(e => e.UserId, g => new { uniqueUsers = g.Count() })
            .ToList();
            var query = uniqueUsers.Count;

            error = AnalyticsErrorEnum.NO_ERROR;
            return query;
        }

    }
}
