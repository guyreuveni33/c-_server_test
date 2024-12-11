using System.Collections;
using WebApplication10.Models;
using System.Net.Http.Json;
using MongoDB.Driver;

namespace WebApplication10.Repositories;

public class PostRepositories : IPostRepository

{
    private readonly HttpClient _httpclient;
    private readonly IMongoCollection<Post> _postsCollection;

    public PostRepositories(HttpClient httpClient, IMongoClient mongoClient)
    {
        _httpclient = httpClient;
        var database = mongoClient.GetDatabase("BlogDatabase"); // Database name
        _postsCollection = database.GetCollection<Post>("Posts"); // Collection name
    }

    public async Task<IEnumerable<Post>> GetAllPostAsync()
    {
        var response = await _httpclient.GetAsync("https://jsonplaceholder.typicode.com/posts");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API failed{response.StatusCode}");
        }

        var post = await response.Content.ReadFromJsonAsync<IEnumerable<Post>>();
        if (post == null)
        {
            throw new Exception("Failed to deserialize ");
        }

        return post;
    }

    public async Task CreatePostAsync(Post post)
    {
        await _postsCollection.InsertOneAsync(post);
    }

    public async Task InsertFetchedPostsToMongoAsync()
    {
        // Fetch posts from external API
        var posts = await GetAllPostAsync();

        // Avoid duplicates by checking existing posts in MongoDB
        var existingPostIds = (await _postsCollection.Find(_ => true).ToListAsync()).Select(p => p.Id).ToHashSet();
        var newPosts = posts.Where(p => !existingPostIds.Contains(p.Id));

        if (newPosts.Any())
        {
            await _postsCollection.InsertManyAsync(newPosts);
        }
    }

    public async Task DeletePostAsync(int postId)
    {
        var filter = Builders<Post>.Filter.Eq(p => p.Id, postId);
        var response = await _postsCollection.DeleteOneAsync(filter);
        if (response.DeletedCount == 0)
        {
            Console.WriteLine("post not found");
        }
    }

    public async Task UpdatePostTitleAsync(int postId, string title)
    {
        var filter = Builders<Post>.Filter.Eq(p => p.Id, postId);
        var update = Builders<Post>.Update.Set(p => p.Title, title);
        var result = await _postsCollection.UpdateOneAsync(filter, update);
        if (result.ModifiedCount == 0)
        {
            Console.WriteLine("Update Failed");
        }
    }
}