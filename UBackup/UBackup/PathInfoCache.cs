using System;
using System.Collections.Generic;

namespace UBackup
{
    public class PathInfoCache
    {
        private readonly ILogger _logger;
        public readonly List<PathInfo> List;

        public PathInfoCache(ILogger logger)
        {
            _logger = logger;
            List = new List<PathInfo>();
        }

        public bool FindAndComapre(PathInfo pathInfo)
        {
            try
            {
                foreach (var info in List)
                {
                    if (!string.Equals(info.Path, pathInfo.Path, StringComparison.CurrentCultureIgnoreCase)) continue;
                    if (info.LastModified == pathInfo.LastModified)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.AddError($"PathInfoCache >>> FindAndComapre:", ex);
            }

            return false;
        }
    }
}