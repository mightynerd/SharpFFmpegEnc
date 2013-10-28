/*
 * VideoEncoder.cs
 * SharpFFmpegEnc by MightyNerd
 * Licenced under GPL
 * 
 * This is the main class for video encoding using ffmpeg
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace SharpFFmpegEnc
{
    public class VideoEncoder
    {
        //Events
        public delegate void LineRead(string line);
        public event LineRead EventLineRead;

        public delegate void EncodingCompleted(int exitCode, string fullOutput);
        public event EncodingCompleted EventEncodingCompleted;

        public delegate void EncodingProgressChanged(int frame, int frameRate, int size, TimeSpan time, int bitrate);
        public event EncodingProgressChanged EventEncodingProgressChanged;

        //ffmpeg binary path
        private string binPath = "";
        
        public VideoEncoder(string binaryPath)
        {
            //The file must exist
            if (File.Exists(binaryPath))
            {
                binPath = binaryPath;
            }
            else
            {
                throw new Exception("Invalid ffmpeg binary path");
            }
        }

        public void Encode(string arguments)
        {
            //Launch ThreadEncode in a separate thread
            Thread ffmpegThread = new Thread(() => ThreadEncode(arguments));
            ffmpegThread.Start();
        }

        private void ThreadEncode(string args)
        {
            //Create startinfo for the process
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = binPath;
            startInfo.Arguments = args;
            //ffmpeg outputs everything on stderr
            startInfo.RedirectStandardError = true;
            //Needed to redirect stderr
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            Process ffmpeg = new Process();
            ffmpeg.StartInfo = startInfo;
            ffmpeg.Start();

            //Output variables
            string readBuffer = "";
            string fullOutput = "";
            int frame = -1;
            int framerate = -1;
            int size = -1;
            TimeSpan time = TimeSpan.Zero;
            int bitrate = -1;

            //Do this until ffmpeg has exited
            while(ffmpeg.HasExited == false)
            {
                if (ffmpeg.StandardError.EndOfStream == false)
                {
                    //Read stderr into readBuffer
                    readBuffer = ffmpeg.StandardError.ReadLine();
                    //Add the current line into the fullOutput string
                    fullOutput = fullOutput + Environment.NewLine + readBuffer;
                    EventLineRead(readBuffer);
                    
                    //If the current line is an "encoding info" line
                    if (readBuffer.Contains("frame="))
                    {
                        frame = RegexFunctions.reGetFrame(readBuffer);
                        framerate = RegexFunctions.reGetFramerate(readBuffer);
                        size = RegexFunctions.reGetSize(readBuffer);
                        time = RegexFunctions.reGetTime(readBuffer);
                        bitrate = RegexFunctions.reGetBirate(readBuffer);

                    }

                    EventEncodingProgressChanged(frame, framerate, size, time, bitrate);
                }

            }

            //Wait for exit, just in case
            ffmpeg.WaitForExit();

            //ffmpeg exited
            EventEncodingCompleted(ffmpeg.ExitCode, fullOutput);

        }
    }
}
