namespace MangaScraper;

public class Utils
{
    public static async Task DownloadSingleImage(string imageUrl, string filePath, Dictionary<string, string> deadImages)
    {
        try
        {
            byte[] fileBytes = await Program.client.GetByteArrayAsync(imageUrl);
            File.WriteAllBytes(filePath, fileBytes);
            Console.WriteLine("FINISHED IMAGE: " + filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("FAILED: " + filePath);
            deadImages.Add(imageUrl, filePath);
        }
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