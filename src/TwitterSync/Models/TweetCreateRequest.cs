using Newtonsoft.Json;

namespace TwitterSync.Models
{
    public class TweetCreateRequest
    {
        [JsonProperty("reply", NullValueHandling = NullValueHandling.Ignore)]
        public Reply ReplyToTweet { get; set; }

        public string text { get; set; }

        public class Reply
        {
            public string in_reply_to_tweet_id { get; set; }
        }
    }
}