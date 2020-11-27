using Cletor.Views.Controls;
using Cletor.Views.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Cletor.Tests
{
    [TestClass]
    public class StateFullTextEditorShould
    {
        private StateFullTextEditor _cut;
        private string _tempFilePath;

        [TestInitialize]
        public void InitializeTest()
        {
            _cut = new StateFullTextEditor(true);
            _tempFilePath = ConfigurationHandler.Current.TemporalFilesPath +
                            Constants.TemporalDocumentName;
        }

        [TestMethod]
        public void CreateNewDocument()
        {
            var document = _cut.StateFullDocument;

            document.Should().NotBeNull();
        }

        [TestMethod]
        public void ChangeDocument()
        {
            _cut.Selection.InsertText(Constants.HelloWorldText);
            var areEquals = _cut.StateFullDocument.HasChanged(_tempFilePath);

            areEquals.Should().BeFalse();
        }

        [TestMethod]
        public void SaveDocument()
        {
            var fileName = Constants.NewTemporalDocumentName;

            _cut.Selection.InsertText(Constants.HelloWorldText);
            _cut.Save(fileName);
            var hasChanges = _cut.StateFullDocument.HasChanged(_tempFilePath);

            File.Delete(fileName);
            hasChanges.Should().BeFalse();
        }
    }
}
