using DogeBeats.Modules;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.TimeLineEditing
{
    public interface ITLEPanelCellElementManagement
    {
        void AddNewElement();
            
        void RemoveElement();

        void MoveElement();

        void UpdateElement(NameValueCollection values);
    }
}
