/*
 * RegexFunctions.cs
 * SharpFFmpegEnc by MightyNerd
 * Licenced under GPL
 * 
 * This class contains static functions that extract encoding
 * information using regular expressions
 * 
 * Regular expressions tested with http://rubular.com/
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SharpFFmpegEnc
{
    static class RegexFunctions
    {
        public static int reGetFrame(string line)
        {
            Match match = Regex.Match(line, "(\\bframe\\b)(\\S)(\\s*)(\\d*)");
            if (match.Success == true)
            {
                int frames = 0;
                Int32.TryParse(match.Groups[4].Value, out frames);
                return frames;
            } 
            else
            {
                return -1;
            }
        }

        public static int reGetFramerate(string line)
        {
            Match match = Regex.Match(line, "(\\bfps\\b)(=)(\\s*)(\\d*)");
            if (match.Success == true)
            {
                int framerate = 0;
                Int32.TryParse(match.Groups[4].Value, out framerate);
                return framerate;
            }
            else
            {
                Debug.WriteLine("FRAMERATE NOT MATCH");
                return -1;
            }
        }

        public static int reGetSize(string line)
        {
            Match match = Regex.Match(line, "(\\bsize\\b)(=)(\\s*)(\\d*)");
            if (match.Success == true)
            {
                int size = 0;
                Int32.TryParse(match.Groups[4].Value, out size);
                return size;
            }
            else
            {
                return -1;
            }
        }

        public static int reGetBirate(string line)
        {
            Match match = Regex.Match(line, "(\\bbitrate\\b)=(\\s*)(\\d*)");
            if (match.Success == true)
            {
                int bitrate = 0;
                Int32.TryParse(match.Groups[3].Value, out bitrate);
                return bitrate;
            }
            else
            {
                Debug.WriteLine("BITRATE NOT MATCH");
                return -1;
            }
        }

        public static TimeSpan reGetTime(string line)
        {
            Match match = Regex.Match(line, "(\\btime\\b)=(\\s*)(\\d*):(\\d*):(\\d*).(\\d*)");
            if (match.Success == true)
            {
                int hh = 0;
                int mm = 0;
                int ss = 0;

                int.TryParse(match.Groups[3].Value, out hh);
                int.TryParse(match.Groups[4].Value, out mm);
                int.TryParse(match.Groups[5].Value, out ss);
                return new TimeSpan(hh, mm, ss);
            }
            else
            {
                return new TimeSpan(0, 0, 0);
            }
        }


        private static void printGroups(GroupCollection gc)
        {
            int i = 0;
            foreach (Group g in gc)
            {
                Debug.WriteLine(i + "-" + g.Value);
                i++;
            }
        }
    }
}
