using Cletor.Resources.Enums;
using System;
using System.IO;

namespace Cletor.Views.Controls
{
    public class StateFullDocument
    {
        private string _cacheDocument;

        public StateFullDocument(string tempFilePath, StateFullTextEditor editor)
        {
            if (string.IsNullOrWhiteSpace(tempFilePath))
                throw new ArgumentNullException(nameof(tempFilePath));

            NewDocument(tempFilePath);
            Editor = editor;
        }

        public DocumentState State { get; private set; }
        public StateFullTextEditor Editor { get; }

        public void NewDocument(string tempFilePath)
        {
            ReloadDocument(tempFilePath);
            State = DocumentState.New;
        }

        protected void ReloadDocument(string tempFilePath)
        {
            if (File.Exists(tempFilePath))
                _cacheDocument = File.ReadAllText(tempFilePath);
            else
            {
                _cacheDocument = string.Empty;
                File.WriteAllText(tempFilePath, _cacheDocument);
            }
        }

        public void UpdateDocument(string tempFilePath)
        {
            ReloadDocument(tempFilePath);
            State = DocumentState.Saved;
        }


        public bool HasChanged(string tempFilePath)
        {
            var tempDocument = File.ReadAllText(tempFilePath);

            var areEquals = _cacheDocument.Equals(tempDocument);

            if (areEquals)
                State = DocumentState.Saved;
            else
                State = DocumentState.Unsaved;


            return !areEquals;
        }
    }
}
