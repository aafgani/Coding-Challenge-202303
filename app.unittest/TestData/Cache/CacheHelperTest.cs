using Infrastructure.Cache;
using Microsoft.Extensions.Options;
using Model.AppConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.unittest.TestData.Cache
{
    public class CacheHelperTest
    {
        [Fact]
        public void CreateCacheFromEmptyShouldOk()
        {
            //Arrange
            IOptions<CacheConfiguration> _options = Options.Create(new CacheConfiguration()
            {
                AbsoluteExpiration = 5000,
                MaxEntry = 10
            });
            var cache = new MyOwnCacheHelper<int>(_options);
            int[] numberOfUsers = new int[] { 1001,1002,1003,1004,1005};

            //Act
            for (int i = 0; i < numberOfUsers.Count(); i++)
            {
                cache.GetOrCreate(numberOfUsers[i], () => { return numberOfUsers[i] % 1234; });
            }

            //Assert
            var status = cache.GetStatus();
            Assert.NotEqual(0, status.TotalMisses);
            Assert.Equal(0, status.TotalHits);

        }

        [Fact]
        public void CreateCacheWithExisting()
        {
            //Arrange
            IOptions<CacheConfiguration> _options = Options.Create(new CacheConfiguration()
            {
                AbsoluteExpiration = 5000,
                MaxEntry = 5
            });
            var cache = new MyOwnCacheHelper<int>(_options);
            int[] numberOfUsers = new int[] { 1001, 1002, 1003, 1004, 1005, 1001,1002 };

            //Act
            for (int i = 0; i < numberOfUsers.Count(); i++)
            {
                cache.GetOrCreate(numberOfUsers[i], () => { return numberOfUsers[i] % 1234; });
            }

            //Assert
            var status = cache.GetStatus();
            Assert.True(status.TotalMisses == 5);
            Assert.True(status.TotalHits == 2);
            Assert.Equal(5, status.CurrentEntryCount);
        }

        [Fact]
        public void AbsoluteExpiration_ShouldOk()
        {
            //Arrange
            IOptions<CacheConfiguration> _options = Options.Create(new CacheConfiguration()
            {
                AbsoluteExpiration = 1000,
                MaxEntry = 10
            });
            var cache = new MyOwnCacheHelper<int>(_options);
            int[] numberOfUsers = new int[] { 1001, 1002, 1003, 1004, 1005, 1006, 1007 };

            //Act
            for (int i = 0; i < numberOfUsers.Count(); i++)
            {
                cache.GetOrCreate(numberOfUsers[i], () => { return numberOfUsers[i] % 1234; });
                if (i==4)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(4000));
                }
            }

            //Assert
            var status = cache.GetStatus();
            Assert.True(status.CurrentEntryCount < 5);
        }

    }
}
