using DogeBeats.EngineSections.Shared;
using DogeBeats.Modules.TimeLines;
using DogeBeats.Other;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Testowy.Model
{
    //struct candidate
    public class AnimationRouteFrame : ITLEPanelCellElement
    {
        public float Amplitude { get; set; }

        public int Cycles { get; set; }

        public float SpeedAmplitude { get; set; }

        public float SpeedPhase { get; set; }

        public float SpeedCycles { get; set; }

        public TimeSpan FrameTime { get; set; }

        public Placement CheckpointPosition { get; set; }

        public static TimeSpan DurationTime { get; set; }

        static AnimationRouteFrame()
        {
            DurationTime = new TimeSpan(0, 0, 0, 0, 50);
        }

        public TimeSpan GetDurationTime()
        {
            return FrameTime;
        }

        public TimeSpan GetStartTime()
        {
            return FrameTime;
        }

        public void SetStartTime(TimeSpan timeSpan)
        {
            FrameTime = timeSpan;
        }

        internal static IEnumerable<string> GetKeysManualUpdate()
        {
            List<string> keys = new List<string>();
            keys.Add("Amplitude");
            keys.Add("Cycles");
            keys.Add("SpeedAmplitude");
            keys.Add("SpeedPhase");
            keys.Add("SpeedCycles");
            keys.Add("FrameTime");
            return keys;
        }

        public void ManualUpdate(NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["Amplitude"].ToString()))
                Amplitude = ManualUpdaterParser.ParseFloat(values["Amplitude"]);
            if (!string.IsNullOrEmpty(values["Cycles"].ToString()))
                Cycles = ManualUpdaterParser.ParseInt(values["Cycles"]);
            if (!string.IsNullOrEmpty(values["SpeedAmplitude"].ToString()))
                SpeedAmplitude = ManualUpdaterParser.ParseFloat(values["SpeedAmplitude"]);
            if (!string.IsNullOrEmpty(values["SpeedPhase"].ToString()))
                SpeedPhase = ManualUpdaterParser.ParseFloat(values["SpeedPhase"]);
            if (!string.IsNullOrEmpty(values["SpeedCycles"].ToString()))
                SpeedCycles = ManualUpdaterParser.ParseFloat(values["SpeedCycles"]);
            if (!string.IsNullOrEmpty(values["FrameTime"].ToString()))
                FrameTime = ManualUpdaterParser.ParseTimeSpan(values["FrameTime"]);
        }
    }
}
