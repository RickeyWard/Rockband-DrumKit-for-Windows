namespace Toub.Sound.Midi
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;

    public sealed class MidiCodeGenerator
    {
        private MidiCodeGenerator()
        {
        }

        private static CodeExpression CreateDataArray(byte[] data)
        {
            CodeExpression[] initializers = new CodeExpression[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                initializers[i] = new CodePrimitiveExpression(data[i]);
            }
            return new CodeArrayCreateExpression(typeof(byte), initializers);
        }

        private static CodeObjectCreateExpression CreateEvent(MidiEvent ev)
        {
            CodeObjectCreateExpression expression = null;
            if (ev is SystemExclusiveMidiEvent)
            {
                return CreateSystemEvent(ev);
            }
            if (ev is MetaMidiEvent)
            {
                return CreateMetaEvent(ev);
            }
            if (ev is VoiceMidiEvent)
            {
                expression = CreateVoiceEvent(ev);
            }
            return expression;
        }

        private static CodeObjectCreateExpression CreateMetaEvent(MidiEvent ev)
        {
            CodeObjectCreateExpression expression = null;
            CodeExpression expression2 = new CodePrimitiveExpression(ev.DeltaTime);
            if (ev is SequenceNumber)
            {
                SequenceNumber number = (SequenceNumber) ev;
                return new CodeObjectCreateExpression(typeof(SequenceNumber), new CodeExpression[] { expression2, new CodePrimitiveExpression(number.Number) });
            }
            if (ev is Text)
            {
                Text text = (Text) ev;
                return new CodeObjectCreateExpression(typeof(Text), new CodeExpression[] { expression2, new CodePrimitiveExpression(text.Text) });
            }
            if (ev is Copyright)
            {
                Copyright copyright = (Copyright) ev;
                return new CodeObjectCreateExpression(typeof(Copyright), new CodeExpression[] { expression2, new CodePrimitiveExpression(copyright.Text) });
            }
            if (ev is SequenceTrackName)
            {
                SequenceTrackName name = (SequenceTrackName) ev;
                return new CodeObjectCreateExpression(typeof(SequenceTrackName), new CodeExpression[] { expression2, new CodePrimitiveExpression(name.Text) });
            }
            if (ev is Instrument)
            {
                Instrument instrument = (Instrument) ev;
                return new CodeObjectCreateExpression(typeof(Instrument), new CodeExpression[] { expression2, new CodePrimitiveExpression(instrument.Text) });
            }
            if (ev is Lyric)
            {
                Lyric lyric = (Lyric) ev;
                return new CodeObjectCreateExpression(typeof(Lyric), new CodeExpression[] { expression2, new CodePrimitiveExpression(lyric.Text) });
            }
            if (ev is Marker)
            {
                Marker marker = (Marker) ev;
                return new CodeObjectCreateExpression(typeof(Marker), new CodeExpression[] { expression2, new CodePrimitiveExpression(marker.Text) });
            }
            if (ev is CuePoint)
            {
                CuePoint point = (CuePoint) ev;
                return new CodeObjectCreateExpression(typeof(CuePoint), new CodeExpression[] { expression2, new CodePrimitiveExpression(point.Text) });
            }
            if (ev is ProgramName)
            {
                ProgramName name2 = (ProgramName) ev;
                return new CodeObjectCreateExpression(typeof(ProgramName), new CodeExpression[] { expression2, new CodePrimitiveExpression(name2.Text) });
            }
            if (ev is DeviceName)
            {
                DeviceName name3 = (DeviceName) ev;
                return new CodeObjectCreateExpression(typeof(DeviceName), new CodeExpression[] { expression2, new CodePrimitiveExpression(name3.Text) });
            }
            if (ev is ChannelPrefix)
            {
                ChannelPrefix prefix = (ChannelPrefix) ev;
                return new CodeObjectCreateExpression(typeof(ChannelPrefix), new CodeExpression[] { expression2, new CodePrimitiveExpression(prefix.Prefix) });
            }
            if (ev is MidiPort)
            {
                MidiPort port = (MidiPort) ev;
                return new CodeObjectCreateExpression(typeof(MidiPort), new CodeExpression[] { expression2, new CodePrimitiveExpression(port.Port) });
            }
            if (ev is EndOfTrack)
            {
                EndOfTrack track1 = (EndOfTrack) ev;
                return new CodeObjectCreateExpression(typeof(EndOfTrack), new CodeExpression[] { expression2 });
            }
            if (ev is Tempo)
            {
                Tempo tempo = (Tempo) ev;
                return new CodeObjectCreateExpression(typeof(Tempo), new CodeExpression[] { expression2, new CodePrimitiveExpression(tempo.Value) });
            }
            if (ev is SMPTEOffset)
            {
                SMPTEOffset offset = (SMPTEOffset) ev;
                return new CodeObjectCreateExpression(typeof(SMPTEOffset), new CodeExpression[] { expression2, new CodePrimitiveExpression(offset.Hours), new CodePrimitiveExpression(offset.Minutes), new CodePrimitiveExpression(offset.Seconds), new CodePrimitiveExpression(offset.Frames), new CodePrimitiveExpression(offset.FractionalFrames) });
            }
            if (ev is TimeSignature)
            {
                TimeSignature signature = (TimeSignature) ev;
                return new CodeObjectCreateExpression(typeof(TimeSignature), new CodeExpression[] { expression2, new CodePrimitiveExpression(signature.Numerator), new CodePrimitiveExpression(signature.Denominator), new CodePrimitiveExpression(signature.MidiClocksPerClick), new CodePrimitiveExpression(signature.NumberOfNotated32nds) });
            }
            if (ev is KeySignature)
            {
                KeySignature signature2 = (KeySignature) ev;
                return new CodeObjectCreateExpression(typeof(KeySignature), new CodeExpression[] { expression2, new CodeCastExpression(typeof(Key), new CodePrimitiveExpression((byte) signature2.Key)), new CodeCastExpression(typeof(Tonality), new CodePrimitiveExpression((byte) signature2.Tonality)) });
            }
            if (ev is Proprietary)
            {
                Proprietary proprietary = (Proprietary) ev;
                return new CodeObjectCreateExpression(typeof(Proprietary), new CodeExpression[] { expression2, CreateDataArray(proprietary.Data) });
            }
            if (ev is UnknownMetaMidiEvent)
            {
                UnknownMetaMidiEvent event2 = (UnknownMetaMidiEvent) ev;
                expression = new CodeObjectCreateExpression(typeof(UnknownMetaMidiEvent), new CodeExpression[] { expression2, new CodePrimitiveExpression(event2.MetaEventID), CreateDataArray(event2.Data) });
            }
            return expression;
        }

        private static CodeMemberMethod CreateSequenceMethod(MidiSequence sequence)
        {
            CodeMemberMethod method = new CodeMemberMethod {
                Attributes = MemberAttributes.Public,
                Name = "CreateSequence",
                ReturnType = new CodeTypeReference(typeof(MidiSequence))
            };
            method.Statements.Add(new CodeVariableDeclarationStatement(typeof(MidiSequence), "sequence", new CodeObjectCreateExpression(typeof(MidiSequence), new CodeExpression[] { new CodePrimitiveExpression(sequence.Format), new CodePrimitiveExpression(sequence.Division) })));
            for (int i = 0; i < sequence.NumberOfTracks; i++)
            {
                CodeExpression[] parameters = new CodeExpression[1];
                int num2 = i + 1;
                parameters[0] = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "CreateTrack" + num2.ToString(), new CodeExpression[0]);
                method.Statements.Add(new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("sequence"), "AddTrack", parameters));
            }
            method.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("sequence")));
            return method;
        }

        private static CodeObjectCreateExpression CreateSystemEvent(MidiEvent ev)
        {
            CodeObjectCreateExpression expression = null;
            CodeExpression expression2 = new CodePrimitiveExpression(ev.DeltaTime);
            if (ev is SystemExclusiveMidiEvent)
            {
                SystemExclusiveMidiEvent event2 = (SystemExclusiveMidiEvent) ev;
                expression = new CodeObjectCreateExpression(typeof(SystemExclusiveMidiEvent), new CodeExpression[] { expression2, CreateDataArray(event2.Data) });
            }
            return expression;
        }

        private static CodeMemberMethod CreateTrackMethod(string name, MidiTrack track)
        {
            CodeMemberMethod method = new CodeMemberMethod {
                Name = name,
                ReturnType = new CodeTypeReference(typeof(MidiTrack))
            };
            CodeExpression[] parameters = new CodeExpression[0];
            method.Statements.Add(new CodeVariableDeclarationStatement(typeof(MidiTrack), "track", new CodeObjectCreateExpression(typeof(MidiTrack), parameters)));
            for (int i = 0; i < track.Events.Count; i++)
            {
                CodeObjectCreateExpression expression = CreateEvent(track.Events[i]);
                if (expression != null)
                {
                    method.Statements.Add(new CodeMethodInvokeExpression(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression("track"), "Events"), "Add", new CodeExpression[] { expression }));
                }
            }
            method.Statements.Add(new CodeMethodReturnStatement(new CodeVariableReferenceExpression("track")));
            return method;
        }

        private static CodeObjectCreateExpression CreateVoiceEvent(MidiEvent ev)
        {
            CodeObjectCreateExpression expression = null;
            CodeExpression expression2 = new CodePrimitiveExpression(ev.DeltaTime);
            if (ev is NoteOn)
            {
                NoteOn on = (NoteOn) ev;
                return new CodeObjectCreateExpression(typeof(NoteOn), new CodeExpression[] { expression2, new CodePrimitiveExpression(on.Channel), new CodePrimitiveExpression(MidiEvent.GetNoteName(on.Note)), new CodePrimitiveExpression(on.Velocity) });
            }
            if (ev is NoteOff)
            {
                NoteOff off = (NoteOff) ev;
                return new CodeObjectCreateExpression(typeof(NoteOff), new CodeExpression[] { expression2, new CodePrimitiveExpression(off.Channel), new CodePrimitiveExpression(MidiEvent.GetNoteName(off.Note)), new CodePrimitiveExpression(off.Velocity) });
            }
            if (ev is Aftertouch)
            {
                Aftertouch aftertouch = (Aftertouch) ev;
                return new CodeObjectCreateExpression(typeof(Aftertouch), new CodeExpression[] { expression2, new CodePrimitiveExpression(aftertouch.Channel), new CodePrimitiveExpression(MidiEvent.GetNoteName(aftertouch.Note)), new CodePrimitiveExpression(aftertouch.Pressure) });
            }
            if (ev is ProgramChange)
            {
                ProgramChange change = (ProgramChange) ev;
                return new CodeObjectCreateExpression(typeof(ProgramChange), new CodeExpression[] { expression2, new CodePrimitiveExpression(change.Channel), new CodeCastExpression(typeof(GeneralMidiInstruments), new CodePrimitiveExpression(change.Number)) });
            }
            if (ev is Controller)
            {
                Controller controller = (Controller) ev;
                return new CodeObjectCreateExpression(typeof(Controller), new CodeExpression[] { expression2, new CodePrimitiveExpression(controller.Channel), new CodeCastExpression(typeof(Controllers), new CodePrimitiveExpression(controller.Number)), new CodePrimitiveExpression(controller.Value) });
            }
            if (ev is ChannelPressure)
            {
                ChannelPressure pressure = (ChannelPressure) ev;
                return new CodeObjectCreateExpression(typeof(ChannelPressure), new CodeExpression[] { expression2, new CodePrimitiveExpression(pressure.Channel), new CodePrimitiveExpression(pressure.Pressure) });
            }
            if (ev is PitchWheel)
            {
                PitchWheel wheel = (PitchWheel) ev;
                expression = new CodeObjectCreateExpression(typeof(PitchWheel), new CodeExpression[] { expression2, new CodePrimitiveExpression(wheel.Channel), new CodePrimitiveExpression(wheel.UpperBits), new CodePrimitiveExpression(wheel.LowerBits) });
            }
            return expression;
        }

        public static void GenerateMIDICode(ICodeGenerator generator, MidiSequence sequence, string midiName, TextWriter writer)
        {
            CodeNamespace e = new CodeNamespace("AutoGenerated");
            CodeTypeDeclaration declaration = new CodeTypeDeclaration(midiName);
            declaration.Members.Add(CreateSequenceMethod(sequence));
            for (int i = 0; i < sequence.NumberOfTracks; i++)
            {
                int num2 = i + 1;
                declaration.Members.Add(CreateTrackMethod("CreateTrack" + num2.ToString(), sequence[i]));
            }
            e.Types.Add(declaration);
            generator.GenerateCodeFromNamespace(e, writer, new CodeGeneratorOptions());
        }
    }
}

