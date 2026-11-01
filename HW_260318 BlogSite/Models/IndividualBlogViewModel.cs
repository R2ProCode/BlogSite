namespace HW_260318_BlogSite.Models
{
    public class IndividualBlogViewModel
    {
        public BlogPost BlogPost { get; set; }
        public List<Comment> Comments { get; set; }
        public string CommenterName { get; set; }
    }
}
