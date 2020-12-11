using System;
using System.Collections.Generic;

namespace ZeusCaching
{
    /// <summary>
    /// Represents a class that holds caching profiles.
    /// </summary>
    public class ZeusCachingOptions
    {
        private readonly IReadOnlyDictionary<string, ZeusCachingProfileOptions> _profiles;

        public bool IsEnabled { get; set; } = true;

        internal ZeusCachingOptions(IReadOnlyDictionary<string, ZeusCachingProfileOptions> profiles, bool enabled)
        {
            _profiles = profiles;
            IsEnabled = enabled;
        }

        /// <summary>
        /// Checks if the given profile name is defined in this instance of options.
        /// </summary>
        /// <param name="profileName">Specifies the name of the profile to lookup.</param>
        /// <returns></returns>
        public bool ContainsProfile(string profileName)
        {
            return _profiles.ContainsKey(profileName);
        }



        /// <summary>
        /// Finds and returns the profile options instance for given name.
        /// </summary>
        /// <param name="profileName">Specifies the name of the profile.</param>
        /// <returns></returns>
        internal ZeusCachingProfileOptions GetOptions(string profileName)
        {
            if (!ContainsProfile(profileName))
            {
                throw new ArgumentException("Profile not found.", nameof(profileName));
            }

            return _profiles[profileName];
        }
    }
}
