namespace Cletor.Models
{
    public class RemoteImage
    {
        public RemoteImage() { }
        public RemoteImage(string placeHolder, string url)
        {
            PlaceHolder = placeHolder;
            Url = url;
        }

        public string PlaceHolder { get; set; }
        public string Url { get; set; }
    }
}
