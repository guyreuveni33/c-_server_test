using WebApplication10.Models;
using WebApplication10.Repositories;

namespace WebApplication10.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _repository;

    public PostService(IPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Post>> GetAllPostAsync()
    {
        return await _repository.GetAllPostAsync();
    }

    public async Task<Post> GetPostByIdAsync(int id)
    {
        var posts = await _repository.GetAllPostAsync();
        foreach (var post in posts)
        {
            if (post.Id == id)
            {
                return post;
            }
        }

        return null;
    }

    public async Task<IEnumerable<Post>> FilterPostsByUserIdAsync(int userId)
    {
        var posts = await _repository.GetAllPostAsync();
        var filteredPosts = new List<Post>();
        foreach (var post in posts)
        {
            if (post.Id == userId)
            {
                filteredPosts.Add(post);
            }
        }

        return filteredPosts;
    }

    public async Task CreatePostAsync(Post post)
    {
        await _repository.CreatePostAsync(post);
    }

    public async Task InsertFetchedPostsToMongoAsync()
    {
        await _repository.InsertFetchedPostsToMongoAsync();
    }

    public async Task DeletePostAsync(int postId)
    {
        await _repository.DeletePostAsync(postId);
    }

    public async Task<bool> UpdatePostTitleAsync(int postId, string title)
    {
        try
        {
            await _repository.UpdatePostTitleAsync(postId, title);
            return true; // Return true if the update is successful
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in repository: {ex.Message}");
            return false; // Return false if the update fails (e.g., post not found)
        }    }
}