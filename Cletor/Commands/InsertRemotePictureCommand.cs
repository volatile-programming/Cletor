using Cletor.Models;
using Syncfusion.Windows.Controls.RichTextBoxAdv;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Cletor.Commands
{
    public class InsertRemotePictureCommand : RelayCommand
    {
        private readonly MainWindow _currentWindow;
        private readonly List<RemoteImage> _images;

        public InsertRemotePictureCommand(MainWindow currentWindow,
            List<RemoteImage> images) : base(execute: null)
        {
            _currentWindow = currentWindow;
            _images = images;
            _execute = InsertRemotePicture;
        }

        private async void InsertRemotePicture()
        {
            var remoteImage = await Views.Controls.CustomDialog
                .Show<RemoteImage>(_currentWindow, "Insert Remote Image");
            if (remoteImage is null || string.IsNullOrWhiteSpace(remoteImage.Url))
                return;

            var image = new BitmapImage(new Uri(remoteImage.Url));

            SfRichTextBoxAdv.InsertPictureCommand?.Execute(image,
                _currentWindow.TextEditor);
        }
    }
}
