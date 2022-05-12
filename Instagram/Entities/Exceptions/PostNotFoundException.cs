namespace Entities.Exceptions;
public sealed class PostNotFoundException : NotFoundException
{
    public PostNotFoundException(int postId) 
        : base($"The Post with id: {postId} doesn't exist in the database.")
    {
    }
}

