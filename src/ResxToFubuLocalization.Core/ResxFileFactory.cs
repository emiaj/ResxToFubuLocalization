using System.Collections.Generic;
using System.IO;
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
            var resxFileInfo = new ResxFile
                                   {
                                       FilePath = path,
                                       FileName = Path.GetFileNameWithoutExtension(path),
                                       Extension = Path.GetExtension(path),
                                   };

            if (Path.HasExtension(resxFileInfo.FileName))
            {
                resxFileInfo.Name = Path.GetFileNameWithoutExtension(resxFileInfo.FileName);
                resxFileInfo.Culture = Path.GetExtension(resxFileInfo.FileName).TrimStart('.');
            }
            else
            {
                resxFileInfo.Name = resxFileInfo.FileName;
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