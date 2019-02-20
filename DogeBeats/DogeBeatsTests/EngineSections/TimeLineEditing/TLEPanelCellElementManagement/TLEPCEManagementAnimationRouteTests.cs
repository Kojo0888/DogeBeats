using DogeBeats.EngineSections.TimeLineEditing.TLEPanelCellElementManagement;
using DogeBeats.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DogeBeatsTests.EngineSections.TimeLineEditing.TLEPanelCellElementManagement
{
    public class TLEPCEManagementAnimationRouteTests
    {
        TimeLineEditor editor;
        TLEPCEManagementAnimationRoute Management;

        public TLEPCEManagementAnimationRouteTests()
        {
            editor = new TimeLineEditor();
            Management = new TLEPCEManagementAnimationRoute(editor);
        }

        [Fact]
        public void AddNewElement()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void MoveElement()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UpdateElement()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void RemoveElement()
        {
            throw new NotImplementedException();
        }
    }
}
