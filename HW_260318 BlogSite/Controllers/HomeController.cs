using HW_260318_BlogSite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HW_260318_BlogSite.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
            @"Data Source=.\sqlexpress;Initial Catalog=BlogPostsSite;Integrated Security=True;TrustServerCertificate=true;";

        public IActionResult Index(int page = 1, int takeAmount = 3)
        {
            var manager = new BlogPostsDbManager(_connectionString);

            return View(new BlogPostsViewModel
            {
                BlogPosts = manager.GetNextBlogPosts(page, takeAmount),
                TotalPages = manager.GetBlogPostCount(),
                CurrentPage = page,
                TakeAmount = takeAmount
            });
        }

        public IActionResult ViewBlog(int id)
        {
            var manager = new BlogPostsDbManager(_connectionString);
            if (id == 0)
            {
                id = manager.GetMostRecentId();
                //return Redirect($"/home/viewblog?id={id}");
            }
            ;

            IndividualBlogViewModel viewModel = new IndividualBlogViewModel
            {
                BlogPost = manager.GetBlogPostById(id),
                Comments = manager.GetCommentsForBlogPost(id)

            };

            var cookie = Request.Cookies["commenterName"];
            if (cookie != null)
            {
                viewModel.CommenterName = cookie;
            }

            if (viewModel.BlogPost == null)
            {
                return Redirect("/home/index");
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddCommentToBlogPost(int id, string text, string commenterName)
        {
            var manager = new BlogPostsDbManager(_connectionString);
            DateTime dateSubmitted = DateTime.Now;
            manager.AddCommentForBlogPost(id, text, commenterName, dateSubmitted);
            Response.Cookies.Append("commenterName", commenterName);
            return Redirect($"/home/viewblog?id={id}");
        }
    }
}
