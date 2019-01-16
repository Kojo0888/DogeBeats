using DogeBeats.EngineSections.Shared;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.Resources.Centres
{
    public class CentreFileBase<T> : ICentreBase<byte[]>
    {
        public string ResourceType { get; set; }

        public DDictionary<string, byte[]> CentreElements { get; set; }

        public void LoadAll()
        {
            CentreElements = StaticHub.ResourceManager.GetAllResources(ResourceType);
        }

        public void SaveAll()
        {
            StaticHub.ResourceManager.Save(ResourceType, CentreElements);
        }

        public void Save(string name, byte[] bytes)
        {
            if (!CentreElements.ContainsKey(name))
                CentreElements.Add(name, bytes);
            else if (!ReferenceEquals(bytes, CentreElements[name]))
                CentreElements[name] = bytes;

            StaticHub.ResourceManager.Save(ResourceType, new Dictionary<string, byte[]>() { { name, bytes } });
        }

        public T Get(string name)
        {
            if (CentreElements.ContainsKey(name))
                return CentreElements[name];
            else
                return null;
        }

        public DDictionary<string, T> GetAll()
        {
            return CentreElements;
        }
    }
}
