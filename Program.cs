using System.Net;
using HtmlAgilityPack;

namespace MangaScraper;

class Program
{
    public static readonly HttpClient client = new HttpClient();
    public static readonly string PROJECT_PATH = Path.Combine(Environment.CurrentDirectory, "..", "..", "..");
    static async Task Main(string[] args)
    {

        var doc = new HtmlWeb().Load("https://bato.to/title/82074-horimiya-official/2307554-vol_16-ch_122.3?load=2");
        var node = doc.DocumentNode.SelectSingleNode("//h6[@class='text-lg space-x-2']");
        Console.WriteLine(node.FirstChild.FirstChild.InnerText);
        Console.WriteLine(doc.DocumentNode.SelectNodes("//div[@name='image-item']").Count);

        /*
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

        /*


        //Console.WriteLine(doc.Text);
        var stuff = doc.DocumentNode.SelectNodes("//div[@class='space-x-1']");
        List<string> chapterUrls = new List<string>();
        foreach (HtmlNode node in stuff.Nodes())
        {
            if (node.Attributes["href"] != null)
            {
                Console.WriteLine(node.InnerText);
                //Console.WriteLine(node.Attributes["href"].Value);
                chapterUrls.Add("https://bato.to" + node.Attributes["href"].Value + "?load=2");
            }
        }

        var runningMethods = new List<Task<string[]>>();
        foreach (string url2 in chapterUrls)
        {
            runningMethods.Add(GetImagesFromChapter(url2));
        }

        var results = await Task.WhenAll(runningMethods);
        Console.WriteLine("Printing results");
        foreach (string[] chap in results)
        {
            foreach (string str in chap)
            {
                Console.WriteLine(str);
            }
        }

        watch.Stop();
        Console.WriteLine($"Elapsed time: {watch.Elapsed}");
        /*
        var manga = new ScrapedManga(chapterUrls, "duh", url);
        manga.DownloadManga();
        */
    }

    public static async Task<string[]> GetImagesFromChapter(string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);
        var stuff = doc.DocumentNode.SelectSingleNode("//astro-island[@component-url='/_astro/ImageList.1931435f.js']");
        string dump = stuff.Attributes["props"].Value;

        

        var output = dump.Split("https://"); 
        output = output.Skip(1).ToArray();
        var finalOutput = new List<string>();
        //Console.WriteLine(url);
        finalOutput.Add(url);
        foreach (string temp in output)
        {
            var temp2 = temp.Substring(0, temp.IndexOf('\\'));
            //Console.WriteLine(temp2);
            finalOutput.Add(temp2);
        }
        return finalOutput.ToArray();
    }
}
