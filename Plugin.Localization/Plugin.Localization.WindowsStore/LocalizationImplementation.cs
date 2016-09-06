using System;
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

        public string ReadFile(string path)
        {
            var result = ReadFileAsync(path).Result;
            return result;
        }

        public async Task<string> ReadFileAsync(string path)
        {
            try
            {
                var folder = ApplicationData.Current.LocalFolder;
                var sampleFile = await folder.GetFileAsync(path);
                return await FileIO.ReadTextAsync(sampleFile);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}