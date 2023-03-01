using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toverto.EntityModels;
using Toverto.Models;

namespace Toverto.Logics
{
    public class DataExport
    {

        public string ToCsv<T>(IEnumerable<T> items, long fileId)
        {
            var props = typeof(T).GetProperties();
            var csv = new StringBuilder();

            csv.AppendLine(string.Join(",", props.Select(p => p.Name)));

            foreach (var item in items)
            {
                csv.AppendLine(string.Join(",", props.Select(p => p.GetValue(item)?.ToString())));
            }
            File.WriteAllText("posts" + fileId + ".csv", csv.ToString());
            return "Success";
        }

        public async Task<string> SQLInserts(IEnumerable<PostModel> posts)
        {
            var dbcontext = new TovertoContext();
            foreach (var pos in posts)
            {
                var post = new Post { Id = pos.id, Body = pos.body, HashId = pos.HashId, Title = pos.title, UserId = pos.userId };
                dbcontext.Posts.Add(post);
            }
            return await RetryAndSaveDB(dbcontext, 3);
        }
        public async Task<string> LogInserts(Exception ex)
        {
            var dbcontext = new TovertoContext();
            
            var log = new Log { Message = ex.Message,InnerException = ex.InnerException.ToString(),Source = ex.Source,StackTrace = ex.StackTrace};
            dbcontext.Logs.Add(log);
            return await RetryAndSaveDB(dbcontext, 3);
        }
        public async Task<string> RetryAndSaveDB(TovertoContext dbcontext, int maxRetries)
        {
            int retryCount = 0;
            while (retryCount < maxRetries)
            {
                try
                {
                    dbcontext.SaveChanges();
                    return "Success";
                }
                catch (Exception ex)
                {
                    if(ex is DbUpdateException) { 
                        retryCount++;
                        if (retryCount >= maxRetries)
                            {
                                throw new Exception($"Failed to save changes to database after {maxRetries} retries.", ex);
                            }
                            else
                            {
                                await Task.Delay(TimeSpan.FromSeconds(2));
                            }
                    }
                    else
                    {
                        throw new Exception($"Failed to save changes to database after {maxRetries} retries.", ex);
                    }
                }
            }
            return "failed";

        }
    }
}
