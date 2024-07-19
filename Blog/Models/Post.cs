namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string[] Position { get; set; }
        public string Category { get; set; }
        public bool IsPublic { get; set; }
        public DateTime PublishDate { get; set; }

        public Post()
        {
            Position = new string[] { };
        }
    }
}
