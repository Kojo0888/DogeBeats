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
    public class CentreSerializationBase<T> : ICentreBase<T> where T : class, INamedElement, new()
    {
        public string ResourceType { get; set; }

        public DDictionary<string, T> CentreElements { get; set; } = new DDictionary<string, T>();

        public CentreSerializationBase(string type)
        {
            ResourceType = type;
            CentreElements = new DDictionary<string, T>();
        }

        public void LoadAll()
        {
            CentreElements = StaticHub.ResourceManager.GetAllOfSerializedObjects<T>(ResourceType);
        }

        public void SaveAll()
        {
            StaticHub.ResourceManager.SaveAllOfSerializedObjects<T>(ResourceType, CentreElements);
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

            StaticHub.ResourceManager.SaveAllOfSerializedObjects(ResourceType, new Dictionary<string, T>() { { name, obj } });
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

        public T CreateElement(string name)
        {
            T element = new T();
            if (!string.IsNullOrEmpty(name))
            {
                if (CentreElements.ContainsKey(name))
                    name = name + " _ " + new Random().Next();
                element.Name = name;
                CentreElements.Add(name, element);
            }
            return element;
        }

        public T RenameElement(T element, string oldName, string newName)
        {
            if (!string.IsNullOrEmpty(oldName))
            {
                if (CentreElements.Keys.Contains(oldName))
                    CentreElements.Remove(oldName);
            }

            if (!string.IsNullOrEmpty(newName))
            {
                if (CentreElements.ContainsKey(newName))
                    newName = newName + " _ " + new Random().Next();
                element.Name = newName;
                CentreElements.Add(newName, element);
            }
            return element;
        }
    }
}
