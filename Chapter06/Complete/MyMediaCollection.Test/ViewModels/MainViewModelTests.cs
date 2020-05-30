using System;
using MyMediaCollection.ViewModels;
using UnitTestProject2;
using UnitTestProject2.Fakes;
using Windows.UI.Core;
using Xunit;

namespace MyMediaCollection.Test.ViewModels
{
    public class MainViewModelTests
    {
        [Fact]
        public async void PopulateData_CreatesThreeMediaItems()
        {
            // Arrange and Act
            MainViewModel vm = null;

            await App.SharedWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                vm = new MainViewModel(ServiceHelper.GetMockNavigationService().Object,
                                       ServiceHelper.GetMockDataService().Object);
            });

            // Assert
            Assert.NotNull(vm.Items);
            Assert.Equal(3, vm.Items.Count);
        }

        [Fact]
        public async void PopulateData_CreatesFourMediums()
        {
            // Arrange and Act
            MainViewModel vm = null;

            await App.SharedWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                vm = new MainViewModel(ServiceHelper.GetMockNavigationService().Object,
                                        ServiceHelper.GetMockDataService().Object);
            });

            // Assert
            Assert.NotNull(vm.Mediums);
            Assert.Equal(4, vm.Mediums.Count);
        }

        [Fact]
        public async void SetSelectedMediaItem_EnablesDeleteCommand()
        {
            // Arrange
            MainViewModel vm = null;

            await App.SharedWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                vm = new MainViewModel(ServiceHelper.GetMockNavigationService().Object,
                                       ServiceHelper.GetMockDataService().Object);

                // Act
                vm.SelectedMediaItem = vm.Items[1];
            });

            // Assert
            Assert.True(vm.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public async void DeleteItem_DeletesSelectedItem()
        {
            // Arrange
            MainViewModel vm = null;

            await App.SharedWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                vm = new MainViewModel(ServiceHelper.GetMockNavigationService().Object,
                                        new FakeDataService());

                // Act
                vm.SelectedMediaItem = vm.Items[1];
                vm.DeleteCommand.Execute(null);
            });

            // Assert
            Assert.Equal(2, vm.Items.Count);
        }

        [Fact]
        public async void AddEditItem_AddsItems()
        {
            // Arrange
            MainViewModel vm = null;

            await App.SharedWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                vm = new MainViewModel(ServiceHelper.GetMockNavigationService().Object,
                                       ServiceHelper.GetMockDataService().Object);

                // Act
                vm.AddEditCommand.Execute(null);
                vm.AddEditCommand.Execute(null);
            });

            // Assert
            Assert.Equal(5, vm.Items.Count);
        }
    }
}