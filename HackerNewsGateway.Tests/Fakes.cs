using System.Reflection;

namespace HackerNewsGateway.Tests
{
    public class Fakes
    {
        public static string GetResourceString(string filename)
        {
            var binDir = (new FileInfo(Assembly.GetExecutingAssembly().Location)).Directory.FullName;
            var fileFullPath = Path.Combine(binDir, "Samples", filename);
            var content = File.ReadAllText(fileFullPath);
            return content;
        }
    }
}
