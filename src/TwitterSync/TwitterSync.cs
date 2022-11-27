using Newtonsoft.Json;
using OAuth;
using RestSharp;
using TwitterSync.Models;

namespace TwitterSync
{
    public class TwitterSync
    {
        private const int MAX_LENGTH_CONTENT_TWEET = 250;
        private const string TWITER_API_TWEETS = "/tweets";
        private const string TWITTER_API = "https://api.twitter.com/2";
        private string accessToken;

        private string accessTokenSecret;

        private string consumerKey;

        private string consumerToken;

        public TwitterSync() : this(
            Environment.GetEnvironmentVariable("CONSUMER_KEY"),
            Environment.GetEnvironmentVariable("CONSUMER_TOKEN"),
            Environment.GetEnvironmentVariable("ACCESS_TOKEN"),
            Environment.GetEnvironmentVariable("ACCESS_TOKEN_SECRET"))
        {
        }

        public TwitterSync(string? consumerKey,
            string? consumerToken,
            string? accessToken,
            string? accessTokenSecret)
        {
            ArgumentNullException.ThrowIfNull(consumerKey);
            ArgumentNullException.ThrowIfNull(consumerToken);
            ArgumentNullException.ThrowIfNull(accessToken);
            ArgumentNullException.ThrowIfNull(accessTokenSecret);

            this.consumerKey = consumerKey;
            this.consumerToken = consumerToken;
            this.accessToken = accessToken;
            this.accessTokenSecret = accessTokenSecret;
        }
        public TweetCreateResponse CreateTweet(string content, string? idReplyTweet = null)
        {
            ArgumentNullException.ThrowIfNull(content);

            var client = new RestClient(TWITTER_API);

            OAuthRequest oAuthRequest = OAuthRequest.ForProtectedResource("POST", consumerKey, consumerToken, accessToken, accessTokenSecret);
            oAuthRequest.RequestUrl = TWITTER_API + TWITER_API_TWEETS;

            // add HTTP header authorization
            string auth = oAuthRequest.GetAuthorizationHeader();

            var request = new RestRequest(oAuthRequest.RequestUrl);
            request.AddHeader("Authorization", auth);
            request.Method = Method.POST;

            request.AddHeader("Content-Type", "application/json; charset=utf-8");

            TweetCreateRequest tweetCreateRequest = new TweetCreateRequest() { text = TrimLength(content) };
            if (idReplyTweet != null)
                tweetCreateRequest.ReplyToTweet = new TweetCreateRequest.Reply() { in_reply_to_tweet_id = idReplyTweet };

            request.AddParameter("application/json", JsonConvert.SerializeObject(tweetCreateRequest), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
                throw new Exception($"Error creating tweet {response.StatusCode} {response.Content}");
            return JsonConvert.DeserializeObject<TweetCreateResponse>(response.Content);
        }

        private string TrimLength(string content)
        {
            if (content.Length > MAX_LENGTH_CONTENT_TWEET)
            {
                content = content.Substring(0, MAX_LENGTH_CONTENT_TWEET - 5);
                content += "...";
            }
            return content;
        }
    }
}