namespace Service.Contracts;
public interface IServiceManager
{
    IPostService PostService { get; }
    IAuthenticationService AuthenticationService { get; }
    IFollowService FollowService { get; }
    ILikeService LikeService { get; }
    ICommentService CommentService { get; }
    IFileService FileService { get; }
    IStoryService StoryService { get; }
    IUserService UserService { get; }
}

