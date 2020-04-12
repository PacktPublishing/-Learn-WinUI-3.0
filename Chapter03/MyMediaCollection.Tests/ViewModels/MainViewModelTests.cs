using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMediaCollection.ViewModels;

namespace MyMediaCollection.Tests.ViewModels
{
    [TestClass]
    public class MainViewModelTests
    {
        [TestMethod]
        public void PopulateData_CreatesThreeMediaItems()
        {
            // Arrange and Act
            var vm = new MainViewModel();

            // Assert
            Assert.IsNotNull(vm.Items, "Items collection is null.");
            Assert.AreEqual(3, vm.Items.Count, "Items count does not match.");
        }

        [TestMethod]
        public void PopulateData_CreatesFourMediums()
        {
            // Arrange and Act
            var vm = new MainViewModel();

            // Assert
            Assert.IsNotNull(vm.Mediums, "Mediums collection is null.");
            Assert.AreEqual(4, vm.Mediums.Count, "Mediums count does not match.");
        }

        [TestMethod]
        public void SetSelectedMediaItem_EnablesDeleteCommand()
        {
            // Arrange
            var vm = new MainViewModel();

            // Act
            vm.SelectedMediaItem = vm.Items[1];

            // Assert
            Assert.IsTrue(vm.DeleteCommand.CanExecute(null));
        }

        [TestMethod]
        public void DeleteItem_DeletesSelectedItem()
        {
            // Arrange
            var vm = new MainViewModel();

            // Act
            vm.SelectedMediaItem = vm.Items[1];
            vm.DeleteCommand.Execute(null);

            // Assert
            Assert.AreEqual(2, vm.Items);
        }

        [TestMethod]
        public void AddEditItem_AddsItems()
        {
            // Arrange
            var vm = new MainViewModel();

            // Act
            vm.AddEditCommand.Execute(null);
            vm.AddEditCommand.Execute(null);

            // Assert
            Assert.AreEqual(5, vm.Items);
        }
    }
}