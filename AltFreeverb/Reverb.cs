using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AltFreeverb
{
    public sealed class Reverb
    {
        internal sealed class CombFilter
        {
            private readonly float[] buffer;

            private int bufferIndex;
            private float filterStore;

            private float feedback;
            private float damp1;
            private float damp2;

            public CombFilter(int bufferSize)
            {
                buffer = new float[bufferSize];

                bufferIndex = 0;
                filterStore = 0F;

                feedback = 0F;
                damp1 = 0F;
                damp2 = 0F;
            }

            public void Mute()
            {
                Array.Clear(buffer, 0, buffer.Length);
                filterStore = 0F;
            }

            public void Process(float[] inputBlock, float[] outputBlock)
            {
                var blockIndex = 0;
                while (blockIndex < outputBlock.Length)
                {
                    if (bufferIndex == buffer.Length)
                    {
                        bufferIndex = 0;
                    }

                    var srcRem = buffer.Length - bufferIndex;
                    var dstRem = outputBlock.Length - blockIndex;
                    var rem = Math.Min(srcRem, dstRem);

                    for (var t = 0; t < rem; t++)
                    {
                        var blockPos = blockIndex + t;
                        var bufferPos = bufferIndex + t;

                        var input = inputBlock[blockPos];

                        var output = buffer[bufferPos];
                        if ((Unsafe.As<float, int>(ref output) & 0x7F800000) < 0x32000000)
                        {
                            output = 0F;
                        }

                        filterStore = (output * damp2) + (filterStore * damp1);
                        if ((Unsafe.As<float, int>(ref filterStore) & 0x7F800000) < 0x32000000)
                        {
                            filterStore = 0F;
                        }

                        buffer[bufferPos] = input + (filterStore * feedback);
                        outputBlock[blockPos] += output;
                    }

                    bufferIndex += rem;
                    blockIndex += rem;
                }
            }

            public float Feedback
            {
                get => feedback;
                set => feedback = value;
            }

            public float Damp
            {
                get => damp1;

                set
                {
                    damp1 = value;
                    damp2 = 1F - value;
                }
            }
        }



        internal sealed class AllPassFilter
        {
            private readonly float[] buffer;

            private int bufferIndex;

            private float feedback;

            public AllPassFilter(int bufferSize)
            {
                buffer = new float[bufferSize];

                bufferIndex = 0;

                feedback = 0F;
            }

            public void Mute()
            {
                Array.Clear(buffer, 0, buffer.Length);
            }

            public void Process(float[] block)
            {
                var blockIndex = 0;
                while (blockIndex < block.Length)
                {
                    if (bufferIndex == buffer.Length)
                    {
                        bufferIndex = 0;
                    }

                    var srcRem = buffer.Length - bufferIndex;
                    var dstRem = block.Length - blockIndex;
                    var rem = Math.Min(srcRem, dstRem);

                    for (var t = 0; t < rem; t++)
                    {
                        var blockPos = blockIndex + t;
                        var bufferPos = bufferIndex + t;

                        var input = block[blockPos];

                        var bufout = buffer[bufferPos];
                        if ((Unsafe.As<float, int>(ref bufout) & 0x7F800000) < 0x32000000)
                        {
                            bufout = 0F;
                        }

                        block[blockPos] = bufout - input;
                        buffer[bufferPos] = input + (bufout * feedback);
                    }

                    bufferIndex += rem;
                    blockIndex += rem;
                }
            }

            public float Feedback
            {
                get => feedback;
                set => feedback = value;
            }
        }
    }
}
