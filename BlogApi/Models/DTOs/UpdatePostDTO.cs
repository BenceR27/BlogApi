namespace BlogApi.Models.DTOs
{
    public class UpdatePostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int blogId { get; set; }

    }
}
