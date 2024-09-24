using HtmlAgilityPack;

namespace MangaScraper;

public abstract class ScrapedManga
{
    protected bool isInitialized = false;
    protected string rootUrl;
    protected string title;
    protected Dictionary<string, string> chapterDict;
    protected string mangaPath;
    public List<List<string>> deadImages;
    public List<ScrapedChapter> chapters;
    public abstract void Initialize(string rootUrl, string rootPath);
    public abstract void InitializeFromFile(string filePath);
    public abstract Task DownloadManga();
    protected abstract void BuildChapterList(HtmlDocument document);
    protected abstract void BuildTitle(HtmlDocument document);
    protected abstract void BuildDirectoryStructure(string rootPath);
    protected abstract Task DownloadChapter(string chapterPath, string url);
    protected abstract Task DownloadSingleImage(string imageUrl, string fileName, int imageNumber);
}