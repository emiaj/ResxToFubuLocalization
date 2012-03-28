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
            var files = new FileSystem().FindFiles(input.Source, new FileSet {Include = "*.resx"});
            files.Each(file =>
            {
                var resxFile = factory.CreateFrom(file);
                var strings = resxFile.Data.Select(x => new LocalString(x.Key, x.Value));
                var culture = resxFile.Culture ?? input.DefaultCulture;
                var targetFilename = "{0}.{1}.locale.config".ToFormat(culture, resxFile.Name);
                var target = FileSystem.Combine(input.Target, targetFilename);
                if(!Directory.Exists(Path.GetDirectoryName(target)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(target));
                }
                XmlDirectoryLocalizationStorage.Write(target, strings);
            });
            return true;
        }
    }
}