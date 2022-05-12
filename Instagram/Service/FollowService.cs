using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Entities.Exceptions;
using Service.Contracts;
using Shared.RequestFeatures;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.SignalR;

namespace Service;

public class FollowService : IFollowService
{
    private readonly IRepositoryManager _repository;
    private readonly UserManager<User> _userManager;
    private readonly INotificationService _notifiCation;

    public FollowService(
        IRepositoryManager repository,
        UserManager<User> userManager,
        INotificationService notifiCation)
    {
        _repository = repository;
        _userManager = userManager;
        _notifiCation = notifiCation;
    }

    public async Task CreateFollowAsync(ClaimsPrincipal follower, string username)
    {
        var followee = await _userManager.FindByNameAsync(username);
        if (followee == null)
            throw new UserNotFoundException(username);

        var followeeId = followee.Id;
        var followerId = _userManager.GetUserId(follower);
        var followerName = _userManager.GetUserName(follower);

        var follow = new Follow
        {
            FollowerId = followerId,
            FolloweeId = followeeId,
            FollowDate = DateTime.UtcNow
        };

        _repository.Follow.AddFollow(follow);
        await _repository.SaveAsync();

        await updateFeedPostCollectionForFollower();

        async Task updateFeedPostCollectionForFollower()
        {
            var feedPosts = (await _repository.Post.GetLastestPostOfUserAsync(followeeId))
                .Select(p => new FeedPost(followeeId, p.Id))
                .ToArray();
            var postCount = feedPosts.Length;

            await _repository.FeePostCache.CreateRangeFeedPostsAsync(followerId, feedPosts);
        }
    }

    public async Task DeleteFollowAsync(ClaimsPrincipal follower, string username)
    {
        var followerId = _userManager.GetUserId(follower);

        var followee = await _userManager.FindByNameAsync(username);
        if (followee == null)
            throw new UserNotFoundException(username);
        var followeeId = followee.Id;
        var follow = new Follow
        {
            FollowerId = followerId,
            FolloweeId = followeeId,
        };
        _repository.Follow.RemoveFolloweeForUser(follow);

        await _repository.SaveAsync();

        await _repository.FeePostCache.RemoveUserFeedPostsOfOwner(followerId, followeeId);
    }

    public async Task<(IEnumerable<UserFollowDto> items, MetaData meta)> GetFollowers(ClaimsPrincipal currentUser, string userName, PageParameters parameters)
    {
        var currentUserId = _userManager.GetUserId(currentUser);
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException(userName);
        }
        var userFollows = await _repository.Follow.GetFollowersForUser(user.Id, parameters);
        var followingIds = await _repository.Follow.FilterFollowingByUser(currentUserId, userFollows.Select(x => x.Id!));
        foreach (var userFollow in userFollows)
        {
            userFollow.Status = followingIds.Contains(userFollow.Id) ? "following" : "unFollowing";
        }
        return (userFollows, userFollows.MetaData);
    }

    public async Task<(IEnumerable<UserFollowDto> items, MetaData meta)> GetFollowing(ClaimsPrincipal currentUser, string userName, PageParameters parameters)
    {
        var currentUserId = _userManager.GetUserId(currentUser);
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException(userName);
        }

        var following = await _repository.Follow.GetFolloweesForUser(user.Id, parameters);
        var followingByCurrentUser = await _repository.Follow.FilterFollowingByUser(currentUserId, following.Select(x => x.Id!));
        foreach (var userFollow in following)
        {
            userFollow.Status = followingByCurrentUser.Contains(userFollow.Id) ? "following" : "unFollowing";
        }
        return (following, following.MetaData);
    }

    public async Task<(IEnumerable<SuggestionFollowDto> items, MetaData meta)> GetSugestionFollowForUser(ClaimsPrincipal user, PageParameters parameters)
    {
        var userId = _userManager.GetUserId(user);
        var users = await _repository.Follow.GetSugestionFollowForUser(userId);
        var count = users.Count();
        users = users.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);
        var pages = new PagedList<SuggestionFollowDto>(users, count, parameters.PageNumber, parameters.PageSize);
        return (pages, pages.MetaData);
    }
}