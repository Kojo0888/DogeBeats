using NAudio.Wave;

namespace DogeBeats.Modules.MusicPlayer
{
    internal class WMAFileReader : WaveStream
    {
        private string v;

        public WMAFileReader(string v)
        {
            this.v = v;
        }
    }
}