using DogeBeats.EngineSections.Shared;
using DogeBeats.Modules.Music;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.Resources.Centres
{
    public class CentreFileBase<T> : ICentreBase<T> where T : IByteParsable, INamedElement, new()
    {
        public string ResourceType { get; set; }

        public DDictionary<string, T> CentreElements { get; set; }

        public CentreFileBase(string type)
        {
            ResourceType = type;
        }

        public void LoadAll()
        {
            CentreElements = new DDictionary<string, T>();

            var elements = StaticHub.ResourceManager.GetAllResources(ResourceType);
            foreach (var element in elements)
            {
                T si = new T();
                si.LoadBytes(element.Value);
                si.Name = element.Key;
                CentreElements.Add(element.Key, si);
            }
        }

        public void SaveAll()
        {
            Dictionary<string, byte[]> toUpdate = new Dictionary<string, byte[]>();
            foreach (var CentreElement in CentreElements)
            {
                toUpdate.Add(CentreElement.Key, CentreElement.Value.GetBytes());
            }

            StaticHub.ResourceManager.Save(ResourceType, toUpdate);
        }
        //to do rest

        public void Save(string name, T obj)
        {
            if (!CentreElements.ContainsKey(name))
                CentreElements.Add(name, obj);
            else if (!ReferenceEquals(obj, CentreElements[name]))
                CentreElements[name] = obj;

            byte[] bytes = obj.GetBytes();

            StaticHub.ResourceManager.Save(ResourceType, new Dictionary<string, byte[]>() { { name, bytes } });
        }

        public void Save(string name, byte[] bytes)
        {
            T obj = new T();
            obj.LoadBytes(bytes);
            obj.Name = name;

            if (!CentreElements.ContainsKey(name))
                CentreElements.Add(name, obj);
            else if (!ReferenceEquals(obj, CentreElements[name]))
                CentreElements[name] = obj;

            StaticHub.ResourceManager.Save(ResourceType, new Dictionary<string, byte[]>() { { name, bytes } });
        }

        public T Get(string name)
        {
            if (CentreElements.ContainsKey(name))
                return CentreElements[name];
            else
                return default(T);
        }

        public DDictionary<string, T> GetAll()
        {
            return CentreElements;
        }
    }
}
