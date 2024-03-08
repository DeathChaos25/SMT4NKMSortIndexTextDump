using Amicitia.IO.Binary;

namespace SMT4NKMSortIndexTextDump.NKMSortIndex
{
    internal class TBCR
    {
        public int MAGIC { get; set; }
        public int HeaderSize { get; set; }
        public int SectionNum { get; set; }
        public List<uint> SectionPointers { get; set; } = new List<uint>();

        public void Read(BinaryObjectReader TBCRReader)
        {
            MAGIC = TBCRReader.ReadInt32();
            HeaderSize = TBCRReader.ReadInt32();
            SectionNum = TBCRReader.ReadInt32();

            for(int i = 0; i < SectionNum; i++)
            {
                SectionPointers.Add((uint)(TBCRReader.ReadUInt32() + HeaderSize));
            }

            foreach(int ptr in SectionPointers)
            {
                Console.WriteLine($"TBCR: Section Ptr 0x{ptr:X8}");
            }

            byte[] targetPadding = TBCRReader.ReadArray<byte>(HeaderSize - (int)TBCRReader.Position);
            Console.WriteLine($"TBCR: Current address after reading Ptrs is 0x{TBCRReader.Position:X8}");
        }

        public void Write(BinaryObjectWriter TBCRWriter)
        {
            TBCRWriter.WriteInt32(MAGIC);
            TBCRWriter.WriteInt32(HeaderSize);
            TBCRWriter.WriteInt32(SectionNum);

            for (int i = 0; i < SectionPointers.Count; i++)
            {
                TBCRWriter.WriteUInt32((uint)(SectionPointers[i] - HeaderSize)); // write pointers with offset
            }

            TBCRWriter.Align(HeaderSize);

            Console.WriteLine($"TBCR: Current address after Writing Ptrs is 0x{TBCRWriter.Position:X8}");
        }
    }
}
