namespace Entities.Exceptions
{
    public class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(int commentId) 
            : base($"Comment with id: {commentId} doesn't exist in the database")
        {
        }
    }
}
