using Cletor.Resources.Enums;
using System;

namespace Cletor.Views.Controls
{
    public class DocumentUpdatedEventArgs : EventArgs
    {
        public DocumentUpdatedEventArgs(DocumentState documentState,
            string documentTitle)
        {
            DocumentState = documentState;
            DocumentTitle = documentTitle;
        }

        public DocumentState DocumentState { get; }
        public string DocumentTitle { get; }
    }
}
