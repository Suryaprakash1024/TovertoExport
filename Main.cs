using System.Configuration;
using System.Text.Json;
using Toverto.Logics;
using Toverto.Models;

namespace Toverto
{
    public class Main
    {

        public async Task TovertoExport()
        {
            Console.Write("Fetching Posts...");

            //Fetching posts from API
            List<PostModel> posts = await new FetchPost().FetchPosts();

            string Setting = ConfigurationManager.AppSettings["Setting"];
            long fileId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var res = "";
            switch (Setting)
            {
                case FileFormats.Csv:
                    posts.ForEach(post =>
                    {
                        post.body = post.body.Replace("\n", "");
                    });
                    res = new DataExport().ToCsv(posts, fileId);
                    Console.WriteLine("CSV File generated..");
                    break;
                case FileFormats.Sql:
                    res = await new DataExport().SQLInserts(posts);
                    Console.WriteLine("Posts inserted in sql");
                    break;
                case FileFormats.Json:
                    string json = JsonSerializer.Serialize(posts);
                    File.WriteAllText("posts" + fileId + ".json", json);
                    Console.WriteLine("JSON File generated..");
                    break;
                default:
                    Console.WriteLine("Error: No Such formats!! kindly check config.");
                    break;
            }
        }
    }
}
