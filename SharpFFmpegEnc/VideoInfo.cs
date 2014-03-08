using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFFmpegEnc
{
    class VideoInfo
    {
        private string videoPath;
        private string videoContainer;
        private int videoBitrate;
        private VideoSize videoSize;
        private string videoCodec;
        private int videoFramerate;

        private string audioCodec;
        private int audioBitrate;
        private int audioRate;



        struct VideoSize
        {
            public int Width
            {
                get { return width; }
                set { width = value; }
            }
            public int Height
            {
                get { return height; }
                set { height = value; }
            }

            public VideoSize(int w, int h)
            {
                width = w;
                height = h;
            }

            private int width;
            private int height;
        }
    }
}
