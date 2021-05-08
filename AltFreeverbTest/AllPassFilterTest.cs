using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using AltFreeverb;

namespace AltFreeverbTest
{
    public class AllPassFilterTest
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(16)]
        [TestCase(23)]
        [TestCase(50)]
        [TestCase(100)]
        public void BufferSize16_Feedback05(int delay)
        {
            var path = @"data\apf_bs16_fb05.csv";

            var data = File.ReadLines(path).Select(line => float.Parse(line)).ToArray();
            Assert.IsTrue(data.Length == 500);

            var expected = new float[delay].Concat(data).ToArray();

            var apf = new Reverb.AllPassFilter(16);
            apf.Feedback = 0.5F;

            var actual = new float[expected.Length];
            actual[delay] = 1F;
            apf.Process(actual);

            for (var t = 0; t < expected.Length; t++)
            {
                var error = actual[t] - expected[t];
                Assert.IsTrue(Math.Abs(error) < 1.0E-3);
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(16)]
        [TestCase(23)]
        [TestCase(50)]
        [TestCase(100)]
        public void BufferSize23_Feedback07(int delay)
        {
            var path = @"data\apf_bs23_fb07.csv";

            var data = File.ReadLines(path).Select(line => float.Parse(line)).ToArray();
            Assert.IsTrue(data.Length == 500);

            var expected = new float[delay].Concat(data).ToArray();

            var apf = new Reverb.AllPassFilter(23);
            apf.Feedback = 0.7F;

            var actual = new float[expected.Length];
            actual[delay] = 1F;
            apf.Process(actual);

            for (var t = 0; t < expected.Length; t++)
            {
                var error = actual[t] - expected[t];
                Assert.IsTrue(Math.Abs(error) < 1.0E-3);
            }
        }
    }
}
