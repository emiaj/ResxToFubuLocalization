using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using FubuCore;

namespace ResxToFubuLocalization.Core
{
    public class ResxFileFactory
    {
        private readonly IFileSystem _fileSystem;

        public ResxFileFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public ResxFile CreateFrom(string path)
        {
            var resxFileInfo = new ResxFile();
            resxFileInfo.FilePath = path;
            resxFileInfo.FileName = Path.GetFileName(path);
            resxFileInfo.Name = Path.GetFileNameWithoutExtension(path);
            if (Path.HasExtension(resxFileInfo.Name))
            {
                resxFileInfo.Culture = Path.GetExtension(resxFileInfo.Name).TrimStart('.');
            }
            var content = _fileSystem.ReadStringFromFile(path);
            var xml = XDocument.Parse(content);
            var entries = from x in xml.Root.Descendants("data")
                          let key = x.Attribute("name")
                          let value = x.Element("value")
                          where key != null && value != null
                          select new { Key = key.Value, value.Value };
            entries.Each(e => resxFileInfo.Data[e.Key] = e.Value);
            return resxFileInfo;
        }
    }
}