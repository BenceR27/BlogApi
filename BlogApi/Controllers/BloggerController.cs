using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Controllers
{
    [Route("blogger")]
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

            string sql = @"SELECT * FROM blogger;";
            
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
        public Blogger AddNewBlogger(AddBloggerDTO blogger)
        {

            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var blg = new Blogger()
            {
                Name = blogger.Name,
                Email = blogger.Email,
                Age = blogger.Age,
                Password = blogger.Password,
                RegistrationTime = DateTime.Now
            };

            var sql = @"INSERT INTO `blogger` (`Name`, `Email`, `Age`, `Password`, `RegistrationTime`) VALUES (@name,@email,@age,@password,@registrationtime)";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@name", blg.Name);
            cmd.Parameters.AddWithValue("@email", blg.Email);
            cmd.Parameters.AddWithValue("age", blg.Age);
            cmd.Parameters.AddWithValue("@password", blg.Password);
            cmd.Parameters.AddWithValue("@registrationtime", blg.RegistrationTime);

            cmd.ExecuteNonQuery();

            connector.Close();

            return blg;
        }

        [HttpPut]
        public object UpdateBlogger([FromQuery] int id, [FromBody] UpdateBloggerDTO updateBloggerDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"UPDATE `blogger` SET `name`=@name,`email`=@email,`age`=@age,`password`=@password 
                WHERE `id`= @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@name", updateBloggerDto.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDto.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDto.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDto.Password);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            var updatedBlogger = new UpdateBloggerDTO
            {
                Name = updateBloggerDto.Name,
                Email = updateBloggerDto.Email,
                Age = updateBloggerDto.Age,
                Password = updateBloggerDto.Password
            };

            connector.Close();

            return new { message = "Sikeres frissítés.", result = updatedBlogger };
        }


        [HttpDelete]
        public object DeleteBlogger(int id, DeleteBloggerDTO blogger)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var blgdelete = new Blogger()
            {
                Id = blogger.Id
            };

            var sql = @"DELETE FROM `blogger` WHERE Id=@Id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@id", blgdelete.Id);

            cmd.ExecuteNonQuery();

            connector.Close();

            return blgdelete;
        }

        [HttpGet("byId")]
        public object GetBloggerById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT `name`,`email` FROM `blogger` WHERE `id` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();
            datareader.Read();
            var blogger = new
            {
                Name = datareader.GetString(0),
                Email = datareader.GetString(1)
            };

            connector.Close();
            return blogger;
        }

        [HttpGet("bloggerOwnPost")]
        public List<object> GetBloggerWithPost(int id)
        {
            List<object> ownPost = new List<object>();
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT blogger.name, blogpost.title, blogpost.content  
                        FROM `blogger` 
                        INNER JOIN blogpost ON blogger.id = blogpost.blogId
                        WHERE blogger.`id` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var bloggerOwnPosts = new
                {
                    Name = datareader.GetString(0),
                    Title = datareader.GetString(1),
                    Content = datareader.GetString(2)
                };

                ownPost.Add(bloggerOwnPosts);
            }



            connector.Close();

            return ownPost;
        }

        [HttpGet("NumberOfPosts")]
        public object GetNumerOfPosts()
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var sql = @"SELECT COUNT(*) FROM blogpost";

            var cmd = new MySqlCommand(sql, connector);

            var db = cmd.ExecuteScalar();

            connector.Close();

            return new { message = $"Posztok száma : {db}" };
        }
    }
}
