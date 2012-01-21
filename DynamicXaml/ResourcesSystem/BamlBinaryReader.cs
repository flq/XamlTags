using System;
using System.IO;

namespace DynamicXaml.ResourcesSystem
{
    /// <summary>
    /// Taken from the StylesExplorer project from codeplex.
    /// </summary>
    internal class BamlBinaryReader : BinaryReader
    {
        // Methods
        public BamlBinaryReader(Stream stream)
            : base(stream)
        {
        }

        public virtual double ReadCompressedDouble()
        {
            switch (ReadByte())
            {
                case 1:
                    return 0;

                case 2:
                    return 1;

                case 3:
                    return -1;

                case 4:
                    {
                        double num = ReadInt32();
                        return (num * 1E-06);
                    }
                case 5:
                    return ReadDouble();
            }
            throw new NotSupportedException();
        }

        public int ReadCompressedInt32()
        {
            return Read7BitEncodedInt();
        }
    }
}