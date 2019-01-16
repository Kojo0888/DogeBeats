using DogeBeats.EngineSections.Resources.Centres;
using DogeBeats.EngineSections.Shared;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.Resources
{
    public class CentreSerializationBase<T> : ICentreBase<T> where T : class, INamedElement
    {
        public string ResourceType { get; set; }

        public DDictionary<string, T> CentreElements { get; set; }

        public void LoadAll()
        {
            CentreElements = StaticHub.ResourceManager.GetAllOfSerializedObjects<T>(ResourceType);
        }

        public void SaveAll()
        {
            StaticHub.ResourceManager.SetAllOfSerializedObjects<T>(ResourceType, CentreElements);
        }

        public void Save(T obj)
        {
            Save(obj.Name, obj);
        }

        public void Save(string name, T obj)
        {
            if (!CentreElements.ContainsKey(name))
                CentreElements.Add(name, obj);
            else if (!ReferenceEquals(obj, CentreElements[name]))
                CentreElements[name] = obj;

            StaticHub.ResourceManager.SetAllOfSerializedObjects(ResourceType, new Dictionary<string, T>() { { name, obj } });
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
