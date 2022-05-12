using Contracts;
using Dapper;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Repository
{
    public class FollowRepository : RepositoryBase<Follow>, IFollowRepository
    {
        public FollowRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<PagedList<UserFollowDto>> GetFolloweesForUser(string userId, PageParameters parameters)
        {
            return await PagedList<UserFollowDto>.ToPagedListAsync(
                FindByCondition(u => u.FollowerId == userId)
                .Include(f => f.Followee)
                .Select(f => new UserFollowDto {
                    Id = f.FolloweeId, 
                    UserName = f.Followee!.UserName, 
                    Avatar = f.Followee.Avatar,
                    FullName = f.Followee.FullName,
                }),
                parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PagedList<UserFollowDto>> GetFollowersForUser(string userId, PageParameters parameters)
        {
            return await PagedList<UserFollowDto>.ToPagedListAsync(
                FindByCondition(f => f.FolloweeId == userId)
                .Include(f => f.Follower)
                .Select(f => new UserFollowDto
                {
                    Id = f.FollowerId,
                    UserName = f.Follower!.UserName,
                    Avatar = f.Follower.Avatar,
                    FullName = f.Follower.FullName,
                }),
                parameters.PageNumber,
                parameters.PageSize);
        }


        public async Task<IEnumerable<UserFollowDto>> GetFollowersForUser(string userId)
        {
            return await FindByCondition(f => f.FolloweeId == userId)
                .Include(f => f.Follower)
                .Select(f => new UserFollowDto
                {
                    Id = f.FollowerId,
                    UserName = f.Follower!.UserName,
                    Avatar = f.Follower.Avatar,
                    FullName = f.Follower.FullName,
                }).ToListAsync();
        }

        public async Task<IEnumerable<SuggestionFollowDto>> GetSugestionFollowForUser(string userId)
        {
            var query = @"WITH SuggestionFriends AS (
                        SELECT f2.FollowerId, f2.FolloweeId,
                        Count(*) OVER(PARTITION BY f2.FolloweeId ORDER BY f2.FolloweeId) as friends,
                        ROW_NUMBER() OVER(PARTITION BY f2.FolloweeId ORDER BY f2.FolloweeId) as nth
                        From (SELECT * FROM Follows as f1 WHERE f1.FollowerId = @userId) as user1
                        INNER JOIN Follows as f2
                        On user1.FolloweeId = f2.FollowerId AND f2.FolloweeId <> @userId
                        WHERE f2.FolloweeId NOT IN(SELECT FolloweeId FROM follows WHERE FollowerId = @userId) )
                        Select FollowerId as FriendId, FolloweeId as SuggestionId, u1.UserName as FriendName, u2.UserName as SuggestionName, Friends, u2.Avatar
                        From SuggestionFriends
                        INNER JOIN AspNetUsers as u1
                        ON u1.Id = FollowerId
                        INNER JOIN AspNetUsers as u2
                        ON u2.Id = FolloweeId
                        WHERE nth < 3";
            var follows = await RepositoryContext.Connection.QueryAsync<SuggestionFriend>(query, new { userId = userId });

            var f = new Dictionary<string, SuggestionFollowDto>();
            if (follows != null)
                foreach (var follow in follows)
                {
                    if (f.TryGetValue(follow.SuggestionId!, out SuggestionFollowDto? suggestion))
                    {
                        suggestion.FriendNames.Add(follow.FriendName);
                    }
                    else
                    {
                        f[follow.SuggestionId] = new SuggestionFollowDto
                        {
                            Friends = follow.Friends,
                            UserName = follow.SuggestionName,
                            Id = follow.SuggestionId,
                            Avatar = follow.Avatar,
                            FriendNames = new List<string>() { follow.FriendName }
                        };
                    }
                }
            return f.Select(t => t.Value);
        }

        public async Task<Follow?> GetFollowAsync(string followerId, string followingId)
        {
            return await FindByCondition(t => t.FollowerId == followerId && t.FolloweeId == followingId)
                .FirstOrDefaultAsync();
        }

        public void AddFollow(Follow follow)
        {
            follow.FollowDate = DateTime.Now;
            Create(follow);
        }

        public void RemoveFolloweeForUser(Follow follow)
        {
            Delete(follow);
        }

        public async Task<IEnumerable<string>> FilterFollowingByUser(string userId, IEnumerable<string> followingIds)
        {
            return await FindByCondition(t => t.FollowerId == userId && followingIds.Contains(t.FolloweeId))
                .Select(t => t.FolloweeId)
                .ToListAsync();
        }

    }
}
public class SuggestionFriend
{
    public string? FriendId { get; set; }
    public string? FriendName { get; set; }
    public string? SuggestionId { get; set; }
    public string? SuggestionName { get; set; }
    public string? Avatar { get; set; }
    public int Friends { get; set; }
}

