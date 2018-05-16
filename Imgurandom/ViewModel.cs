using System;
using System.ComponentModel;

namespace Imgurandom
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        // Misc. oddities needed for certain methods.
        private string _Alphanumericals = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private Random _RNG = new Random();

        // Hook up buttons to their Functions
        public ViewModel(MainWindow win)
        {
            // Misc buttons
            win.CopyButton.Click += CopyImageURLToClipboard;
            win.SaveButton.Click += SaveImage;
            win.OpenAsGif.Click += OpenAsGif;

            // Navigation
            win.ForwardButton.Click += IncrementIndex;
            win.BackwardButton.Click += DecrementIndex;
            win.GenerateButton.Click += FindNextImage;

            // Convenience
            win.ImageViewer.MouseLeftButtonDown += FindNextImage;
            win.ImageViewer.MouseRightButtonDown += CopyImageURLToClipboard;
        }

        // PROPERTY CHANGED REQUIRED ITEMS
        public event PropertyChangedEventHandler PropertyChanged;
        private void Changed(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        // =====
    }
}
