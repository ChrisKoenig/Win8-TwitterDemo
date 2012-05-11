using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Application5.Messages;
using Application5.Model;
using GalaSoft.MvvmLight.Messaging;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Application5
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage : Page
    {
        public BlankPage()
        {
            this.InitializeComponent();
            try
            {
                DataTransferManager.GetForCurrentView().DataRequested += BlankPage_DataRequested;
            }
            catch { }
        }

        private void BlankPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var currentTweet = TweetsGridView.SelectedItem as Tweet;
            if (currentTweet == null)
            {
                args.Request.FailWithDisplayText("No item selected");
            }
            else
            {
                args.Request.Data.Properties.Title = "Tweet from " + currentTweet.Poster;
                args.Request.Data.Properties.Description = currentTweet.Text;
                args.Request.Data.SetUri(new Uri(currentTweet.Link));
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var searchTerm = "#win8dev";

            if (e.Parameter != null && e.Parameter.ToString().Length > 0)
                searchTerm = e.Parameter.ToString();

            Messenger.Default.Send<SearchMessage>(new SearchMessage(searchTerm));
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            // do nothing for now...
        }
    }
}