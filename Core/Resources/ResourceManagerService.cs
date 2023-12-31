using Core.Core;
using Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Core.Resources
{
    public class ResourceManagerService<TResource> : IResourceManagerService
    {
        private readonly ResourceManager _resourceManager;

        public ResourceManagerService()
        {
            _resourceManager = new ResourceManager(typeof(TResource));
        }

        public string GetString(string key)
        {
            
            return _resourceManager.GetString(key) ?? $"Key '{key}' not found.";
        }
    }
}
