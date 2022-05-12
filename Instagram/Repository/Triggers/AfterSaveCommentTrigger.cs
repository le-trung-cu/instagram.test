using Contracts;
using Entities.Models;
using EntityFrameworkCore.Triggered;

namespace Repository.Triggers;

public class AfterSaveCommentTrigger : IAfterSaveTrigger<Comment>
{
    private IRepositoryManager _repositoryManager;
    private RepositoryContext _context;

    public AfterSaveCommentTrigger(IRepositoryManager repositoryManager, RepositoryContext context)
    {
        _repositoryManager = repositoryManager;
        _context = context;
    }

    public async Task AfterSave(ITriggerContext<Comment> context, CancellationToken cancellationToken)
    {
        var comment = context.Entity;

        var tasks = new List<Task>();
        if (context.ChangeType == ChangeType.Added)
        {
            // Increate Comment Count The Post
            await _repositoryManager.ActivityCount.IncrementCommentCountOfPostAsync(comment.PostId);
            await _context.Entry(comment).Reference(t => t.Parent).LoadAsync();
            await _repositoryManager.PostCache.IncrementCommentsAsync(comment.PostId);
            comment = comment.Parent;
            while(comment != null)
            {
                await _repositoryManager.ActivityCount.IncrementCommentCountOfCommentAsync(comment.Id);
                await _context.Entry(comment).Reference(t => t.Parent).LoadAsync();
                comment = comment.Parent;
            }
        }
        else if (context.ChangeType == ChangeType.Deleted)
        {
            await _repositoryManager.ActivityCount.DescrementCommentCoutOfPost(comment.PostId);
            await _repositoryManager.PostCache.DecremenCommentsAsync(comment.PostId);
            if (comment.ParentId.HasValue)
            {
                await _repositoryManager.ActivityCount.DescrementCommentCoutOfCommentAsync(comment.ParentId.Value);
            }
        }
    }
}
