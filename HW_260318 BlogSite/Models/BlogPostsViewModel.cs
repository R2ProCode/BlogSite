namespace HW_260318_BlogSite.Models
{
    public class BlogPostsViewModel
    {
        public List<BlogPost> BlogPosts { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int TakeAmount { get; set; }
    }
}
