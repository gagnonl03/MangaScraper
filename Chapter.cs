using HtmlAgilityPack;

namespace MangaScraper;

public abstract class Chapter
{
    protected string title;
    protected string chapterPath;
    protected int numImages;
    protected string chapterUrl;
    public bool finished = false;

    public abstract void InitializeFull(string mangaPath, string chapterUrl);
    public abstract void InitializePartial(string mangaPath, string chapterUrl);

    protected async void DownloadImages(HtmlDocument doc)
    {
        List<string> imageUrls = GetImageUrlsFromDoc(doc);
        List<Task<bool>> tasks = new List<Task<bool>>();
        for (int i = 0; i < imageUrls.Count; i++)
        {
            tasks.Add(Utils.DownloadSingleImage(imageUrls[i], Path.Combine(chapterPath, chapterPath + $"{i + 1}.png")));
        }
        
        await Task.WhenAll(tasks);
    }

    public async Task Download()
    {
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(chapterUrl);
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
    }
    protected void MakeBaseDirectory(string mangaPath)
    {
        chapterPath = Path.Combine(mangaPath, Utils.FormatStringToPathSafe(title));
        Directory.CreateDirectory(chapterPath);
    }
    
    protected abstract int GetNumImagesFromDoc(HtmlDocument doc);
    protected abstract string GetTitleFromDoc(HtmlDocument doc);
    protected abstract List<string> GetImageUrlsFromDoc(HtmlDocument doc);
}