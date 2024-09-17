using System.Reflection;
using HtmlAgilityPack;

namespace MangaScraper;

public class BatoChapter : Chapter
{

    public override async void InitializeFull(string mangaPath, string chapterUrl)
    {
        InitializePartial(mangaPath, chapterUrl);
    }

    public override async void InitializePartial(string mangaPath, string chapterUrl)
    {
        this.chapterUrl = chapterUrl;
        var web = new HtmlWeb();
        var doc = await web.LoadFromWebAsync(chapterUrl);
        title = doc.DocumentNode.SelectSingleNode("//h6[@class='text-lg space-x-2']").FirstChild.FirstChild.InnerText;
        numImages = doc.DocumentNode.SelectNodes("//div[@name='image-item']").Count;
        MakeBaseDirectory(mangaPath);

    }

    protected override int GetNumImagesFromDoc(HtmlDocument doc)
    {
        return doc.DocumentNode.SelectNodes("//div[@name='image-item']").Count;
    }

    protected override string GetTitleFromDoc(HtmlDocument doc)
    {
        return doc.DocumentNode.SelectSingleNode("//h6[@class='text-lg space-x-2']").FirstChild.FirstChild.InnerText;
    }

    protected override List<string> GetImageUrlsFromDoc(HtmlDocument doc)
    {
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

        return imageList;
    }

}