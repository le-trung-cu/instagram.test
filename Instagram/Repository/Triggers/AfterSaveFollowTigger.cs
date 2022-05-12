using Contracts;
using Entities.Models;
using EntityFrameworkCore.Triggered;

namespace Repository.Triggers
{
    public class AfterSaveFollowTigger : IAfterSaveTrigger<Follow>
    {
        private readonly IRepositoryManager _repository;

        public AfterSaveFollowTigger(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task AfterSave(ITriggerContext<Follow> context, CancellationToken cancellationToken)
        {
            if(context.ChangeType == ChangeType.Added)
            {
                await _repository.ActivityUserData.IncreaseFollowerCount(context.Entity.FolloweeId);
                await _repository.ActivityUserData.IncreaseFollowingCount(context.Entity.FollowerId);
            }
            else if(context.ChangeType == ChangeType.Deleted)
            {
                await _repository.ActivityUserData.DescreaseFollowerCount(context.Entity.FolloweeId);
                await _repository.ActivityUserData.DescreaseFollowingCount(context.Entity.FollowerId);
            }
        }
    }
}
