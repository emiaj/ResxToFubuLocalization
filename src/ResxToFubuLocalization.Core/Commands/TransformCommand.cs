using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FubuCore;
using FubuCore.CommandLine;
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
                var strings = resxFile.Data.Select(x => new LocalString(x.Key, x.Value));
                var culture = resxFile.Culture ?? input.DefaultCulture;
                var targetFilename = "{0}.{1}.locale.config".ToFormat(culture, resxFile.Name);
                var target = FileSystem.Combine(input.Target, targetFilename);
                if(!Directory.Exists(Path.GetDirectoryName(target)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(target));
                }
                strings.Each(s => Console.WriteLine("LocalString: {0}", s));
                Console.WriteLine("Writting locale strings to: {0}", target);
                XmlDirectoryLocalizationStorage.Write(target, strings);
                Console.ResetColor();
                Console.WriteLine("--------------------------------------");
            });
            return true;
        }
    }
}