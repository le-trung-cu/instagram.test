namespace Entities.Exceptions
{
    public sealed class CommentDeleteForbiddenException : ForbiddenException
    {
        public CommentDeleteForbiddenException(string userId, int commentId) 
            : base($"User with id: {userId} can't deleted a comment has id: {commentId}")
        {
        }
    }
}
