namespace Toub.Sound.Midi
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;

    internal class MidiParser
    {
        private MidiParser()
        {
        }

        private static MidiEvent ParseMetaEvent(long deltaTime, byte eventType, byte[] data, ref long pos)
        {
            MidiEvent event3;
            try
            {
                long num3;
                MidiEvent event2 = null;
                switch (eventType)
                {
                    case 0:
                    {
                        pos += 1L;
                        int number = (data[(int) ((IntPtr) pos)] << 8) | data[(int) ((IntPtr) (pos + 1L))];
                        event2 = new SequenceNumber(deltaTime, number);
                        pos += 2L;
                        break;
                    }
                    case 1:
                        event2 = new Text(deltaTime, ReadASCIIText(data, ref pos));
                        break;

                    case 2:
                        event2 = new Copyright(deltaTime, ReadASCIIText(data, ref pos));
                        break;

                    case 3:
                        event2 = new SequenceTrackName(deltaTime, ReadASCIIText(data, ref pos));
                        break;

                    case 4:
                        event2 = new Instrument(deltaTime, ReadASCIIText(data, ref pos));
                        break;

                    case 5:
                        event2 = new Lyric(deltaTime, ReadASCIIText(data, ref pos));
                        break;

                    case 6:
                        event2 = new Marker(deltaTime, ReadASCIIText(data, ref pos));
                        break;

                    case 7:
                        event2 = new CuePoint(deltaTime, ReadASCIIText(data, ref pos));
                        break;

                    case 8:
                        event2 = new ProgramName(deltaTime, ReadASCIIText(data, ref pos));
                        break;

                    case 9:
                        event2 = new DeviceName(deltaTime, ReadASCIIText(data, ref pos));
                        break;

                    case 0x20:
                        pos += 1L;
                        event2 = new ChannelPrefix(deltaTime, data[(int) ((IntPtr) pos)]);
                        pos += 1L;
                        break;

                    case 0x21:
                        pos += 1L;
                        event2 = new MidiPort(deltaTime, data[(int) ((IntPtr) pos)]);
                        pos += 1L;
                        break;

                    case 0x2f:
                        pos += 1L;
                        event2 = new EndOfTrack(deltaTime);
                        break;

                    case 0x58:
                        pos += 1L;
                        event2 = new TimeSignature(deltaTime, data[(int) ((IntPtr) pos)], data[(int) ((IntPtr) (pos + 1L))], data[(int) ((IntPtr) (pos + 2L))], data[(int) ((IntPtr) (pos + 3L))]);
                        pos += 4L;
                        break;

                    case 0x59:
                        pos += 1L;
                        event2 = new KeySignature(deltaTime, (Key) data[(int) ((IntPtr) pos)], (Tonality) data[(int) ((IntPtr) (pos + 1L))]);
                        pos += 2L;
                        break;

                    case 0x7f:
                    {
                        num3 = ReadVariableLength(data, ref pos);
                        byte[] destinationArray = new byte[num3];
                        Array.Copy(data, (int) pos, destinationArray, 0, (int) num3);
                        event2 = new Proprietary(deltaTime, destinationArray);
                        pos += num3;
                        break;
                    }
                    case 0x51:
                    {
                        pos += 1L;
                        int num2 = ((data[(int) ((IntPtr) pos)] << 0x10) | (data[(int) ((IntPtr) (pos + 1L))] << 8)) | data[(int) ((IntPtr) (pos + 2L))];
                        event2 = new Tempo(deltaTime, num2);
                        pos += 3L;
                        break;
                    }
                    case 0x54:
                        pos += 1L;
                        event2 = new SMPTEOffset(deltaTime, data[(int) ((IntPtr) pos)], data[(int) ((IntPtr) (pos + 1L))], data[(int) ((IntPtr) (pos + 2L))], data[(int) ((IntPtr) (pos + 3L))], data[(int) ((IntPtr) (pos + 4L))]);
                        pos += 5L;
                        break;

                    default:
                    {
                        num3 = ReadVariableLength(data, ref pos);
                        byte[] buffer2 = new byte[num3];
                        Array.Copy(data, (int) pos, buffer2, 0, (int) num3);
                        event2 = new UnknownMetaMidiEvent(deltaTime, eventType, buffer2);
                        pos += num3;
                        break;
                    }
                }
                event3 = event2;
            }
            catch (Exception exception)
            {
                throw new MidiParserException("Unable to parse meta MIDI event.", exception, pos);
            }
            return event3;
        }

        public static MidiTrack ParseToTrack(byte[] data)
        {
            MidiTrack track2;
            long pos = 0L;
            bool flag = false;
            int num2 = 0;
            bool flag2 = false;
            byte[] destinationArray = null;
            try
            {
                MidiTrack track = new MidiTrack();
                while (pos < data.Length)
                {
                    long deltaTime = ReadVariableLength(data, ref pos);
                    byte num4 = data[(int) ((IntPtr) pos)];
                    if (flag2 && (num4 != 0x7f))
                    {
                        throw new MidiParserException("Expected to find a system exclusive continue byte.", pos);
                    }
                    if ((num4 & 0x80) == 0)
                    {
                        if (num2 == 0)
                        {
                            throw new MidiParserException("Status byte required for running status.", pos);
                        }
                        flag = true;
                    }
                    else
                    {
                        num2 = num4;
                        flag = false;
                    }
                    byte messageType = (byte) ((num2 >> 4) & 15);
                    MidiEvent message = null;
                    if ((messageType >= 8) && (messageType <= 14))
                    {
                        if (!flag)
                        {
                            pos += 1L;
                        }
                        byte channel = (byte) (num2 & 15);
                        message = ParseVoiceEvent(deltaTime, messageType, channel, data, ref pos);
                    }
                    else if (num2 == 0xff)
                    {
                        pos += 1L;
                        byte eventType = data[(int) ((IntPtr) pos)];
                        pos += 1L;
                        message = ParseMetaEvent(deltaTime, eventType, data, ref pos);
                    }
                    else if (num2 == 240)
                    {
                        pos += 1L;
                        long num8 = ReadVariableLength(data, ref pos);
                        if (data[(int) ((IntPtr) ((pos + num8) - 1L))] == 0xf7)
                        {
                            destinationArray = new byte[num8 - 1L];
                            Array.Copy(data, (int) pos, destinationArray, 0, ((int) num8) - 1);
                            message = new SystemExclusiveMidiEvent(deltaTime, destinationArray);
                        }
                        else
                        {
                            int destinationIndex = (destinationArray == null) ? 0 : destinationArray.Length;
                            byte[] array = new byte[destinationIndex + num8];
                            if (destinationArray != null)
                            {
                                destinationArray.CopyTo(array, 0);
                            }
                            Array.Copy(data, (int) pos, array, destinationIndex, (int) num8);
                            destinationArray = array;
                            flag2 = true;
                        }
                        pos += num8;
                    }
                    else
                    {
                        if (num2 != 0xf7)
                        {
                            throw new MidiParserException("Invalid status byte found.", pos);
                        }
                        if (!flag2)
                        {
                            destinationArray = null;
                        }
                        pos += 1L;
                        long num10 = ReadVariableLength(data, ref pos);
                        int num11 = (destinationArray == null) ? 0 : destinationArray.Length;
                        byte[] buffer3 = new byte[num11 + num10];
                        if (destinationArray != null)
                        {
                            destinationArray.CopyTo(buffer3, 0);
                        }
                        Array.Copy(data, (int) pos, buffer3, num11, (int) num10);
                        destinationArray = buffer3;
                        if (data[(int) ((IntPtr) ((pos + num10) - 1L))] == 0xf7)
                        {
                            message = new SystemExclusiveMidiEvent(deltaTime, destinationArray);
                            destinationArray = null;
                            flag2 = false;
                        }
                    }
                    if (message != null)
                    {
                        track.Events.Add(message);
                    }
                }
                track2 = track;
            }
            catch (MidiParserException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new MidiParserException("Failed to parse MIDI file.", exception, pos);
            }
            return track2;
        }

        private static MidiEvent ParseVoiceEvent(long deltaTime, byte messageType, byte channel, byte[] data, ref long pos)
        {
            MidiEvent event3;
            try
            {
                MidiEvent event2 = null;
                switch (messageType)
                {
                    case 8:
                        event2 = new NoteOff(deltaTime, channel, data[(int) ((IntPtr) pos)], data[(int) ((IntPtr) (pos + 1L))]);
                        pos += 2L;
                        break;

                    case 9:
                        event2 = new NoteOn(deltaTime, channel, data[(int) ((IntPtr) pos)], data[(int) ((IntPtr) (pos + 1L))]);
                        pos += 2L;
                        break;

                    case 10:
                        event2 = new Aftertouch(deltaTime, channel, data[(int) ((IntPtr) pos)], data[(int) ((IntPtr) (pos + 1L))]);
                        pos += 2L;
                        break;

                    case 11:
                        event2 = new Controller(deltaTime, channel, data[(int) ((IntPtr) pos)], data[(int) ((IntPtr) (pos + 1L))]);
                        pos += 2L;
                        break;

                    case 12:
                        event2 = new ProgramChange(deltaTime, channel, data[(int) ((IntPtr) pos)]);
                        pos += 1L;
                        break;

                    case 13:
                        event2 = new ChannelPressure(deltaTime, channel, data[(int) ((IntPtr) pos)]);
                        pos += 1L;
                        break;

                    case 14:
                    {
                        byte num2;
                        byte num3;
                        int bits = (data[(int) ((IntPtr) pos)] << 8) | data[(int) ((IntPtr) (pos + 1L))];
                        MidiEvent.Split14BitsToBytes(bits, out num2, out num3);
                        event2 = new PitchWheel(deltaTime, channel, num2, num3);
                        pos += 2L;
                        break;
                    }
                    default:
                        throw new ArgumentOutOfRangeException("messageType", messageType, "Not a voice message.");
                }
                event3 = event2;
            }
            catch (Exception exception)
            {
                throw new MidiParserException("Unable to parse voice MIDI event.", exception, pos);
            }
            return event3;
        }

        private static string ReadASCIIText(byte[] data, ref long pos)
        {
            long num = ReadVariableLength(data, ref pos);
            string str = Encoding.ASCII.GetString(data, (int) pos, (int) num);
            pos += num;
            return str;
        }

        private static long ReadVariableLength(byte[] data, ref long pos)
        {
            long num = data[(int) ((IntPtr) pos)];
            if ((data[(int) ((IntPtr) pos)] & 0x80) != 0)
            {
                num &= 0x7fL;
                do
                {
                    pos += 1L;
                    num = (num << 7) + (data[(int) ((IntPtr) pos)] & 0x7f);
                }
                while ((pos < data.Length) && ((data[(int) ((IntPtr) pos)] & 0x80) != 0));
            }
            pos += 1L;
            return num;
        }

        [Serializable]
        public class MidiParserException : ApplicationException, ISerializable
        {
            private long _position;

            public MidiParserException()
            {
            }

            public MidiParserException(string message) : base(message)
            {
            }

            protected MidiParserException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
                this._position = info.GetInt64("position");
            }

            public MidiParserException(string message, Exception innerException) : base(message, innerException)
            {
            }

            public MidiParserException(string message, long position) : base(message)
            {
                this._position = position;
            }

            public MidiParserException(string message, Exception innerException, long position) : base(message, innerException)
            {
                this._position = position;
            }

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("position", this._position);
                base.GetObjectData(info, context);
            }

            public long Position
            {
                get
                {
                    return this._position;
                }
            }
        }
    }
}

