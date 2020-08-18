using CommandLine;

namespace Succubus.Common
{
    public static class OptionsParser
    {
        public static T Parse<T>(string[] args) where T : class, new()
        {
            using var parser = new Parser();
            var result = parser.ParseArguments<T>(args);
            var options = new T();

            result.MapResult(x => x, x => options);

            return options;
        }
    }
}