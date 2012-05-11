using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application5.Messages;
using Application5.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Windows.Data.Json;

namespace Application5.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<Tweet> Tweets { get; set; }

        [PreferredConstructor]
        public MainViewModel()
        {
            Tweets = new ObservableCollection<Tweet>();
            if (IsInDesignMode)
            {
                LoadMockTweets();
            }
            else
            {
                //LoadTweets();
            }
            MessengerInstance.Register<SearchMessage>(this, (message) =>
            {
                SearchString = message.Criteria;
                LoadTweets();
            });
            LoadTweetsCommand = new RelayCommand(() => LoadTweets());
        }

        private string _searchString = "#win8dev";

        public string SearchString
        {
            get { return _searchString; }
            private set
            {
                _searchString = value;
                RaisePropertyChanged("SearchString");
            }
        }

        public bool IsNotBusy
        {
            get { return !IsBusy; }
        }

        private bool _isBusy = true;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy = value)
                    return;
                _isBusy = value;
                RaisePropertyChanged("IsBusy");
                RaisePropertyChanged("IsNotBusy");
            }
        }

        private void LoadMockTweets()
        {
            SearchString = "#DesignTime";
            for (int i = 0; i < 10; i++)
            {
                var tweet = new Tweet()
                {
                    Image = "http://a0.twimg.com/profile_images/1754832391/Photo_square_normal.jpeg",
                    Poster = "fakechriskoenig",
                    Text = "Tweet number " + i,
                };
                Tweets.Add(tweet);
            }
        }

        public RelayCommand LoadTweetsCommand { get; set; }

        private static readonly string URL_TEMPLATE = "https://twitter.com/#!/{0}/status/{1}";
        private static readonly string SEARCH_URL = "http://search.twitter.com/search.json?q={0}&rpp={1}";

        private async void LoadTweets(int MaxItems = 99)
        {
            // set the progress indicator
            IsBusy = true;

            // format the search URL
            var SearchTerm = Uri.EscapeDataString(SearchString);
            var FormattedSearchString = String.Format(SEARCH_URL, SearchTerm, MaxItems);

            // fetch the data from twitter
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = int.MaxValue;
            var response = await client.GetStringAsync(FormattedSearchString);

            // parse the response
            var json = JsonObject.Parse(response);
            var results = json.GetNamedArray("results");
            var tweets = from t in results
                         select new Tweet()
                         {
                             Image = t.GetObject().GetNamedString("profile_image_url"),
                             Poster = t.GetObject().GetNamedString("from_user"),
                             Text = t.GetObject().GetNamedString("text"),
                             Link = String.Format(
                                URL_TEMPLATE,
                                t.GetObject().GetNamedString("from_user"),
                                t.GetObject().GetNamedString("id_str")),
                         };

            // Display the results
            Tweets.Clear();
            foreach (var tweet in tweets)
            {
                Tweets.Add(tweet);
            }

            // reset the progress indiator
            IsBusy = false;
        }
    }
}