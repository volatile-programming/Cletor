using Cletor.Views.Controls;
using Cletor.Views.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Cletor.Tests
{
    [TestClass]
    public class StateFullDocumentShould
    {
        private StateFullDocument _cut;
        private string _tempFilePath;

        [TestInitialize]
        public void InitializeTest()
        {
            var editor = new StateFullTextEditor(true);

            _cut = editor.StateFullDocument;
            _tempFilePath = ConfigurationHandler.Current.TemporalFilesPath +
                            Constants.TemporalDocumentName;
        }

        [TestMethod]
        public void GuardAgainstNull()
        {
            Action initialize = () => _ = new StateFullDocument(null, null);

            initialize.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void CacheNewDocument()
        {
            var areEquals = _cut.HasChanged(_tempFilePath);

            areEquals.Should().BeFalse();
        }

        [TestMethod]
        public void CompareDocumentHasChanged()
        {
            if (File.Exists(_tempFilePath))
                File.Delete(_tempFilePath);

            var editor = new StateFullTextEditor(true);
            var cut = editor.StateFullDocument;

            editor.Selection.InsertText(Constants.HelloWorldText);
            editor.CacheChanges(_tempFilePath);
            var hasChanges = cut.HasChanged(_tempFilePath);

            hasChanges.Should().BeTrue();
        }
    }
}
