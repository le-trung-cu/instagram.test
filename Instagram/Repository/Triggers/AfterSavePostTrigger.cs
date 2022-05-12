using Contracts;
using Entities.Models;
using EntityFrameworkCore.Triggered;

namespace Repository.Triggers
{
    public class AfterSavePostTrigger : IAfterSaveTrigger<Post>
    {
        private readonly IRepositoryManager _repository;

        public AfterSavePostTrigger(IRepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task AfterSave(ITriggerContext<Post> context, CancellationToken cancellationToken)
        {
            if(context.ChangeType == ChangeType.Added)
            {
                context.Entity.Slug = new Shared.Base53(context.Entity.Id).ToString();
                await _repository.SaveAsync();
                await _repository.ActivityUserData.IncreasePostCount(context.Entity.OwnerId);
            }
        }
    }
}
