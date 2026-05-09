using System.Net;
using HtmlAgilityPack;

namespace MangaScraper;

class Program
{
    public static readonly HttpClient client = new HttpClient();
    public static readonly string PROJECT_PATH = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
    static async Task Main(string[] args)
    {


        //var doc = new HtmlWeb().Load("https://bato.to/title/82074-horimiya-official/2307554-vol_16-ch_122.3?load=2");
        //var node = doc.DocumentNode.SelectSingleNode("//h6[@class='text-lg space-x-2']");
        //Console.WriteLine(node.FirstChild.FirstChild.InnerText);
        //Console.WriteLine(doc.DocumentNode.SelectNodes("//div[@name='image-item']").Count);

        
        var watch = System.Diagnostics.Stopwatch.StartNew();
        BatoManga manga = new BatoManga();
        manga.Initialize("https://bato.to/title/86666-bloom-into-you-official", PROJECT_PATH);
        await manga.DownloadManga();
        while (manga.deadImages.Count > 0)
        {
            await manga.RetryDeadImages();
        }
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Console.WriteLine($"Elapsed time: {elapsedMs} ms");


    }
}
