namespace Toub.Sound.Midi
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    [Serializable]
    public abstract class MidiEvent : ICloneable
    {
        private long _deltaTime;

        protected MidiEvent(long deltaTime)
        {
            this.DeltaTime = deltaTime;
        }

        public MidiEvent Clone()
        {
            return (MidiEvent) base.MemberwiseClone();
        }

        internal static int CombineBytesTo14Bits(byte upper, byte lower)
        {
            int num = upper;
            num = num << 7;
            return (num | lower);
        }

        protected static string DataToString(byte[] data)
        {
            if (data == null)
            {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0)
                {
                    builder.Append(",");
                }
                builder.Append("0x");
                builder.Append(data[i].ToString("X2"));
            }
            builder.Append("]");
            return builder.ToString();
        }

        public static string GetNoteName(byte note)
        {
            string str;
            int num = note / 12;
            switch ((note % 12))
            {
                case 0:
                    str = "C";
                    break;

                case 1:
                    str = "C#";
                    break;

                case 2:
                    str = "D";
                    break;

                case 3:
                    str = "D#";
                    break;

                case 4:
                    str = "E";
                    break;

                case 5:
                    str = "F";
                    break;

                case 6:
                    str = "F#";
                    break;

                case 7:
                    str = "G";
                    break;

                case 8:
                    str = "G#";
                    break;

                case 9:
                    str = "A";
                    break;

                case 10:
                    str = "A#";
                    break;

                case 11:
                    str = "B";
                    break;

                default:
                    str = "";
                    break;
            }
            return (str + num);
        }

        public static byte GetNoteValue(string noteName)
        {
            int num;
            if (noteName == null)
            {
                throw new ArgumentNullException("noteName");
            }
            if (noteName.Length < 2)
            {
                throw new ArgumentOutOfRangeException("noteName", noteName, "Note names must be at least 2 characters in length.");
            }
            int num2 = 0;
            switch (char.ToLower(noteName[num2]))
            {
                case 'a':
                    num = 9;
                    break;

                case 'b':
                    num = 11;
                    break;

                case 'c':
                    num = 0;
                    break;

                case 'd':
                    num = 2;
                    break;

                case 'e':
                    num = 4;
                    break;

                case 'f':
                    num = 5;
                    break;

                case 'g':
                    num = 7;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("noteName", noteName, "The note must be c, d, e, f, g, a, or b.");
            }
            num2++;
            switch (char.ToLower(noteName[num2]))
            {
                case 'b':
                    num--;
                    num2++;
                    break;

                case '#':
                    num++;
                    num2++;
                    break;
            }
            if (num2 >= noteName.Length)
            {
                throw new ArgumentOutOfRangeException("noteName", noteName, "Octave must be specified.");
            }
            int num3 = noteName[num2] - '0';
            if ((num3 == 1) && (num2 < (noteName.Length - 1)))
            {
                num2++;
                num3 = 10 + (noteName[num2] - '0');
            }
            if ((num3 < 0) || (num3 > 10))
            {
                throw new ArgumentOutOfRangeException("noteName", noteName, "The octave must be in the range 0 to 10.");
            }
            num = (num3 * 12) + num;
            if ((num < 0) || (num > 0x7f))
            {
                throw new ArgumentOutOfRangeException("noteName", noteName, "Notes must be in the range from C0 to G10.");
            }
            return (byte) num;
        }

        public static byte GetNoteValue(GeneralMidiPercussion percussion)
        {
            return (byte) percussion;
        }

        internal static void Split14BitsToBytes(int bits, out byte upperBits, out byte lowerBits)
        {
            lowerBits = (byte) (bits & 0x7f);
            bits = bits >> 7;
            upperBits = (byte) (bits & 0x7f);
        }

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.GetType().Name);
            builder.Append("\t");
            builder.Append(this.DeltaTime.ToString());
            return builder.ToString();
        }

        public virtual void Write(Stream outputStream)
        {
            this.WriteVariableLength(outputStream, this._deltaTime);
        }

        protected void WriteVariableLength(Stream outputStream, long value)
        {
            long num = value & 0x7fL;
            while ((value = value >> 7) > 0L)
            {
                num = num << 8;
                num |= 0x80L;
                num += value & 0x7fL;
            }
            while (true)
            {
                outputStream.WriteByte((byte) (num & 0xffL));
                if ((num & 0x80L) == 0L)
                {
                    return;
                }
                num = num >> 8;
            }
        }

        public virtual long DeltaTime
        {
            get
            {
                return this._deltaTime;
            }
            set
            {
                if (value < 0L)
                {
                    throw new ArgumentOutOfRangeException("DeltaTime", value, "Delta times must be non-negative.");
                }
                this._deltaTime = value;
            }
        }
    }
}

