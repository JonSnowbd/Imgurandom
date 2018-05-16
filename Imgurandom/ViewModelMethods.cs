using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;

namespace Imgurandom
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Takes the current image URL and saves the corresponding Image into a folder called Imgurandom in your Pictures.
        /// </summary>
        /// <param name="_s">Unused. Merely here to fulfill delegate parameters.</param>
        /// <param name="_e">Unused. Merely here to fulfill delegate parameters.</param>
        public void SaveImage(object _s, object _e)
        {
            string p = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Imgurandom");
            Directory.CreateDirectory(p); // Wont do anything if already exists.
            string filename = CurrentImageURL.Replace("https://i.imgur.com/", "").Replace(".png", "").Replace("gifv", "");
            p = Path.Combine(p, filename + ".png");
            try
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(CurrentImageURL, p);
                    MessageBox.Show("Download complete. It will be in MyPictures/Imgurandom", "That image", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch
            {
                MessageBox.Show("Something went wrong. Are you running this program in admin mode?", "That image", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Takes the current image URL and opens it as a gifv in your browser, since this does not support GIFV 
        /// </summary>
        /// <param name="_s">Unused. Merely here to fulfill delegate parameters.</param>
        /// <param name="_e">Unused. Merely here to fulfill delegate parameters.</param>
        public void OpenAsGif(object _s, object _e)
        {
            System.Diagnostics.Process.Start(CurrentImageURL.Replace(".png", ".gifv"));
        }

        /// <summary>
        /// Takes a url(string) and determines whether or not it points to a valid IMGUR Image.
        /// </summary>
        /// <param name="url">The proper, full, https, url pointing at the direct image link. EG: https://i.imgur.com/0qsmNQT.png </param>
        /// <returns>Whether or not the image points to a valid IMGUR Image, as a bool</returns>
        public bool DoesImageExist(string url)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse res;
                req.Method = "HEAD";
                req.AllowAutoRedirect = false;
                using (res = (HttpWebResponse)req.GetResponse())
                {
                    Console.WriteLine(res.StatusCode);
                    if (res.StatusCode != HttpStatusCode.OK || res.StatusCode == HttpStatusCode.Found)
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Takes a URL and add its to the URLHistory. If the History is above 10, delete the earliest URL.
        /// </summary>
        /// <param name="url">The new URL to be added to URLHistory.</param>
        public void AddNewURL(string url)
        {
            URLHistory.Add(url);
            if (URLHistory.Count > 10)
            {
                URLHistory.RemoveAt(0);
            }
            Index = URLHistory.Count - 1;
        }

        /// <summary>
        /// Convenience method to go forward an index, making sure to not go out of array index bounds.
        /// </summary>
        /// <param name="_s">Unused. Merely here to fulfill delegate parameters.</param>
        /// <param name="_e">Unused. Merely here to fulfill delegate parameters.</param>
        public void IncrementIndex(object _s, object _e)
        {
            try
            {
                if (Index + 2 > URLHistory.Count)
                    return;
                Index++;
                Changed("Index");
            }
            catch
            {
                // Failing this on one off bugs is not harmful, if even possible. Just exit out.
                return;
            }
        }

        /// <summary>
        /// Convenience method to go backward an index, making sure to not go out of array index bounds.
        /// </summary>
        /// <param name="_s">Unused. Merely here to fulfill delegate parameters.</param>
        /// <param name="_e">Unused. Merely here to fulfill delegate parameters.</param>
        public void DecrementIndex(object _s, object _e)
        {
            try
            {
                if (Index - 1 < 0)
                    return;
                Index--;
                Changed("Index");
                Changed("CurrentIndex");
            }
            catch
            {
                // Failing this on one off bugs is not harmful, if even possible. Just exit out.
                return;
            }

        }

        /// <summary>
        /// This takes the currently selected Image URL (by using the index on URLHistory) and copies it
        /// to the windows clip board
        /// </summary>
        /// <param name="_s">Unused. Merely here to fulfill delegate parameters.</param>
        /// <param name="_e">Unused. Merely here to fulfill delegate parameters.</param>
        public void CopyImageURLToClipboard(object _s, object _e)
        {
            Clipboard.SetText(CurrentImageURL);
        }

        /// <summary>
        /// This creates a url many times until it finds a valid image then sets it as the new current image url.
        /// </summary>
        /// <param name="_s">Unused. Merely here to fulfill delegate parameters.</param>
        /// <param name="_e">Unused. Merely here to fulfill delegate parameters.</param>
        public void FindNextImage(object _s, object _e)
        {
            for(int j = 0; j < 20; j++)
            {
                string new_url = "https://i.imgur.com/";
                int letters = _RNG.Next(5, 7);

                for (int i = 0; i < letters; i++)
                {
                    new_url += _Alphanumericals[_RNG.Next(_Alphanumericals.Length)];
                }

                new_url += ".png";

                if (DoesImageExist(new_url))
                {
                    AddNewURL(new_url);
                    return;
                }
            }
            AddNewURL("https://i.imgur.com/HI137JZ.png");
        }
    }
}
