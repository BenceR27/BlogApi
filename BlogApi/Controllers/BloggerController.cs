using BlogApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        private readonly string ConnectionString = "server=localhost;database=Blog;uid=root;password=";

        [HttpGet]
        public List<Blogger> GetAllBlogger()
        {
            List<Blogger> bloggers = new();
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = "SELECT * FROM blogger;";
            
            var cmd = new MySqlCommand(sql, connector);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var blogger = new Blogger()
                {
                    Id = datareader.GetInt32(0),
                    Name = datareader.GetString(1),
                    Email = datareader.GetString(2),
                    Age = datareader.GetInt32(3),
                    Password = datareader.GetString(4),
                    RegistrationTime = datareader.GetDateTime(5)
                };
                bloggers.Add(blogger);
            }
            
            connector.Close();
            return bloggers;
        }

        [HttpPost]
        public object AddNewBlogger(Blogger blogger)
        {
            return null;
        }

        [HttpPut]
        public object UpdateBlogger(int id, Blogger blogger)
        {
            return null;
        }

        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            return null;
        }
    }
}
