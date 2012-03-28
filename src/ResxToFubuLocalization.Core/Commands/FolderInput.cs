using System;
using System.ComponentModel;
using FubuCore.CommandLine;

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

        [Description("The culture info name used by default (en-US)")]
        public string DefaultCulture { get; set; }
    }


    [CommandDescription("Transform .resx files into localized fubu .locale files")]
    public class TransformCommand : FubuCommand<FolderInput>
    {
        public override bool Execute(FolderInput input)
        {
            Console.WriteLine("{0} : {1}", "Source", input.Source);
            Console.WriteLine("{0} : {1}", "Target", input.Target);
            Console.WriteLine("{0} : {1}", "DefaultCulture", input.DefaultCulture);
            return true;
        }
    }
}