/*
 * SharpFFmpegEnc by MightyNerd
 * Licenced under LGPL
 * 
 * This is an example program used to test the library
 * 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpFFmpegEncTest
{
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //Create a VideoEncoder and launch it with proper arguments
            //Remember to change the ffmpeg, input and output path!
            SharpFFmpegEnc.VideoEncoder encoder = new SharpFFmpegEnc.VideoEncoder(@"D:\Program\FFmpeg\ffmpeg.exe");

            //Assign a method to each event
            encoder.EventLineRead += encoder_EventLineRead;
            encoder.EventEncodingProgressChanged += encoder_EventEncodingProgressChanged;
            encoder.EventEncodingCompleted += encoder_EventEncodingCompleted;

            encoder.Encode("-y -i INPUT_PATH -c:v libx264 -crf 30 OUTPUT PATH");
        }

        private void encoder_EventEncodingCompleted(int exitCode, string fullOutput)
        {
            //Display the exit code and save the output in a file
            MessageBox.Show("ffmpeg exited\nExit code: " + exitCode + "\nFull output saved as output.txt");
            System.IO.File.WriteAllText(Environment.CurrentDirectory + System.IO.Path.DirectorySeparatorChar + "output.txt", fullOutput);
        }

        private void encoder_EventEncodingProgressChanged(int frame, int frameRate, int size, TimeSpan time, int bitrate)
        {
            //You need to use cross thread calls since the events are called from a separate thread
            if (txtStatus.InvokeRequired == true)
            {
                txtStatus.Invoke(new Action<int, int, int, TimeSpan, int>(encoder_EventEncodingProgressChanged), frame, frameRate, size, time, bitrate);
            } else
            {
                txtStatus.Text = "Frame=" + frame + " Fps=" + frameRate + " Size=" + size + "kB Bitrate=" + bitrate + "kb/s" + " Time=" + time.Hours + ":" + time.Minutes + ":" + time.Seconds;
            }
        }

        private void encoder_EventLineRead(string line)
        {
            if (txtConsole.InvokeRequired == true)
            {
                txtConsole.Invoke(new Action<string>(encoder_EventLineRead), line);
            } else
            {
                txtConsole.Text = txtConsole.Text = txtConsole.Text + ";" + Environment.NewLine + line;
            }
        }

        private void txtConsole_TextChanged(object sender, EventArgs e)
        {
            //Scroll to the bottom
            txtConsole.SelectionStart = txtConsole.Text.Length;
            txtConsole.ScrollToCaret();
        }

        private void formMain_Load(object sender, EventArgs e)
        {

        }
    }
}
