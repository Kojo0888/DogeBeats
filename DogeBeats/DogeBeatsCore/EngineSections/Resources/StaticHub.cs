﻿using DogeBeats.EngineSections.Resources.Centres;
using DogeBeats.EngineSections.Shared;
using DogeBeats.Model;
using DogeBeats.Modules.Centers;
using DogeBeats.Modules.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.EngineSections.Resources
{
    public static class StaticHub
    {
        public static TrigonometricCache TrigonometricCache { get; set; } = new TrigonometricCache("sin", 0.0001);

        public static ResourceManager ResourceManager { get; set; } = new ResourceManager();

        public static TimeLineCentre TimeLineCentre { get; set; } = new TimeLineCentre();

        public static CentreFileBase<SoundItem> SoundCentre { get; set; } = new CentreFileBase<SoundItem>("Sounds");

        public static CentreFileBase<ImageItem> ImageCentre { get; set; } = new CentreFileBase<ImageItem>("Images");

        //public static ManualUpdater ManualUpdater { get; set; } = new ManualUpdater();
    }
}
