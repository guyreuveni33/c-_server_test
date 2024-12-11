using WebApplication10.Models;

namespace WebApplication10.Services;

public interface IPostService
{
    Task<IEnumerable<Post>> GetAllPostAsync();
    Task<Post> GetPostByIdAsync(int id);
    Task<IEnumerable<Post>> FilterPostsByUserIdAsync(int userId);
    Task CreatePostAsync(Post post);
    Task InsertFetchedPostsToMongoAsync();
    Task DeletePostAsync(int postId);
    Task<bool> UpdatePostTitleAsync(int postId, string title);
}