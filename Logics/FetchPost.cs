using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Toverto.Models;

namespace Toverto.Logics
{
    public class FetchPost
    {
        public async Task<List<PostModel>> FetchPosts()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await new ApiCaller().CallApiWithRetryAsync("https://jsonplaceholder.typicode.com/posts", 3);
            string responseContent = await response.Content.ReadAsStringAsync();
            List<PostModel> posts = JsonSerializer.Deserialize<List<PostModel>>(responseContent);
            Random random = new Random();

            //Hashing id,userid and add a variable
            foreach (PostModel post in posts)
            {
                // Create a timestamp for the current time
                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // Combine the object's properties and the timestamp into a single string
                    string dataToHash = $"{post.id},{post.userId},{timestamp}";

                    // Compute the hash of the combined string
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));

                    // Convert the hash to a lowercase hex string and assign it to the object's HashId property
                    post.HashId = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                }
                // Adding more fun - progress bar 
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Hashing posts: [{new string('=', post.id / 2)}>{new string(' ', 50 - post.id / 2)}] {post.id}%");
                Thread.Sleep(random.Next(0, 10));
            }
            Console.WriteLine("");
            return posts;
        }
    }
}
