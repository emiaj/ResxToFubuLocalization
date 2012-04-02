using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace ResxToFubuLocalization.Core.Test
{
    [TestFixture]
    public class ResxFileFactoryTester : InteractionContext<ResxFileFactory>
    {
        private const string LocalizedResxPath = "some/path/file.es-PE.resx";
        private const string DefaultResxPath = "some/path/file.resx";

        private ResxFile _localizedResxFile;
        private ResxFile _defaultResxFile;

        protected override void beforeEach()
        {
            MockFor<IFileSystem>()
                .Stub(x => x.ReadStringFromFile(LocalizedResxPath))
                .Return(@"
<root>
    <data name=""one"">
        <value>uno</value>
    </data>
    <data name=""two"">
        <value>dos</value>
    </data>
    <data name=""three"">
        <value>tres</value>
    </data>
    <data name=""four"">
        <value>cuatro</value>
    </data>
</root>");

            MockFor<IFileSystem>()
                .Stub(x => x.ReadStringFromFile(DefaultResxPath))
                .Return(@"
<root>
    <data name=""one"">
        <value>one</value>
    </data>
    <data name=""two"">
        <value>two</value>
    </data>
    <data name=""three"">
        <value>three</value>
    </data>
    <data name=""four"">
        <value>four</value>
    </data>
</root>");
            _localizedResxFile = ClassUnderTest.CreateFrom(LocalizedResxPath);
            _defaultResxFile = ClassUnderTest.CreateFrom(DefaultResxPath);
        }

        [Test]
        public void file_name()
        {
            _localizedResxFile.FileName.ShouldEqual("file.es-PE");
            _defaultResxFile.FileName.ShouldEqual("file");
        }

        [Test]
        public void name()
        {
            _localizedResxFile.Name.ShouldEqual("file");
            _defaultResxFile.Name.ShouldEqual("file");
        }
        [Test]
        public void culture()
        {
            _localizedResxFile.Culture.ShouldEqual("es-PE");
            _defaultResxFile.Culture.ShouldBeNull();
        }

        [Test]
        public void data()
        {
            _localizedResxFile.Data.ShouldHaveCount(4).Select(x => x.Value).Join("-").ShouldEqual("uno-dos-tres-cuatro");
            _defaultResxFile.Data.ShouldHaveCount(4).Select(x => x.Value).Join("-").ShouldEqual("one-two-three-four");
        }

        [Test]
        public void extension()
        {
            _localizedResxFile.Extension.ShouldEqual(".resx");
            _defaultResxFile.Extension.ShouldEqual(".resx");
        }

    }
}