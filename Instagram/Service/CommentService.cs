using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Security.Claims;

namespace Service
{
    public class CommentService : ICommentService
    {
        private readonly UserManager<User> _usermanager;
        private readonly IAuthorizationService _authService;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CommentService(UserManager<User> usermanager,
            IAuthorizationService authService,
            IRepositoryManager repository,
            IMapper mapper)
        {
            _usermanager = usermanager;
            _authService = authService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CommentDto> CreateCommentAsync(ClaimsPrincipal user, CommentForCreationDto commentForCreation)
        {
            var comment = _mapper.Map<Comment>(commentForCreation);
            comment.OwnerId = _usermanager.GetUserId(user);
            _repository.Comment.CreateComment(comment);
            await _repository.SaveAsync();
            var commentDto = _mapper.Map<CommentDto>(comment);
            return commentDto;
        }


        public async Task DeleteCommentAsync(ClaimsPrincipal user, int commentId)
        {
            var comment = await _repository.Comment.GetCommentAsync(commentId);

            if (comment == null)
                throw new CommentNotFoundException(commentId);

            var userId = _usermanager.GetUserId(user);
            var authorizationResult = await _authService.AuthorizeAsync(user, comment, "CanDeleteComment");
            if (!authorizationResult.Succeeded)
            {
                throw new CommentDeleteForbiddenException(userId, commentId);
            }
            _repository.Comment.DeleteComment(comment);
            await _repository.SaveAsync();
        }

        public async Task<CommentDto> GetCommentAsync(int commentId)
        {
            var comment = await _repository.Comment.GetCommentAsync(commentId);

            if (comment == null)
                throw new CommentNotFoundException(commentId);
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<(IEnumerable<CommentDto> comments, MetaData metaData)> GetCommentsForPostAsync(ClaimsPrincipal user, int postId, CommentParameters commentParameters)
        {
            if(commentParameters == null)
                throw new NullParametersBadRequestException(nameof(commentParameters));

            var userId = _usermanager.GetUserId(user);

            var post = await _repository.Post.GetPostAsync(postId);
            if(post == null)
                throw new PostNotFoundException(postId);
            var comments = await _repository.Comment.GetCommentsOfPostAsync(postId, commentParameters);
            var commentIds = comments.Select(c => c.Id);
            var commentLikedIds = await _repository.Like.FilterLikedCommentIdsByUser(userId, commentIds);
            
            var commentDtos = _mapper.Map<IEnumerable<CommentDto>>(comments);
            foreach (var comment in commentDtos)
            {
                comment.Liked = commentLikedIds.Contains(comment.Id);
            }

            return (commentDtos, comments.MetaData);
        }
    }
}
