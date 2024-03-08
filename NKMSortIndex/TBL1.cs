using Amicitia.IO.Binary;

namespace SMT4NKMSortIndexTextDump.NKMSortIndex
{
    internal class TBL1
    {
        public int MAGIC { get; set; }
        public int SectionTotalSize { get; set; }
        public int EntrySize { get; set; }
        public int EntryTargetPadding { get; set; }

        public void Read(BinaryObjectReader TBL1Reader)
        {
            MAGIC = TBL1Reader.ReadInt32();
            SectionTotalSize = TBL1Reader.ReadInt32();
            EntrySize = TBL1Reader.ReadInt32();
            EntryTargetPadding = TBL1Reader.ReadInt32();

            Console.WriteLine($"\nTBL1 data: SectionTotalSize - {SectionTotalSize:X8}\nEntrySize - {EntrySize:X8}");
        }

        public void Write(BinaryObjectWriter TBL1Writer)
        {
            TBL1Writer.WriteInt32(MAGIC);
            TBL1Writer.WriteInt32(SectionTotalSize);
            TBL1Writer.WriteInt32(EntrySize);
            TBL1Writer.WriteInt32(EntryTargetPadding);
        }
    }
}
