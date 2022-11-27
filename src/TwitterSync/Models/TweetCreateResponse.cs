namespace TwitterSync.Models
{
    public class TweetCreateResponse
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}