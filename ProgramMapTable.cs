﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TsAnalyser
{
    public class ProgramMapTable
    {
        public static Dictionary<int, String> ElementarystreamTypes = new Dictionary<int, String>()
        {
            { 0, "Reserved" },
            {1,"ISO/IEC 11172-2 (MPEG-1 video) in a packetized stream" },
            {2,"ITU-T Rec. H.262 and ISO/IEC 13818-2 (MPEG-2 higher rate interlaced video) in a packetized stream"},
            {3,"ISO/IEC 11172-3 (MPEG-1 audio) in a packetized stream"},
            {4,"ISO/IEC 13818-3 (MPEG-2 halved sample rate audio) in a packetized stream"},
            {5,"ITU-T Rec. H.222 and ISO/IEC 13818-1 (MPEG-2 tabled data) privately defined"},
            {6,"ITU-T Rec. H.222 and ISO/IEC 13818-1 (MPEG-2 packetized data) privately defined (i.e., DVB subtitles/VBI and AC-3)"},
            {7,"ISO/IEC 13522 (MHEG) in a packetized stream"},
            {8,"ITU-T Rec. H.222 and ISO/IEC 13818-1 DSM CC in a packetized stream"},
            {9,"ITU-T Rec. H.222 and ISO/IEC 13818-1/11172-1 auxiliary data in a packetized stream"},
            {10,"ISO/IEC 13818-6 DSM CC multiprotocol encapsulation"},
            {11, "ISO/IEC 13818-6 DSM CC U-N messages"},
            {12, "ISO/IEC 13818-6 DSM CC stream descriptors"},
            {13, "ISO/IEC 13818-6 DSM CC tabled data"},
            {14,"ISO/IEC 13818-1 auxiliary data in a packetized stream"},
            {15,"ISO/IEC 13818-7 ADTS AAC (MPEG-2 lower bit-rate audio) in a packetized stream"},
            {16,"ISO/IEC 14496-2 (MPEG-4 H.263 based video) in a packetized stream"},
            {17,"ISO/IEC 14496-3 (MPEG-4 LOAS multi-format framed audio) in a packetized stream"},
            {18,"ISO/IEC 14496-1 (MPEG-4 FlexMux) in a packetized stream"},
            {19,"ISO/IEC 14496-1 (MPEG-4 FlexMux) in ISO/IEC 14496 tables"},
            {20, "ISO/IEC 13818-6 DSM CC synchronized download protocol"},
            {21, "Packetized metadata"},
            {22, "Sectioned metadata"},
            {23, "ISO/IEC 13818-6 DSM CC Data Carousel metadata"},
            {24, "ISO/IEC 13818-6 DSM CC Object Carousel metadata"},
            {25, "ISO/IEC 13818-6 Synchronized Download Protocol metadata"},
            {26, "ISO/IEC 13818-11 IPMP"},
            {27,"ITU-T Rec. H.264 and ISO/IEC 14496-10 (lower bit-rate video) in a packetized stream"},
            {36,"ITU-T Rec. H.265 and ISO/IEC 23008-2 (Ultra HD video) in a packetized stream"},
            {66,"Chinese Video Standard in a packetized stream"},
            {128,"ITU-T Rec. H.262 and ISO/IEC 13818-2 for DigiCipher II or PCM audio for Blu-ray in a packetized stream"},
            {129,"Dolby Digital up to six channel audio for ATSC and Blu-ray in a packetized stream"},
            {130,"SCTE subtitle or DTS 6 channel audio for Blu-ray in a packetized stream"},
            {131,"Dolby TrueHD lossless audio for Blu-ray in a packetized stream"},
            {132,"Dolby Digital Plus up to 16 channel audio for Blu-ray in a packetized stream"},
            {133,"DTS 8 channel audio for Blu-ray in a packetized stream"},
            {134,"DTS 8 channel lossless audio for Blu-ray in a packetized stream"},
            {135,"Dolby Digital Plus up to 16 channel audio for ATSC in a packetized stream"},
            {144,"Blu-ray Presentation Graphic Stream (subtitling) in a packetized stream"},
            {145, "ATSC DSM CC Network Resources table"},
            {192,"DigiCipher II text in a packetized stream"},
            {193,"Dolby Digital up to six channel audio with AES-128-CBC data encryption in a packetized stream"},
            {194,"ATSC DSM CC synchronous data or Dolby Digital Plus up to 16 channel audio with AES-128-CBC data encryption in a packetized stream"},
            {207,"ISO/IEC 13818-7 ADTS AAC with AES-128-CBC frame encryption in a packetized stream"},
            {209,"BBC Dirac (Ultra HD video) in a packetized stream"},
            {219,"ITU-T Rec. H.264 and ISO/IEC 14496-10 with AES-128-CBC slice encryption in a packetized stream"},
            {234,"Microsoft Windows Media Video 9 (lower bit-rate video) in a packetized stream"}
        };

        public class Section
        {
            public byte StreamType;
            public short ElementaryPID;
            public short ESInfoLength;
            public IEnumerable<Descriptor> Descriptors;
        }


        public byte PointerField;
        public byte TableId;
        public short SectionLength;
        public short ProgramNumber;
        public byte VersionNumber;
        public bool CurrentNextIndicator;
        public byte SectionNumber;
        public byte LastSectionNumber;
        public short ProgramInfoLength;
        public IEnumerable<Descriptor> Descriptors;
        public List<Section> Sections;
    }
    
    public class ProgramMapTableFactory
    {
        private const int TsPacketSize = 188;
        private const int TsPacketHeaderSize = 4;

        public static ProgramMapTable ProgramMapTableFromTsPackets(TsPacket[] packets)
        {
            var pmt = new ProgramMapTable();

            for (var i = 0; i < packets.Length; i++)
            {
                if (!packets[i].PayloadUnitStartIndicator) continue;

                // if (packets[i].Payload.Length != TsPacketSize - TsPacketHeaderSize) continue;

                pmt.PointerField = packets[i].Payload[0];

                if (pmt.PointerField > packets[i].Payload.Length)
                {
                    Debug.Assert(true, "Program Association Table has packet pointer outside the packet.");
                }

                var pos = 1 + pmt.PointerField;

                pmt.TableId = packets[i].Payload[pos];
                pmt.SectionLength = (short)(((packets[i].Payload[pos + 1]) & 0x3 << 8) + packets[i].Payload[pos + 2]);
                pmt.ProgramNumber = (short)((packets[i].Payload[pos + 3] << 8) + packets[i].Payload[pos + 4]);
                pmt.VersionNumber = (byte)(packets[i].Payload[pos + 5] & 0x3E);
                pmt.CurrentNextIndicator = (packets[i].Payload[pos + 5] & 0x1) != 0;
                pmt.SectionNumber = packets[i].Payload[pos + 6];
                pmt.LastSectionNumber = packets[i].Payload[pos + 7];
                pmt.ProgramInfoLength = (short)(((packets[i].Payload[pos + 10] & 0x3) << 8) + packets[i].Payload[pos + 11]);

                int descLength = 0;
                List<Descriptor> descriptors = new List<Descriptor>();
                while (descLength < pmt.ProgramInfoLength)
                {
                    Descriptor desc = DescriptorFactory.DescriptorFromTsPacketPayload(packets[i].Payload, pos + 12 + 5 + descLength);
                    descriptors.Add(desc);
                    descLength += desc.DescriptorLength + 2;
                }
                pmt.Descriptors = descriptors;                 
                int sectionstartStart = pos + 12 + pmt.ProgramInfoLength;
                int currentSectionStart = sectionstartStart;
                pmt.Sections = new List<ProgramMapTable.Section>();
                do
                {
                    ProgramMapTable.Section section = new ProgramMapTable.Section();
                    section.StreamType = packets[i].Payload[currentSectionStart];
                    section.ElementaryPID = (short)(((packets[i].Payload[currentSectionStart + 1] & 0x1F) << 8) + packets[i].Payload[currentSectionStart + 2]);
                    section.ESInfoLength = (short)(((packets[i].Payload[currentSectionStart + 3] & 0x3) << 8) + packets[i].Payload[currentSectionStart + 4]);
                    //section.Descriptor = BitConverter.ToString(packets[i].Payload, currentSectionStart + 5, section.ESInfoLength);
                    descLength = 0;
                    descriptors = new List<Descriptor>();
                    while (descLength < section.ESInfoLength)
                    {
                        Descriptor desc = DescriptorFactory.DescriptorFromTsPacketPayload(packets[i].Payload, currentSectionStart + 5 + descLength);
                        descriptors.Add(desc);
                        descLength += desc.DescriptorLength + 2;
                    }
                    section.Descriptors = descriptors;
                    currentSectionStart = currentSectionStart + 5 + section.ESInfoLength;
                    pmt.Sections.Add(section);
                } while ((currentSectionStart + 5) <= pmt.SectionLength);
            }

            return pmt;
        }
    }
        
}