﻿using DogeBeats.EngineSections.Resources;
using DogeBeats.Model;
using DogeBeats.Modules.Music;
using DogeBeats.Other;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats.Modules.MusicPlayer
{
    public class ProxySoundManager
    {
        //public static NDictionary<string, SoundPlayer> soundPlayers = new NDictionary<string, SoundPlayer>();


        public static SoundItem PlaySound(string trackName)
        {
            MemoryStream ms = GetSoundMemoryStream(trackName);
            SoundPlayer player = new SoundPlayer(ms);
            player.Play();
            SoundItem track = new SoundItem();
            track.Play();
            return track;
        }

        //TODO: Tests
        //public static SoundTrack AdvPlay(string trackName)
        //{
        //    IWavePlayer waveOutDevice = new WaveOut();
        //    WaveStream mainOutputStream;
        //    WaveChannel32 volumeStream;

        //    MemoryStream ms = GetSoundMemoryStream(trackName);
        //    WaveStream wmaReader = ParseToMP3WaveStream(ms);
        //    byte[] soundBytes = CenterResource.GetResource("Music", trackName);
        //    wmaReader.Write(soundBytes, 0, soundBytes.Length);

        //    volumeStream = new WaveChannel32(wmaReader);

        //    mainOutputStream = volumeStream;

        //    mainOutputStream.Skip(30); //start 30 seconds into the file
        //    waveOutDevice.Init(mainOutputStream);
        //    waveOutDevice.Play();
        //    //mainOutputStream.
        //    return null;
        //}

        private static WaveStream ParseToMP3WaveStream(MemoryStream ms)
        {
            using (var mp3FileReader = new Mp3FileReader(ms))
            {
                return mp3FileReader;
            }
        }

        private static MemoryStream GetSoundMemoryStream(string resourceName)
        {
            byte[] bytes = StaticHub.ResourceManager.GetResource("Music", resourceName);
            if (bytes == null || bytes.Length == 0)
                return null;

            MemoryStream ms = new MemoryStream();
            ms.Write(bytes, 0, bytes.Length);
            return ms;
        }

        //public static void StopMusic(string trackName)
        //{
        //    SoundPlayer player = soundPlayers[trackName];
        //    if (player != null)
        //        player.Stop();
        //    soundPlayers.Remove();//fck
        //}

        //public static void PasueMusic(string trackName)
        //{
        //    SoundPlayer player = soundPlayers[trackName];
        //    if (player != null)
        //        player.Stop();
        //}
    }
}
