using Moq;
using MyMediaCollection.Enums;
using MyMediaCollection.Interfaces;
using MyMediaCollection.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject2
{
    public static class ServiceHelper
    {
        public static Mock<INavigationService> GetMockNavigationService()
        {
            var navService = new Mock<INavigationService>();
            navService.SetupAllProperties();

            return navService;
        }

        public static Mock<IDataService> GetMockDataService()
        {
            var cd = new Medium { Id = 1, MediaType = ItemType.Music, Name = "CD" };
            var vinyl = new Medium { Id = 2, MediaType = ItemType.Music, Name = "Vinyl" };
            var hardcover = new Medium { Id = 3, MediaType = ItemType.Book, Name = "Hardcover" };
            var bluRay = new Medium { Id = 6, MediaType = ItemType.Video, Name = "Blu Ray" };

            var mediums = new List<Medium>
            {
                cd,
                vinyl,
                hardcover,
                bluRay
            };

            var itemTypes = new List<ItemType>
            {
                ItemType.Book,
                ItemType.Music,
                ItemType.Video
            };

            var dataService = new Mock<IDataService>();
            dataService.Setup(s => s.GetMediums()).Returns(mediums);
            dataService.Setup(s => s.GetItems()).Returns(new List<MediaItem>());
            dataService.Setup(s => s.GetItemTypes()).Returns(itemTypes);
            dataService.SetupAllProperties();

            return dataService;
        }
    }
}
