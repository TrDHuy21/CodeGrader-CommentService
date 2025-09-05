using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implement
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(CMContext cMContext) : base(cMContext)
        {
        }
        public override async Task<Comment> GetById(int id)
        {
            return await _cMContext.Comment
                .Include(c => c.Replies)
                .Include(c => c.ParentComment)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async override Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _cMContext.Comment
                .Include(c => c.Replies)
                .Include(c => c.ParentComment)
                .Where(c => c.ParentCommentId == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentByProblemId(int problemId)
        {
            return await _cMContext.Comment
                .Include(c => c.Replies)
                .Include(c => c.ParentComment)
                .Where(c => c.ProblemId == problemId && c.ParentCommentId == null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> SortCommentByLike(int problemId)
        {
            return await _cMContext.Comment
                .Include(c => c.Replies)
                .Include(c => c.ParentComment)
                .Where(c => c.ProblemId == problemId && c.ParentCommentId == null)
                .OrderByDescending(c => c.Like)
                .ThenByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> SortCommentByCreatedAt(int problemId)
        {
            return await _cMContext.Comment
                .Include(c => c.Replies)
                .Include(c => c.ParentComment)
                .Where(c => c.ProblemId == problemId && c.ParentCommentId == null)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
