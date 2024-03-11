using MongoDB.Driver;
using DataPipeline;
using DataPipeline.DataCollection.Models;

namespace DataPipelineTest
{
    public class TrackTest : BaseTest
    {

        //Section 1:Nullable fields tests

        /// <summary>
        ///Checking if it throws an exception while generating new page view having empty ip 
        /// </summary>
        [Test]
        public void if_no_ip_code_should_throw_exepction()
        {
            //generating a new page view then making ip null for testing purpose 
            MongoDbPageView page = GeneratePageView();
            page.Ip = "";

            var exception = Assert.Throws<Exception>(()
                => _trackService.LogPageview(page));

            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_IP_IS_EMPTY));
        }

        /// <summary>
        ///Checking if it throws an exception while generating new page view having 
        ///empty user id 
        /// </summary>
        [Test]
        public void if_no_user_id_should_throw_exception()
        {
            //generating a new page view then making user id null for testing purpose 
            MongoDbPageView page = GeneratePageView();
            page.UserId = "";
            var exception = Assert.Throws<Exception>(()
                => _trackService.LogPageview(page));
            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_USER_ID_IS_EMPTY));
        }
        /// <summary>
        ///Checking if it throws an exception while generating new page view having 
        ///empty post id
        /// </summary>
        [Test]
        public void if_no_post_id_should_throw_exception()
        {
            //generating a new page view then making post id null for testing purpose 
            MongoDbPageView page = GeneratePageView();
            page.PostId = "";

            var exception = Assert.Throws<Exception>(()
                => _trackService.LogPageview(page));
            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_POST_ID_IS_EMPTY));
        }
        /// <summary>
        ///Checking if it throws an exception while generating new page view having 
        ///empty browser
        /// </summary>
        [Test]

        public void if_no_browser_should_throw_exception()
        {
            //generating a new page view then making browser null for testing purpose
            MongoDbPageView page = GeneratePageView();
            page.Browser = "";

            var exception = Assert.Throws<Exception>(()
                => _trackService.LogPageview(page));
            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_BROWSER_IS_EMPTY));
        }
        /// <summary>
        ///Checking if it throws an exception while generating new page view having 
        ///empty device
        /// </summary>
        [Test]
        public void if_no_device_should_throw_exception()
        {
            //generating a new page view then making device null for testing purpose
            MongoDbPageView page = GeneratePageView();
            page.Device = "";
            var exception = Assert.Throws<Exception>(()
                => _trackService.LogPageview(page));
            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_DEVICE_IS_EMPTY));
        }
        /// <summary>
        ///Checking if it throws an exception while generating new page view having 
        ///empty subscription id 
        /// </summary>
        [Test]
        public void if_no_subscription_id_should_throw_exception()
        {
            //generating a new page view then making subscription id null for testing purpose
            MongoDbPageView page = GeneratePageView();
            page.Domain = "";

            var exception = Assert.Throws<Exception>(()
                => _trackService.LogPageview(page));
            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_SUBSCRIPTION_ID_IS_EMPTY));
        }
        /// <summary>
        ///Checking if it throws an exception while generating new page view having 
        ///empty author
        /// </summary>
        [Test]
        public void if_no_author_should_throw_exception()
        {
            //generating a new page view then making author null for testing purpose
            MongoDbPageView page = GeneratePageView();
            page.PostAuthor = "";

            var exception = Assert.Throws<Exception>(()
                => _trackService.LogPageview(page));
            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_AUTHOR_IS_EMPTY));
        }
        /// <summary>
        ///Checking if it throws an exception while generating new page view having 
        ///empty operating
        /// </summary>
        [Test]
        public void if_no_operating_should_throw_exception()
        {
            //generating a new page view then making operating null for testing purpose
            MongoDbPageView page = GeneratePageView();
            page.Platform = "";

            var exception = Assert.Throws<Exception>(()
                => _trackService.LogPageview(page));
            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_OPERATING_IS_EMPTY));
        }

        /// <summary>
        /// Checking if the ip is valid
        /// </summary>
        [Test]
        public void if_invalid_ip_should_throw_exception()
        {
            //generating a new page view then update the ip to an
            //invalid format for testing purpose
            MongoDbPageView page = GeneratePageView();
            page.Ip = "1";

            var exception = Assert.Throws<Exception>(()
                => _trackService.LogPageview(page));
            Assert.That(exception.Message, Is.EqualTo(Constants.ERROR_INVALID_IP));
        }

        /// <summary>
        /// Checking if we can get country name by ip using getCountryName function
        /// </summary>
        [Test]
        public void checkIfWeCanGetCountryNameFromLocation()
        {
            MongoDbPageView page = GeneratePageView();

            page.Country_Name = _locationService.getCountryName(page.Ip);


            Assert.That("Lebanon", Is.EqualTo(page.Country_Name));

        }


        /// <summary>
        /// This function is used to check if page view was added or not
        /// </summary>
        [Test]
        public void checkPageViewRecordByPostId()
        {
            //generate new page view
            MongoDbPageView page = GeneratePageView();

            //save data in mongodb
            _trackService.LogPageview(page);

            //read page View from mongo db
            var pageview = _trackService.GetPageViewByPostId(page.PostId);


            Assert.That(pageview.PostId, Is.EqualTo(page.PostId));
        }





    }
}