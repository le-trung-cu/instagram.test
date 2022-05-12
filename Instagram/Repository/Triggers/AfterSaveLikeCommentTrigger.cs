using Contracts;
using Entities.Models;
using EntityFrameworkCore.Triggered;

namespace Repository.Triggers;

public class AfterSaveLikeCommentTrigger : IAfterSaveTrigger<LikeComment>
{
    private IRepositoryManager _repository;

    public AfterSaveLikeCommentTrigger(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task AfterSave(ITriggerContext<LikeComment> context, CancellationToken cancellationToken)
    {
        var like = context.Entity;
        if(context.ChangeType == ChangeType.Added)
        {
            await _repository.ActivityCount.IncreaseLikeCountOfCommentAsync(like.CommentId);
        }
        if(context.ChangeType == ChangeType.Deleted)
        {
            await _repository.ActivityCount.DescreaseLikeCoutOfCommentAsync(like.CommentId);
        }
    }
}
