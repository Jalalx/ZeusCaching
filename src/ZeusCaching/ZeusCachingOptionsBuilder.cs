using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ZeusCaching
{
    /// <summary>
    /// Represents a class that builds <see cref="ZeusCachingOptions"/>.
    /// </summary>
    public class ZeusCachingOptionsBuilder
    {
        internal bool _enabled = true;
        internal readonly Dictionary<string, ZeusCachingProfileOptions> _profiles = new Dictionary<string, ZeusCachingProfileOptions>();

        /// <summary>
        /// Adds the default caching profile.
        /// </summary>
        /// <param name="apply">A delegate that configures the caching profile options.</param>
        /// <returns></returns>
        public ZeusCachingOptionsBuilder AddDefaultProfile(Action<ZeusCachingProfileOptions> apply = null)
        {
            return AddNamedProfile(string.Empty, apply);
        }




        /// <summary>
        /// Adds a named caching profile.
        /// </summary>
        /// <param name="profileName">A name for the caching profile. Names must be unique.</param>
        /// <param name="apply">A delegate that configures the caching profile options.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when profile name is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when profile name is duplicate.</exception>
        public ZeusCachingOptionsBuilder AddNamedProfile(string profileName, Action<ZeusCachingProfileOptions> apply = null)
        {
            if (profileName == null)
            {
                throw new ArgumentNullException(nameof(profileName));
            }

            if (_profiles.ContainsKey(profileName))
            {
                if (string.IsNullOrEmpty(profileName))
                {
                    throw new InvalidOperationException($"ZeusCachingProfileOptionsBuilder.AddDefaultProfile is called more than once.");
                }
                else
                {
                    throw new ArgumentException($"An option with profile name {profileName} already exists.");
                }
            }

            var options = new ZeusCachingProfileOptions();
            if (apply != null)
            {
                apply(options);
            }

            _profiles.Add(profileName, options);

            return this;
        }


        /// <summary>
        /// Disables the caching mechanism globally.
        /// </summary>
        /// <returns></returns>
        public ZeusCachingOptionsBuilder DisableGlobally()
        {
            _enabled = false;
            return this;
        }



        /// <summary>
        /// Enables the caching mechanism globally.
        /// </summary>
        /// <returns></returns>
        public ZeusCachingOptionsBuilder EnableGlobally()
        {
            _enabled = true;
            return this;
        }


        /// <summary>
        /// Builds the <see cref="ZeusCachingOptions" /> instance that contains configured profiles.
        /// </summary>
        /// <returns></returns>
        internal ZeusCachingOptions Build()
        {
            if (_profiles.Count == 0)
            {
                throw new InvalidOperationException("No profile is defined. Please call AddDefaultProfile or AddNamedProfile before building the options.");
            }

            return new ZeusCachingOptions(_profiles, _enabled);
        }
    }
}
