using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Testowy.Model;

namespace DogeBeats.Model
{
    public static class CenterTimeLine
    {
        public static readonly string TIMELINE_FOLDER = @"TimeLines";

        public static List<TimeLine> TimeLines { get; set; } = new List<TimeLine>();

        public static void LoadAllTimeLines()
        {
            VerifyFolderExistance();

            string[] files = Directory.GetFiles(TIMELINE_FOLDER);
            foreach (var file in files)
            {
                TimeLine timeline = LoadTimeLine(file);
                if(timeline != null)
                    TimeLines.Add(timeline);
            }
        }

        private static void VerifyFolderExistance()
        {
            if (!Directory.Exists(TIMELINE_FOLDER))
                Directory.CreateDirectory(TIMELINE_FOLDER);
        }

        private static TimeLine LoadTimeLine(string file)
        {
            string data = File.ReadAllText(file);
            XmlSerializer serializer = new XmlSerializer(typeof(TimeLine));
            
            using (TextReader tr = new StreamReader(file))
            {
                TimeLine timeline = serializer.Deserialize(tr) as TimeLine;
                return timeline;
            }
        }

        public static void SaveAllTimeLines()
        {
            foreach (var timeline in TimeLines)
            {
                SaveTimeLine(timeline);
            }
        }

        public static void SaveTimeLine(TimeLine timeline)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TimeLine));

            using (TextWriter tw = new StreamWriter(timeline.TimeLineName))
            {
                serializer.Serialize(tw, timeline);
            }
        }

        public static List<AnimationGroupElement> GetAllAnimationGroupElements()
        {
            return TimeLines.SelectMany(s => s.AnimationGroupElements).ToList();
        }
    }
}
