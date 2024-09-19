using HtmlAgilityPack;

namespace MangaScraper;

public abstract class Chapter
{
    protected string title;
    protected string chapterPath;
    protected int numImages;
    protected string chapterUrl;
    public bool finished = false;
    //deadChapterDict is EXPECTED to have [imageUrl, imageFilePath] imageFilePath must be complete!
    //(ie: ...\[manga]\[chapter]\[number].png. These keys and values are expected to be passed directly back into Utils.DownloadSingleImage
    private Dictionary<string, string> deadChapterDict = new Dictionary<string, string>();

    public abstract void InitializeFull(string mangaPath, string chapterUrl);
    public abstract void InitializePartial(string mangaPath, string chapterUrl);
    public abstract void InitializeFinish();
    
    protected async void DownloadImages(HtmlDocument doc)
    {
        List<string> imageUrls = GetImageUrlsFromDoc(doc);
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < imageUrls.Count; i++)
        {
            tasks.Add(Utils.DownloadSingleImage(imageUrls[i], Path.Combine(chapterPath, chapterPath + $"{i + 1}.png")));
        }
        
        await Task.WhenAll(tasks);
    }

    protected async void RetryDeadImages()
    {
        Dictionary<string, string> deadChapterDictTemp = new Dictionary<string, string>();
        foreach (KeyValuePair<string, string> kvp in deadChapterDict)
        {
            deadChapterDictTemp.Add(kvp.Key, kvp.Value);
        }

        deadChapterDict.Clear();

        List<Task> tasks = new List<Task>();
        
        while (deadChapterDictTemp.Count > 0)
        {
            var deadImage = deadChapterDictTemp.First();
            tasks.Add(Utils.DownloadSingleImage(deadImage.Key, deadImage.Value));
            deadChapterDictTemp.Remove(deadImage.Key);
        }
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

    protected abstract Task<HtmlDocument> SetupBasicInfo(string mangaPath, string chapterUrl);
    protected abstract int GetNumImagesFromDoc(HtmlDocument doc);
    protected abstract string GetTitleFromDoc(HtmlDocument doc);
    protected abstract List<string> GetImageUrlsFromDoc(HtmlDocument doc);
}