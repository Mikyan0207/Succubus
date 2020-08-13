using CommandLine;

namespace Succubus.Common
{
    public static class OptionsParser
    {
        public static T Parse<T>(string[] args) where T : class, new()
        {
            return Parse(new T(), args).Item1;
        }

        public static (T, bool) Parse<T>(T options, string[] args) where T : class
        {
            using var parser = new Parser();
            var result = parser.ParseArguments<T>(args);
            options = result.MapResult(x => x, x => options);

            return (options, result.Tag == ParserResultType.Parsed);
        }
    }
}