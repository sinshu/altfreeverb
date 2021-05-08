using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NUnit.Framework;
using AltFreeverb;

namespace AltFreeverbTest
{
    public class UndenormalizeTest
    {
        [Test]
        public void Positive()
        {
            var data = new float[200];
            var x = 1F;
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = x;
                x /= 2;
            }

            for (var i = 0; i < data.Length; i++)
            {
                var value = Unsafe.As<float, int>(ref data[i]) & 0x7F800000;
                var a = value >> 23;
                Console.WriteLine(data[i] + ": " + a + ", 0x" + value.ToString("X08"));
            }
        }

        [Test]
        public void Negative()
        {
            var data = new float[200];
            var x = 1F;
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = -x;
                x /= 2;
            }

            for (var i = 0; i < data.Length; i++)
            {
                var value = Unsafe.As<float, int>(ref data[i]) & 0x7F800000;
                var a = value >> 23;
                Console.WriteLine(data[i] + ": " + a + ", 0x" + value.ToString("X08"));
            }
        }
    }
}
