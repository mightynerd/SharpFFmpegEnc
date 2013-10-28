SharpFFmpegEnc
==============

An ffmpeg wrapper for .NET languages 

###Introduction
I've wanted a simple ffmpeg wrapper to use in some of my projects. I could't find any existing wrapper that suited me so I decided to write my own.

###Usage
```Csharp
VideoEncoder encoder = new VideoEncoder("C:\ffmpeg.exe");

//Add event handlers
encoder.EventLineRead += encoder_EventLineRead;
encoder.EventEncodingProgressChanged += encoder_EventEncodingProgressChanged;
encoder.EventEncodingCompleted += encoder_EventEncodingCompleted;

//Start the encoding
encoder.Encode("-y -i INPUT_PATH -c:v libx264 -crf 24 OUTPUT PATH");
```
