using Contracts;
using StackExchange.Redis;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IPostRepository> _postRepository;
    private readonly Lazy<IFollowRepository> _followRepository;
    private readonly Lazy<ILikeRepository> _likeRepository;
    private readonly Lazy<ICommentRepository> _commentRepository;
    private readonly Lazy<IActivityCountRepository> _activityCountRepository;
    private readonly Lazy<IMediaRepository> _mediaRepository;
    private readonly Lazy<IActivityUserDataCountRepository> _activityUserDataCountRepository;
    private readonly Lazy<IPostCacheRepository> _postCacheRepository;
    private readonly Lazy<IUserTagRepository> _userTagRepository;
    private readonly Lazy<IFeedPostCacheRepository> _feedPostCacheRepository;
    private readonly Lazy<IUserSavePostRepository> _userSavePostRepository;
    private readonly Lazy<INotificationRepository> _notificationRepository;
    public RepositoryManager(RepositoryContext repositoryContext, IConnectionMultiplexer connectionMultiplexer)
    {
        _repositoryContext = repositoryContext;
        _postRepository = new Lazy<IPostRepository>(() => new PostRepository(repositoryContext));
        _followRepository = new Lazy<IFollowRepository>(() => new FollowRepository(repositoryContext));
        _likeRepository = new Lazy<ILikeRepository>(() => new LikeRepository(repositoryContext));
        _commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(repositoryContext));
        _activityCountRepository = new Lazy<IActivityCountRepository>(() => new ActivityCountRepository(repositoryContext));
        _mediaRepository = new Lazy<IMediaRepository>(() => new MediaRepository(repositoryContext));
        _activityUserDataCountRepository = new Lazy<IActivityUserDataCountRepository>(() => new ActivityUserDataCountRepository(repositoryContext));
        _postCacheRepository = new Lazy<IPostCacheRepository>(() => new PostCacheRepository(connectionMultiplexer));
        _userTagRepository = new Lazy<IUserTagRepository>(() => new UserTagRepository(repositoryContext));
        _feedPostCacheRepository = new Lazy<IFeedPostCacheRepository>(() => new FeedPostCacheRepository(connectionMultiplexer));
        _userSavePostRepository = new Lazy<IUserSavePostRepository> (() => new UserSavePostRepository(repositoryContext));
        _notificationRepository = new Lazy<INotificationRepository> (() => new NotificationRepository(connectionMultiplexer));
    }
    public IPostRepository Post => _postRepository.Value;

    public IFollowRepository Follow => _followRepository.Value;

    public ILikeRepository Like => _likeRepository.Value;

    public ICommentRepository Comment => _commentRepository.Value;

    public IActivityCountRepository ActivityCount => _activityCountRepository.Value;

    public IMediaRepository Media => _mediaRepository.Value;

    public IActivityUserDataCountRepository ActivityUserData => _activityUserDataCountRepository.Value;

    public IPostCacheRepository PostCache => _postCacheRepository.Value;

    public IUserTagRepository UserTag => _userTagRepository.Value;

    public IFeedPostCacheRepository FeePostCache => _feedPostCacheRepository.Value;

    public IUserSavePostRepository SavePost => _userSavePostRepository.Value;

    public INotificationRepository NotificationRepository => _notificationRepository.Value;

    public async Task SaveAsync() => await  _repositoryContext.SaveChangesAsync();

}

