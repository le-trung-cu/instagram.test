namespace Entities.Exceptions
{
    public class UserNotFoundException : NotFoundException
    {
        public UserNotFoundException(string username) 
            : base($"The user with username: {username} doesn't exist in the database.")
        {
        }
    }
}
