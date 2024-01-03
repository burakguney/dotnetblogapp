using BlogApp.Data.Abstract;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private IPostRepository _postRepository;
        public PostsController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public IActionResult Index()
        {
            return View(
                new PostsViewModel
                {
                    Posts = _postRepository.Posts.ToList()
                }
            );
        }

        [HttpGet("posts/{url}")]
        public async Task<IActionResult> Details(string? url)
        {
            return View(await _postRepository
            .Posts.Include(x => x.Tags).FirstOrDefaultAsync(p => p.Url == url));
        }

        [HttpGet("posts/tag/{url}")]
        public async Task<IActionResult> ListByTags(string? url)
        {
            var posts = _postRepository.Posts;

            posts = posts.Where(x => x.Tags.Any(t => t.Url == url));

            PostsViewModel model = new()
            {
                Posts = await posts.ToListAsync()
            };

            return View("Index", model);
        }
    }
}