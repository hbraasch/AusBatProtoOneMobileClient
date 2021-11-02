using MP3Sharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeApp.Helpers;

namespace AusBatProtoOneMobileClient.Helpers
{
    public class VuHelper
    {
        public class VuPlayer
        {
            private string mp3FullFilename { get; set; }
            public Channel channel;


            public void Load(string mp3FullFilename)
            {
                const int DB_POINTS_PER_SEC = 10;

                this.mp3FullFilename = mp3FullFilename;

                MP3Stream mp3Stream = new MP3Stream(new FileStream(mp3FullFilename, FileMode.Open, FileAccess.Read));
                mp3Stream.Position = 0;

                #region *// Read into byte array
                var bytes = default(byte[]);
                using (var memstream = new MemoryStream())
                {
                    var buff = new byte[512];
                    var bytesRead = default(int);
                    while ((bytesRead = mp3Stream.Read(buff, 0, buff.Length)) > 0)
                    {
                        memstream.Write(buff, 0, bytesRead);
                    }
                    bytes = memstream.ToArray();
                }
                #endregion

                #region *// Extract channel 0
                channel = new Channel(mp3Stream.Frequency);
                MemoryStream memStream = new MemoryStream(bytes);
                using (BinaryReader reader = new BinaryReader(memStream))
                {
                    for (int i = 0; i < bytes.Length / 4; i++)
                    {
                        var sampleLeft = (float)reader.ReadInt16();
                        var sampleRight = (float)reader.ReadInt16();
                        channel.Samples.Add((sampleLeft + sampleRight)/2);
                    }
                }
                #endregion

                #region *// Calculate decibel
                channel.CalculateDb(DB_POINTS_PER_SEC);
                #endregion

            }

            bool isPlaying = false;



            public void Play(Action<double, bool> updateDisplay)
            {
                if (channel == null) throw new ApplicationException($"File [{mp3FullFilename}] has not been loaded yet");
                Task.Factory.StartNew(async () => {
                    Stopwatch stopWatch = new Stopwatch();
                    var newPlayTime = 100;
                    stopWatch.Start();
                    isPlaying = true;
                    foreach (var db in channel.dBs)
                    {
                        while (stopWatch.ElapsedMilliseconds < newPlayTime) { await Task.Delay(1); }
                        newPlayTime += 100;
                        if (!isPlaying) break;
                        updateDisplay?.Invoke(db, isPlaying);
                    }
                    isPlaying = false;
                    updateDisplay?.Invoke(0, isPlaying);
                });
            }

            internal void Stop()
            {
                isPlaying = false;
            }
        }

        public class Channel
        {

            private int sampleRate;

            public List<float> Samples = new List<float>();
            public List<double> dBs = new List<double>();
            public double maxDb = double.MinValue;
            public double minDb = double.MaxValue;

            public Channel(int sampleRate)
            {
                this.sampleRate = sampleRate;
            }

            internal void CalculateDb(int pointsPerSec)
            {
                var samplesPerSecond = sampleRate;
                int samplesPerChunk = samplesPerSecond / pointsPerSec;
                var chunks = Samples.Chunks(samplesPerChunk);
                maxDb = double.MinValue;
                minDb = double.MaxValue;
                foreach (var chunk in chunks)
                {
                    double sum = 0;
                    foreach (var sample in chunk)
                    {
                        sum += (sample * sample);
                    }
                    double rms = Math.Sqrt(sum / chunk.Count());
                    rms = Math.Max(10e-3, rms);
                    var decibel = 92.8 + 20 * Math.Log10(rms);
                    dBs.Add(decibel);
                    if (decibel > maxDb) maxDb = decibel;
                    if (decibel < minDb) minDb = decibel;
                }
            }


        }
    }
}
