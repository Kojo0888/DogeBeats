using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.TimeLines
{
    public interface ITLEPanelCellElement
    {
        TimeSpan GetDurationTime();

        TimeSpan GetStartTime();

        void SetStartTime(TimeSpan timeSpan);

        void SetDefaultData();

        void UpdateManual(NameValueCollection values);

        List<string> GetKeysUpdateManual();
    }
}
