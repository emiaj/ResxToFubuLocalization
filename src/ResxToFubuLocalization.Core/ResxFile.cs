using System.Collections.Generic;

namespace ResxToFubuLocalization.Core
{
    public class ResxFile
    {
        public ResxFile()
        {
            Data = new Dictionary<string, string>();
        }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Culture { get; set; }
        public string Extension { get; set; }
        public IDictionary<string, string> Data { get; private set; }
    }
}