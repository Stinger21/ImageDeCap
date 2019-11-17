using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using FFmpeg.NET;
using MediaToolkit;
using MediaToolkit.Model;
using System.IO;

namespace imageDeCap
{
    public static class SoundRecording
    {
        static WasapiLoopbackCapture CaptureInstance;
        public static string WavPath = "";
        public static string Mp3PathUntrimmed = "";
        public static string Mp3Path = "";
        public static void Start()
        {
            WavPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\imageDeCap\out.wav";
            Mp3PathUntrimmed = WavPath.Replace(".wav", ".mp3");
            Mp3Path = WavPath.Replace(".wav", "_trimmed.mp3");
            CaptureInstance = new WasapiLoopbackCapture();
            WaveFileWriter RecordedAudioWriter = new WaveFileWriter(WavPath, CaptureInstance.WaveFormat);
            CaptureInstance.DataAvailable += (s, a) =>
            {
                RecordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
            };
            
            CaptureInstance.RecordingStopped += (s, a) =>
            {
                RecordedAudioWriter.Dispose();
                RecordedAudioWriter = null;
                CaptureInstance.Dispose();
            };

            CaptureInstance.StartRecording();
        }

        public static void Stop()
        {
            CaptureInstance.StopRecording();

            ProgressWindow w = new ProgressWindow();
            w.SetProgress($"Compressing Audio...", 50, 100);
            var inputFile = new MediaFile { Filename = WavPath };
            var outputFile = new MediaFile { Filename = Mp3PathUntrimmed };
            
            using (var engine = new Engine())
            {
                engine.Convert(inputFile, outputFile); // wav to mp3
            }
            w.Close();
        }
    }
}
