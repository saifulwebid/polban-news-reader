using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace PolbanNewsReader
{
    public class FeedDataSource
    {
        private ObservableCollection<FeedItem> _Feeds = new ObservableCollection<FeedItem>();
        public ObservableCollection<FeedItem> Feeds
        {
            get { return _Feeds; }
        }

        public async Task GetFeedsAsync()
        {
            SyndicationClient client = new SyndicationClient();
            Uri feedUri = new Uri("http://www.polban.ac.id/index.php?format=feed&type=rss");

            try
            {
                SyndicationFeed feed = await client.RetrieveFeedAsync(feedUri);

                if (feed.Items != null && feed.Items.Count > 0)
                {
                    foreach (SyndicationItem item in feed.Items)
                    {
                        FeedItem feedItem = new FeedItem();

                        if (item.Title != null && item.Title.Text != null) {
                            feedItem.Title = item.Title.Text;
                        }

                        if (item.Authors != null && item.Authors.Count > 0) {
                            feedItem.Author = item.Authors[0].Name.ToString();
                        }

                        if (item.Summary != null && item.Summary.Text != null) {
                            feedItem.Content = item.Summary.Text;
                        }

                        if (item.PublishedDate != null) {
                            feedItem.PubDate = item.PublishedDate.DateTime;
                        }

                        if (item.Links != null && item.Links.Count > 0) {
                            feedItem.Link = item.Links[0].Uri;
                        }

                        _Feeds.Add(feedItem);
                    }
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        public static FeedItem GetItem(string title)
        {
            var _feedDataSource = App.Current.Resources["feedDataSource"] as FeedDataSource;
            var matches = _feedDataSource.Feeds.Where((item) => item.Title.Equals(title));

            if (matches.Count() > 1) return matches.First();
            return null;
        }
    }
}
