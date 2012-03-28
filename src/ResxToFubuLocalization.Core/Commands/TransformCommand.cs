using System.Collections.Generic;
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
                var targetFilename = string.IsNullOrEmpty(resxFile.Culture)
                                         ? "{0}.{1}.locale.config".ToFormat(resxFile.Name, resxFile.Culture)
                                         : "{0}.locale.config".ToFormat(resxFile.Name);
                var target = FileSystem.Combine(input.Target, targetFilename);
                XmlDirectoryLocalizationStorage.Write(target, strings);
            });
            return true;
        }
    }
}