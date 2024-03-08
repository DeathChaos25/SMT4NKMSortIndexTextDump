using Amicitia.IO.Binary;

namespace SMT4NKMSortIndexTextDump.NKMSortIndex
{
    internal class DataSection
    {
        public TBL1 TBL1Header { get; set; } = new TBL1();
        public List<byte> Data { get; set; } = new List<byte>();

        public void Read(BinaryObjectReader DataReader)
        {
            TBL1Header.Read(DataReader);

            Data = DataReader.ReadArray<byte>(TBL1Header.SectionTotalSize).ToList();
        }

        public void Write(BinaryObjectWriter DataWriter)
        {
            TBL1Header.MAGIC = 0x314C4254; // TBL1
            TBL1Header.EntrySize = 4;
            TBL1Header.SectionTotalSize = Data.Count;
            TBL1Header.EntryTargetPadding = 0x10;

            TBL1Header.Write(DataWriter);

            DataWriter.WriteArray<byte>(Data.ToArray());

            DataWriter.Align(0x10);
        }
    }
}
