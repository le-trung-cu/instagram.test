namespace Entities.Exceptions
{
    public sealed class MediaTypeUnprocessableException : UnprocessableException
    {
        public MediaTypeUnprocessableException()
            : base("Model type of a media is either video or photo")
        {
        }
    }
}
