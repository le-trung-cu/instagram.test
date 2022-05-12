namespace Entities.Exceptions
{
    public class PostTypeUnprocessableException : UnprocessableException
    {
        public PostTypeUnprocessableException() 
            : base("PostType must be one of those: Post, PostPhoto, PostVideo, PostCarousel")
        {
        }
    }
}
