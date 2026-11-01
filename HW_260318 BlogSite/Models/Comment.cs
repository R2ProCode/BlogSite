namespace HW_260318_BlogSite.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string CommenterName { get; set; }
        public DateTime DateSubmitted { get; set; }
        public int BlogPostId { get; set; }
    }
}
