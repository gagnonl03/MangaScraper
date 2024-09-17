namespace MangaScraper;

public class Utils
{
    public async static Task<bool> DownloadSingleImage(string imageUrl, string filePath, List<string>? deadChapters = null)
    {
        try
        {
            byte[] fileBytes = await Program.client.GetByteArrayAsync(imageUrl);
            File.WriteAllBytes(filePath, fileBytes);
            Console.WriteLine("FINISHED IMAGE: " + filePath);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("FAILED: " + filePath);
            if (deadChapters != null)
            {
                deadChapters.Add(imageUrl);
            }
        }

        return false;
    }

    public static void BuildDirectoryStructrue(Dictionary<string, string> chapterDict)
    {
        
    }

    public static string FormatStringToPathSafe(string str)
    {
        return str.Replace("#", "(pound)")
            .Replace("&", "(amp)")
            .Replace("%", "(percent)")
            .Replace("{", "(rb)")
            .Replace("}", "(lb)")
            .Replace("*", "(ast)")
            .Replace("\\", "(bslash)")
            .Replace("/", "(fslash)")
            .Replace("|", "(pipe)")
            .Replace("<", "(less)")
            .Replace(">", "(greater)")
            .Replace("=", "(equals)")
            .Replace("+", "(plus)")
            .Replace("\"", "(dquote)")
            .Replace("\'", "(squote)")
            .Replace("!", "(exclam)")
            .Replace("@", "(at)")
            .Replace(":", "(colon)")
            .Replace("`", "(backtick)")
            .Replace("?", "(qmark)")
            .Replace("$", "(dollar)");
    }

    public static string FormatPathSafeToString(string str)
    {
        return str.Replace("(pound)", "#")
            .Replace("(amp)", "&")
            .Replace("(percent)", "%")
            .Replace("(rb)", "{")
            .Replace("(lb)", "}")
            .Replace("(ast)", "*")
            .Replace("(bslash)", "\\")
            .Replace("(fslash)", "/")
            .Replace("(pipe)", "|")
            .Replace("(less)", "<")
            .Replace("(greater)", ">")
            .Replace("(equals)", "=")
            .Replace("(plus)", "+")
            .Replace("(dquote)", "\"")
            .Replace("(squote)", "\'")
            .Replace("(exclam)", "!")
            .Replace("(at)", "@")
            .Replace("(colon)", ":")
            .Replace("(backtick)", "`")
            .Replace("(qmark)", "?")
            .Replace("(dollar)", "$");
    }
}