using Cletor.Resources;
using Cletor.Resources.Enums;
using Cletor.Resources.Languages;
using Cletor.Views.Helpers;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace Cletor.Views.Controls
{
    public class StateFullTextEditor : SfRichTextBoxAdv
    {
        private string _documentTitle;

        #region Constructor

        public StateFullTextEditor() : this(false) { }
        public StateFullTextEditor(bool isTest)
        {
            if (isTest)
                OnLoaded(this, new RoutedEventArgs());
            else
                Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CacheChanges();
            StateFullDocument = new StateFullDocument(TemporalFilePath, this);

            _documentTitle = DocumentTitle;
            DocumentSaved += OnDocumentSaved;

            DocumentStyles.Update(Document.Styles);
        }

        #endregion

        #region Properties

        public string ViewDocumentTitle => string.IsNullOrWhiteSpace(DocumentTitle) ?
                UIText.DefaultDocumentTitle :
                DocumentTitle;

        public StateFullDocument StateFullDocument { get; private set; }

        public string TemporalFilePath
        {
            get
            {
                var documentTitle = (ViewDocumentTitle.Contains(Constants.TempFileExtension))
                    ? ViewDocumentTitle
                    : ViewDocumentTitle + $"{Constants.TempFileExtension}{Constants.HtmlFileExtension}";

                return $"{ConfigurationHandler.Current.TemporalFilesPath}{documentTitle}";
            }
        }

        #endregion

        #region Document State Events

        public event EventHandler<DocumentUpdatedEventArgs> DocumentStateChanged;

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name != nameof(DocumentTitle) ||
                StateFullDocument is null ||
                _documentTitle == DocumentTitle)
                return;

            _documentTitle = DocumentTitle;

            if (DocumentTitle.Contains(Constants.TempFileExtension))
                return;

            CacheChanges();
            DocumentStyles.Update(Document.Styles);
            StateFullDocument.NewDocument(TemporalFilePath);

            var args = new DocumentUpdatedEventArgs(StateFullDocument.State, ViewDocumentTitle);
            DocumentStateChanged?.Invoke(this, args);
        }

        protected virtual void OnDocumentSaved(object sender, DocumentSavedEventArgs e)
        {
            CacheChanges();
            StateFullDocument.UpdateDocument(TemporalFilePath);

            var args = new DocumentUpdatedEventArgs(StateFullDocument.State, ViewDocumentTitle);
            DocumentStateChanged?.Invoke(this, args);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (StateFullDocument.State == DocumentState.Unsaved)
                return;

            CacheChanges();
            if (StateFullDocument.HasChanged(TemporalFilePath))
            {
                var args = new DocumentUpdatedEventArgs(StateFullDocument.State, _documentTitle);
                DocumentStateChanged?.Invoke(this, args);
            }
        }

        #endregion

        #region Cache

        public void CacheChanges(string tempFilePath = null)
        {
            var converter = new SfDocumentToHtml();
            var html = converter.Convert(Document);
            if (string.IsNullOrWhiteSpace(html))
                return;

            var path = tempFilePath ?? TemporalFilePath;
            File.WriteAllText(path, html);
        }

        public void CleanCache()
        {
            if (File.Exists(TemporalFilePath))
                File.Delete(TemporalFilePath);
        }

        public string GetTempFile()
        {
            CacheChanges();
            var temporalFiles = File.ReadAllText(TemporalFilePath);

            return temporalFiles;
        }

        #endregion
    }
}
