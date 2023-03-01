using System.Text.Json;
using Toverto.Models;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using Toverto.Logics;
using Toverto;

Console.WriteLine("Application Started");
try
{
    await new Main().TovertoExport();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    //Can logged to files, but already logging in console. So pushing to DB.
    var logged = await new DataExport().LogInserts(ex);
    // re-throw the exception so it can be handled by the caller
    throw;
}

Console.WriteLine("Application completed.");
