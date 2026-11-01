using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace HW_260318_BlogSite.Models
{
    public class BlogPostsDbManager
    {
        private string _connectionString;

        public BlogPostsDbManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<BlogPost> GetNextBlogPosts(int page, int takeAmount)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM BlogPosts
                                    ORDER BY DateSubmitted DESC
                                    OFFSET @skipamount ROWS
                                    FETCH NEXT @takeamount ROWS ONLY";
            command.Parameters.AddWithValue("@skipamount", (page - 1) * takeAmount);
            command.Parameters.AddWithValue("@takeamount", takeAmount);
            connection.Open();
            var reader = command.ExecuteReader();
            var blogPosts = new List<BlogPost>();
            while (reader.Read())
            {
                blogPosts.Add(new BlogPost
                {
                    Id = (int)reader["Id"],
                    Title = (string)reader["Title"],
                    Text = (string)reader["Text"],
                    DateSubmitted = (DateTime)reader["DateSubmitted"]
                });
            }

            return blogPosts;
        }

        public BlogPost GetBlogPostById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM BlogPosts                                   
                                    WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                return null;
            }

            return new BlogPost
            {
                Id = (int)reader["Id"],
                Title = (string)reader["Title"],
                Text = (string)reader["Text"],
                DateSubmitted = (DateTime)reader["DateSubmitted"]              
            };
        }

        public List<Comment> GetCommentsForBlogPost(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT c.CommenterName, c.Text, c.DateSubmitted FROM Comments c
                                    JOIN BlogPosts bp
                                    ON c.BlogPostId = bp.Id
                                    WHERE bp.Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = command.ExecuteReader();

            List<Comment> comments = new List<Comment>();
            while (reader.Read())
            {
                comments.Add(new Comment
                {
                    CommenterName = (string)reader["CommenterName"],
                    Text = (string)reader["Text"],
                    DateSubmitted = (DateTime)reader["DateSubmitted"]
                });
            }
            return comments;
        }

        public int AddBlogPost(string title, string text)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO BlogPosts (Title, Text, DateSubmitted)
                                    VALUES (@title, @text, @datesubmitted)
                                    SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@title", title);
            command.Parameters.AddWithValue("@text", text);
            command.Parameters.AddWithValue("@datesubmitted", DateTime.Now);

            connection.Open();
            return (int)(decimal)command.ExecuteScalar();
        }

        public void AddCommentForBlogPost(int id, string text, string commenterName)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Comments (Text, CommenterName, DateSubmitted, BlogPostId)
                                    VALUES (@text, @commenterName, @datesubmitted, @id)";
            command.Parameters.AddWithValue("@text", text);
            command.Parameters.AddWithValue("@commenterName", commenterName);
            command.Parameters.AddWithValue("@datesubmitted", DateTime.Now);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public int GetMostRecentId()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT TOP 1 Id FROM BlogPosts
                                    ORDER BY DateSubmitted DESC";
            connection.Open();
            return (int)command.ExecuteScalar();
        }

        public int GetBlogPostCount()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT COUNT(*) FROM BlogPosts";
            connection.Open();
            return (int)command.ExecuteScalar();
        }
    }
}
