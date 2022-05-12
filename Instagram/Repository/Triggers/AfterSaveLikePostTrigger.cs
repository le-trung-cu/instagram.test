using Contracts;
using Entities.Models;
using EntityFrameworkCore.Triggered;

namespace Repository.Triggers;

public class AfterSaveLikePostTrigger : IAfterSaveTrigger<LikePost>
{
    private IRepositoryManager _repository;

    public AfterSaveLikePostTrigger(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task AfterSave(ITriggerContext<LikePost> context, CancellationToken cancellationToken)
    {
        var like = context.Entity;
        if(context.ChangeType == ChangeType.Added)
        {
            await _repository.ActivityCount.IncreaseLikeCountOfPostAsync(like.PostId);
            await _repository.PostCache.IncrementLikesAsync(like.PostId);
        }
        if(context.ChangeType == ChangeType.Deleted)
        {
            await _repository.ActivityCount.DescreaseLikeCoutOfPostAsync(like.PostId);
            await _repository.PostCache.DecrementLikesAsync(like.PostId);
        }
    }
}

