using System;

namespace ZeusCaching.Services
{
    public class ZeusCachingProfileResolver : IZeusCachingProfileResolver
    {
        private readonly ZeusCachingOptions _options;

        public ZeusCachingProfileResolver(ZeusCachingOptions options)
        {
            _options = options;
        }

        public ZeusCachingProfileOptions GetOptions(string profileName)
        {
            if (!_options.ContainsProfile(profileName))
            {
                if (profileName == string.Empty)
                {
                    throw new InvalidOperationException($"No default profile is registered.");
                }
                else
                {
                    throw new InvalidOperationException($"No profile with name '{profileName}' is registered.");
                }
            }

            return _options.GetOptions(profileName);
        }
    }
}
