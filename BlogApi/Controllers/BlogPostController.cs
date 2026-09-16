using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=Blog;uid=root;password=";
        
        public List<BlogPost> GetAllBlogpost()
        {
            List<BlogPost> blogposts = new();
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = "SELECT * FROM blogpost;";

            var cmd = new MySqlCommand(sql, connector);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var blogpost = new BlogPost();
                {
                    Id = datareader.GetInt32(0);
                    Title = datareader.GetString(1);
                    Content = datareader.GetString(2);
                    postTime = datareader.GetDateTime(3);
                    updateTime = datareader.GetDateTime(4);
                    blogId = datareader.GetInt32(5);
                }
            }

            connector.Close();
            return blogposts;
        }
    }
}
