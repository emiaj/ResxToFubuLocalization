using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FubuCore;
using FubuCore.CommandLine;
using FubuCore.Util;
using FubuLocalization;
using FubuLocalization.Basic;

namespace ResxToFubuLocalization.Core.Commands
{
    [CommandDescription("Transform .resx files into localized fubu .locale files")]
    public class TransformCommand : FubuCommand<FolderInput>
    {
        public override bool Execute(FolderInput input)
        {
            var factory = new ResxFileFactory(new FileSystem());
            var cache = new Cache<string, IList<LocalString>>(x => new List<LocalString>());
            Console.WriteLine("Searching for resx files in {0}", input.Source);
            var files = new FileSystem().FindFiles(input.Source, new FileSet {Include = "*.resx"}).ToList();
            if (files.Count == 0)
            {
                Console.WriteLine("Resx files not found.");
                return true;
            }
            Console.WriteLine("Resx files found: {0}", files.Join(","));
            files.Each(file =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Transforming file: {0}", file);
                var resxFile = factory.CreateFrom(file);
                var strings = resxFile.Data
                    .Select(x => new LocalString(string.Format("{0}.{1}", resxFile.Name, x.Key), x.Value))
                    .ToList();
                var culture = resxFile.Culture ?? input.DefaultCulture;
                strings.Each(s => Console.WriteLine("LocalString: {0}", s));
                cache[culture].AddRange(strings);
                Console.ResetColor();
                Console.WriteLine("--------------------------------------");
            });
            if (!Directory.Exists(input.Target))
            {
                Directory.CreateDirectory(input.Target);
            }
            cache.GetAllKeys().Each(k =>
            {
                var culture = CultureInfo.GetCultureInfo(k);
                XmlDirectoryLocalizationStorage.Write(input.Target, culture, cache[k]);
            });

            return true;
        }
    }
}