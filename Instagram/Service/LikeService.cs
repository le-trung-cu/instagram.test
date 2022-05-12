using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects;
using System.Security.Claims;

namespace Service
{
    public class LikeService : ILikeService
    {
        private readonly IRepositoryManager _repository;
        private readonly UserManager<User> _userManager;
        private readonly INotificationService _notificationService;

        public LikeService(IRepositoryManager repository, UserManager<User> userManager, INotificationService notificationService)
        {
            _repository = repository;
            _userManager = userManager;
            _notificationService = notificationService;
        }     

        public async Task LikePostAsync(ClaimsPrincipal user, int postId)
        {
            var userId = _userManager.GetUserId(user);
            var userName = _userManager.GetUserName(user);
            var post = await _repository.Post.GetPostAsync(postId);
            if(post == null)
            {
                throw new PostNotFoundException(postId);
            }
            var like = new LikePost { OwnerId = userId, PostId = postId };
            _repository.Like.CreateLikePost(like);
            await _repository.SaveAsync();

            var notification = new NotificationDto
            {
                CreatedAt = DateTimeOffset.Now,
                Message = $"{userName} likes your post",
                UserName = userName,
                UserId = userId,
                Type = NotifiCationTypes.UserLikePost,
            };
            await _repository.NotificationRepository.CreateNotifiCation(post.OwnerId, notification);

            _notificationService.SendMessage(post.OwnerId, notification);
        }
        public async Task UnLikePost(ClaimsPrincipal user, int postId)
        {
            var userId = _userManager.GetUserId(user);
            var like = new LikePost { OwnerId = userId, PostId = postId };

            _repository.Like.DeleteLikePost(like);
            await _repository.SaveAsync();
        }

        public async Task LikeCommentAsync(ClaimsPrincipal user, int commentId)
        {
            var userId = _userManager.GetUserId(user);
            var like = new LikeComment { OwnerId = userId, CommentId = commentId };

            _repository.Like.CreateLikeComment(like);
            await _repository.SaveAsync();
        }
        public async Task UnLikeCommentAsync(ClaimsPrincipal user, int commentId)
        {
            var userId = _userManager.GetUserId(user);
            var like = new LikeComment { OwnerId = userId, CommentId = commentId };

            _repository.Like.DeleteLikeComment(like);
            await _repository.SaveAsync();
        }
    }
}
