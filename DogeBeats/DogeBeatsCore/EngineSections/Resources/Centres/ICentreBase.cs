using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.Resources.Centres
{
    interface ICentreBase<T>
    {
        string ResourceType { get; set; }
        DDictionary<string, T> CentreElements { get; set; }
        void LoadAll();
        void Save(string name, T data);
        void SaveAll();
        T Get(string name);
        DDictionary<string, T> GetAll();

    }
}
