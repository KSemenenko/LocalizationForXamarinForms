using System;
using Plugin.Localization.Abstractions;


namespace Plugin.Localization
{
    /// <summary>
    /// Cross platform Plugin.Localization implemenations
    /// </summary>
    public class Localization
    {
        private static readonly Lazy<ILocalization> Implementation = 
            new Lazy<ILocalization>(CreatePluginLocalization, System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static ILocalization Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        private static ILocalization CreatePluginLocalization()
        {
            return new LocalizationImplementation();
        }

        static Exception NotImplementedInReferenceAssembly()
        {
            return
                new NotImplementedException(
                    "This functionality is not implemented in the portable version of this assembly.  " +
                    "You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}

