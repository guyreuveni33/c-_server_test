using WebApplication10.Models;

namespace WebApplication10.Repositories;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAllPostAsync();
    Task CreatePostAsync(Post post);
    Task InsertFetchedPostsToMongoAsync();
    Task DeletePostAsync(int postId);

    Task UpdatePostTitleAsync(int postId,string title);
}