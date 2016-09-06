using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Plugin.Localization.Abstractions;

namespace Plugin.Localization
{
    public partial class LocalizationImplementation : ILocalization
    {
        private string FileLoad(string path)
        {
            return ReadFile(path);
        }

        private string ReadFile(string path)
        {
            var result = ReadFileAsync(path).Result;
            return result;
        }

        private async Task<string> ReadFileAsync(string path)
        {
            // Get the local folder.
            var local = ApplicationData.Current.LocalFolder;

            if(local != null)
            {
                // Get the DataFolder folder.
                var dataFolder = await local.GetFolderAsync(string.Empty);

                // Get the file.
                var file = await dataFolder.OpenStreamForReadAsync(path);

                // Read the data.
                using(var streamReader = new StreamReader(file))
                {
                    return streamReader.ReadToEnd();
                }
            }

            return string.Empty;
        }
    }
}