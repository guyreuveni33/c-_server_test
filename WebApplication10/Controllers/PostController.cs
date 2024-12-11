using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication10.Models;
using WebApplication10.Services;

namespace WebApplication10.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
    {
        var posts = await _postService.GetAllPostAsync();
        if (posts.Any())
        {
            return Ok(posts);
        }

        return NotFound();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPostById(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        return Ok(post);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostsByUserId(int userId)
    {
        var posts = await _postService.FilterPostsByUserIdAsync(userId);
        if (posts.Any())
        {
            return Ok(posts);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] Post post)
    {
        if (post == null)
        {
            BadRequest("Post data is invalid");
        }

        await _postService.CreatePostAsync(post);
        return StatusCode(201, post); // Returns HTTP 201 with the created resource
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePost(int postId)
    {
        try
        {
            await _postService.DeletePostAsync(postId);
            return NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPut("{postId}/title")]
    public async Task<ActionResult> UpdatePostTitle(int postId, [FromBody] string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
        {
            return BadRequest("missing title");
        }

        try
        {
            var res = await _postService.UpdatePostTitleAsync(postId, newTitle);
            if (!res)
            {
                return NotFound("not found");
            }

            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}