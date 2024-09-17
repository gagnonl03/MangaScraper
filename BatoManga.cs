using HtmlAgilityPack;

namespace MangaScraper;

public class BatoManga : ScrapedManga
{
    

    public BatoManga() {}


    public override void Initialize(string rootUrl, string rootPath)
    {
        this.rootUrl = rootUrl;
        
        chapterDict = new Dictionary<string, string>();
        deadImages = new List<List<string>>();
        var web = new HtmlWeb();
        var doc = web.Load(rootUrl);
        BuildTitle(doc);
        BuildChapterList(doc);
        BuildDirectoryStructure(rootPath);
        

    }

    public override void InitializeFromFile(string filePath)
    {
        Console.WriteLine("unsupported for now");
    }

    public override async Task DownloadManga()
    {
        List<Task> tasks = new List<Task>();
        foreach (var tuple in chapterDict)
        {
            string chapterPath = Path.Combine(mangaPath, Utils.FormatStringToPathSafe(tuple.Key));
            tasks.Add(DownloadChapter(chapterPath, tuple.Value));
        }
        await Task.WhenAll(tasks);
        
        
    }

    protected override void BuildChapterList(HtmlDocument document)
    {
        var chapterNodes = document.DocumentNode.SelectNodes("//div[@class='space-x-1']");
        foreach (HtmlNode node in chapterNodes)
        {
            chapterDict.Add(node.InnerText, "http://bato.to" + node.FirstChild.Attributes["href"].Value + "?load=2");
        }
    }

    protected override void BuildTitle(HtmlDocument document)
    {
        var parentNode = document.DocumentNode.SelectSingleNode("//h3[@class='text-lg md:text-2xl font-bold']");
        title = parentNode.FirstChild.InnerText;
    }

    protected override void BuildDirectoryStructure(string rootPath)
    {
        mangaPath = Path.Combine(rootPath, Utils.FormatStringToPathSafe(title));
        Directory.CreateDirectory(mangaPath);
        foreach (var chapterName in chapterDict.Keys)
        {
            Directory.CreateDirectory(Path.Combine(mangaPath, Utils.FormatStringToPathSafe(chapterName)));
        }
    }

    protected override async Task DownloadChapter(string chapterPath, string url)
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(url);
        var stuff = doc.DocumentNode.SelectSingleNode("//astro-island[@component-url='/_astro/ImageList.1931435f.js']");
        string dump = stuff.Attributes["props"].Value;
        var output = dump.Split("https://"); 
        output = output.Skip(1).ToArray();
        var imageList = new List<string>();
        foreach (string temp in output)
        {
            var temp2 = temp.Substring(0, temp.IndexOf('\\'));
            imageList.Add("https://" + temp2);
        }
        List<Task> tasks = new List<Task>();
        int imageNum = 1;
        foreach (string imageUrl in imageList)
        {
            tasks.Add(DownloadSingleImage(imageUrl, chapterPath, imageNum));
            imageNum++;
        }
        await Task.WhenAll(tasks);
    }

    protected override async Task DownloadSingleImage(string imageUrl, string chapterPath, int imgNumber)
    {
        byte[] fileBytes;
        try
        {
            string imagePath = Path.Combine(chapterPath, imgNumber + ".png");
            if (File.Exists(imagePath))
            {
                return;
            }

            fileBytes = await Program.client.GetByteArrayAsync(imageUrl);
            File.WriteAllBytes(imagePath, fileBytes);
            Console.WriteLine("FINISHED IMAGE " + imagePath);
        }
        catch (Exception e)
        {
            Console.WriteLine("IMAGE " + chapterPath + imgNumber + ".png " + "FAILED!");
            deadImages.Add(new List<string> {imageUrl, chapterPath, imgNumber.ToString()});
            
        }
    }

    public async Task RetryDeadImages()
    {
        if (deadImages.Count == 0)
        {
            return;
        }

        Console.WriteLine("Retrying dead images");
        List<Task> tasks = new List<Task>();
        while (deadImages.Count > 0)
        {
            List<string> image = deadImages.First();
            tasks.Add(DownloadSingleImage(image[0], image[1], Int32.Parse(image[2])));
            deadImages.Remove(image);
        }

        await Task.WhenAll(tasks);
    }


    private void InitalizeChapters()
    {
        
    }
}