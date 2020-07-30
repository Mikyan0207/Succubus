﻿using CommandLine;
using Succubus.Commands;
using Succubus.Database.Options;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Succubus.Utils
{
    public static class OptionsParser
    {
        public static T Parse<T>(string[] args) where T : ICommandOptions, new() => Parse(new T(), args).Item1;

        public static (T, bool) Parse<T>(T options, string[] args) where T : ICommandOptions
        {
            using (var parser = new Parser())
            {
                var result = parser.ParseArguments<T>(args);
                options = result.MapResult(x => x, x => options);

                return (options, result.Tag == ParserResultType.Parsed);
            }
        }
    }
}