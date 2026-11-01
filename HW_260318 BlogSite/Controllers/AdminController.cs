using HW_260318_BlogSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace HW_260318_BlogSite.Controllers
{
    public class AdminController : Controller
    {
        private string _connectionString =
           @"Data Source=.\sqlexpress;Initial Catalog=BlogPostsSite;Integrated Security=True;TrustServerCertificate=true;";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBlogPost(string title, string text)
        {
            BlogPostsDbManager manager = new BlogPostsDbManager(_connectionString);
            int id = manager.AddBlogPost(title, text);
            return Redirect($"/home/viewblog?id={id}");
        }
    }
}
