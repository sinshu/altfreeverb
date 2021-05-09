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
        var sampleRates = new int[]
        {
            16000,
            22050,
            44100,
            96000,
            192000
        };

        foreach (var sampleRate in sampleRates)
        {
            var length = 1 * sampleRate;

            var reverb = new Reverb(sampleRate, length);

            var inputLeft = new float[length];
            var inputRight = new float[length];
            var outputLeft = new float[length];
            var outputRight = new float[length];

            inputLeft[0] = 1;

            reverb.Process(inputLeft, inputRight, outputLeft, outputRight);

            var format = new WaveFormat(sampleRate, 16, 2);
            using (var writer = new WaveFileWriter("test" + sampleRate + ".wav", format))
            {
                for (var t = 0; t < length; t++)
                {
                    writer.WriteSample(10 * outputLeft[t]);
                    writer.WriteSample(10 * outputRight[t]);
                }
            }
        }
    }
}
