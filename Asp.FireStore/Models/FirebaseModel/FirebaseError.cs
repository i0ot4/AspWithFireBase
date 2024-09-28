namespace Asp.FireStore.Models.FirebaseModel
{
    public class FirebaseError
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<Error> errors { get; set; }
    }
}
