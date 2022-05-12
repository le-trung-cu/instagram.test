namespace Entities.Exceptions
{
    public class PostDeleteForbiddenException : ForbiddenException
    {
        public PostDeleteForbiddenException(string userId, int postId)
            : base($"User with id: {userId} can't deleted a post has id: {postId}")
        {
        }
    }
}
