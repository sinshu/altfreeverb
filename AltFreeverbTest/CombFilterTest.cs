using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using AltFreeverb;

namespace AltFreeverbTest
{
    public class CombFilterTest
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(16)]
        [TestCase(23)]
        [TestCase(50)]
        [TestCase(100)]
        public void BufferSize16_Feedback08_Damp01(int delay)
        {
            var path = @"data\comb_bs16_fb08_da01.csv";

            var data = File.ReadLines(path).Select(line => float.Parse(line)).ToArray();
            Assert.IsTrue(data.Length == 500);

            var expected = new float[delay].Concat(data).ToArray();

            var comb = new Reverb.CombFilter(16);
            comb.Feedback = 0.8F;
            comb.Damp = 0.1F;

            var input = new float[expected.Length];
            input[delay] = 1F;

            var actual = new float[expected.Length];
            comb.Process(input, actual);

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
        public void BufferSize23_Feedback07_Damp03(int delay)
        {
            var path = @"data\comb_bs23_fb07_da03.csv";

            var data = File.ReadLines(path).Select(line => float.Parse(line)).ToArray();
            Assert.IsTrue(data.Length == 500);

            var expected = new float[delay].Concat(data).ToArray();

            var comb = new Reverb.CombFilter(23);
            comb.Feedback = 0.7F;
            comb.Damp = 0.3F;

            var input = new float[expected.Length];
            input[delay] = 1F;

            var actual = new float[expected.Length];
            comb.Process(input, actual);

            for (var t = 0; t < expected.Length; t++)
            {
                var error = actual[t] - expected[t];
                Assert.IsTrue(Math.Abs(error) < 1.0E-3);
            }
        }
    }
}
