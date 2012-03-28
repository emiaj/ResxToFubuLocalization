using System;
using System.ComponentModel;

namespace ResxToFubuLocalization.Core.Commands
{
    public class FolderInput
    {
        public FolderInput()
        {
            DefaultCulture = "en-US";
        }
        [Description("The folder where .resx files will be found")]
        public string Source { get; set; }
        [Description("The folder where .locate files will be written")]
        public string Target { get; set; }
        [Description("The culture used by default")]
        public string DefaultCulture { get; set; }
    }
}