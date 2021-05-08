using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.Wave;
using AltFreeverb;

class Program
{
    static void Main(string[] args)
    {
        var sampleRate = 44100;
        var length = 1 * sampleRate;

        var reverb = new Reverb(length);

        var inputLeft = new float[length];
        var inputRight = new float[length];
        var outputLeft = new float[length];
        var outputRight = new float[length];

        inputLeft[0] = 0.99F;

        reverb.Process(inputLeft, inputRight, outputLeft, outputRight);

        var format = new WaveFormat(sampleRate, 16, 2);
        using (var writer = new WaveFileWriter("test.wav", format))
        {
            for (var t = 0; t < length; t++)
            {
                writer.WriteSample(outputLeft[t]);
                writer.WriteSample(outputRight[t]);
            }
        }
    }
}
