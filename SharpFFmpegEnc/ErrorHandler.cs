using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpFFmpegEnc
{
    class ErrorHandler
    {
        public static bool CheckForErrors(string line)
        {
            string errorList = SharpFFmpegEnc.Properties.Resources.errors;
            string[] errorArray = errorList.Split('\n');
            foreach (string error in errorArray)
            {
                if (line.Contains(error.Replace("\r", "")))
                {
                    //Error found
                    //throw new Exception("ffmpeg error: " + line);
                    return false;
                }
            }

            return false;
        }
    }
}
