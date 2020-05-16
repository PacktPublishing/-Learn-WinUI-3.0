using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MyMediaCollection.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace MyMediaCollection.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IDictionary<string, Type> _pages = new ConcurrentDictionary<string, Type>();

        public const string RootPage = "(Root)";

        public const string UnknownPage = "(Unknown)";

        private static Frame AppFrame => (Frame)Window.Current.Content;

        public void Configure(string page, Type type)
        {
            if (_pages.ContainsKey(page))
                throw new ArgumentException($"The {page} page is already registered.");

            if (_pages.Values.Any(v => v == type))
                throw new ArgumentException($"The {type.Name} view has already been registered under another name.");

            _pages.Add(page, type);
        }

        /// <summary>
        /// Gets the name of the currently displayed page.
        /// </summary>
        public string CurrentPage
        {
            get
            {
                var frame = AppFrame;

                if (frame.BackStackDepth == 0)
                    return RootPage;

                if (frame.Content == null)
                    return UnknownPage;

                var type = frame.Content.GetType();

                if (_pages.Values.All(v => v != type))
                    return UnknownPage;

                var item = _pages.Single(i => i.Value == type);

                return item.Key;
            }
        }

        public void NavigateTo(string page)
        {
            NavigateTo(page, null);
        }

        public void NavigateTo(string page, object parameter)
        {
            if (!_pages.ContainsKey(page))
                throw new ArgumentException($"Unable to find a page registered with the name {page}.");

            if (AppFrame != null)
                AppFrame.Navigate(_pages[page], parameter);
        }

        public void GoBack()
        {
            if (AppFrame?.CanGoBack == true)
                AppFrame.GoBack();
        }
    }
}