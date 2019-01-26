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
            keys.Add("CheckpointPosition.X");
            keys.Add("CheckpointPosition.Y");
            keys.Add("CheckpointPosition.Width");
            keys.Add("CheckpointPosition.Height");
            keys.Add("CheckpointPosition.Rotation");
            return keys;
        }

        public void ManualUpdate(NameValueCollection values)
        {
            if (!string.IsNullOrEmpty(values["Amplitude"].ToString()))
                Amplitude = ManualUpdaterParser.ParseFloat(values["Ease"]);
            if (!string.IsNullOrEmpty(values["Cycles"].ToString()))
                Cycles = ManualUpdaterParser.ParseInt(values["RunningTime"]);
            if (!string.IsNullOrEmpty(values["SpeedAmplitude"].ToString()))
                SpeedAmplitude = ManualUpdaterParser.ParseFloat(values["SpeedAmplitude"]);
            if (!string.IsNullOrEmpty(values["SpeedPhase"].ToString()))
                SpeedPhase = ManualUpdaterParser.ParseFloat(values["SpeedPhase"]);
            if (!string.IsNullOrEmpty(values["SpeedCycles"].ToString()))
                SpeedCycles = ManualUpdaterParser.ParseFloat(values["SpeedCycles"]);
            if (!string.IsNullOrEmpty(values["FrameTime"].ToString()))
                FrameTime = ManualUpdaterParser.ParseTimeSpan(values["FrameTime"]);
            if (!string.IsNullOrEmpty(values["CheckpointPosition.X"].ToString()))
                CheckpointPosition.X = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.X"]);
            if (!string.IsNullOrEmpty(values["CheckpointPosition.Y"].ToString()))
                CheckpointPosition.Y = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.Y"]);
            if (!string.IsNullOrEmpty(values["CheckpointPosition.Width"].ToString()))
                CheckpointPosition.Width = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.Width"]);
            if (!string.IsNullOrEmpty(values["CheckpointPosition.Height"].ToString()))
                CheckpointPosition.Height = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.Height"]);
            if (!string.IsNullOrEmpty(values["CheckpointPosition.Rotation"].ToString()))
                CheckpointPosition.Rotation = ManualUpdaterParser.ParseFloat(values["CheckpointPosition.Rotation"]);

        }
    }
}
