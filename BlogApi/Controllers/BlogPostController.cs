using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("blogpost")]
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
                var blogpost = new BlogPost()
                {
                    Id = datareader.GetInt32(0),
                    Title = datareader.GetString(1),
                    Content = datareader.GetString(2),
                    postTime = datareader.GetDateTime(3),
                    updateTime = datareader.GetDateTime(4),
                    blogId = datareader.GetInt32(4)
                };
                blogposts.Add(blogpost);
            }

            connector.Close();
            return blogposts;
        }

        [HttpPost]
        public BlogPost AddNewPost(AddPostDTO addPostDTO)
        {

            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var post = new BlogPost()
            {
                Title = addPostDTO.Title,
                Content = addPostDTO.Content,
                postTime = DateTime.Now,
                updateTime = DateTime.Now,
                blogId = addPostDTO.blogId

            };

            var sql = $"INSERT INTO `blogpost`(`Title`, `Content`, `postTime`, `updateTime`, `blogId`) VALUES ('@title','@content','@posttime','@updatetime','@blogid')";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@title", post.Title);
            cmd.Parameters.AddWithValue("@content", post.Content);
            cmd.Parameters.AddWithValue("@posttime", post.postTime);
            cmd.Parameters.AddWithValue("@updatetime", post.updateTime);
            cmd.Parameters.AddWithValue("@blogid", post.blogId);

            cmd.ExecuteNonQuery();

            connector.Close();

            return post;
        }

        [HttpDelete]
        public object DeletePost(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = $"DELETE FROM `blogpost` WHERE id = @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connector.Close();

            return new object m;
        }
    }
}
