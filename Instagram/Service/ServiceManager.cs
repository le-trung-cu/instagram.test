using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Service.Contracts;

namespace Service;
public class ServiceManager : IServiceManager
{
    private readonly Lazy<IPostService> _postService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IFollowService> _followService;
    private readonly Lazy<ILikeService> _likeService;
    private readonly Lazy<ICommentService> _commentService;
    private readonly IFileService _fileService;
    private readonly Lazy<IStoryService> _storyService;
    private readonly Lazy<IUserService> _userService;

    public ServiceManager(
        IRepositoryManager repositoryManger, 
        ILoggerManager logger,
        IMapper mapper,
        UserManager<User> userManager,
        IDistributedCache distributedCache,
        IConfiguration configuration,
        IAuthorizationService authorization,
        IFileService fileService,
        INotificationService notifiCation)
    {
        _postService = new Lazy<IPostService>(() =>
        new PostService(repositoryManger, mapper, userManager, authorization, fileService, logger, StoryService));

        _authenticationService = new Lazy<IAuthenticationService>(() => 
        new AuthenticationService(logger, mapper, userManager, configuration));

        _followService = new Lazy<IFollowService>(() =>
        new FollowService(repositoryManger, userManager, notifiCation));


        _likeService = new Lazy<ILikeService>(() => new LikeService(repositoryManger, userManager, notifiCation));

        _commentService = new Lazy<ICommentService>(() => new CommentService(userManager, authorization, repositoryManger, mapper));
        _fileService = fileService;

        _storyService = new Lazy<IStoryService>(() => new StoryService(distributedCache));
        _userService = new Lazy<IUserService>(() => new UserService(userManager, mapper));
    }

    public IPostService PostService => _postService.Value;

    public IAuthenticationService AuthenticationService => _authenticationService.Value;


    public IFollowService FollowService => _followService.Value;

    public ILikeService LikeService => _likeService.Value;

    public ICommentService CommentService => _commentService.Value;

    public IFileService FileService => _fileService;

    public IStoryService StoryService => _storyService.Value;

    public IUserService UserService => _userService.Value;
}

