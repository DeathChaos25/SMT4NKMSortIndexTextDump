using Amicitia.IO.Binary;

namespace SMT4NKMSortIndexTextDump.NKMSortIndex
{
    internal class DemonName
    {
        public TBL1 TBL1Header { get; set; } = new TBL1();
        public List<string> Name { get; set; } = new List<string>();

        public void Read(BinaryObjectReader DemonNameReader)
        {
            TBL1Header.Read(DemonNameReader);

            var numOfEntries = TBL1Header.SectionTotalSize / TBL1Header.EntrySize; // EntrySize should be 0x28

            Console.WriteLine($"Demon Names: Expecting {numOfEntries} entries");

            for (var i = 0; i < numOfEntries; i++)
            {
                Name.Add(DemonNameReader.ReadString(StringBinaryFormat.FixedLength, 0x20));
            }
        }

        public void Write(BinaryObjectWriter DemonNameWriter)
        {
            TBL1Header.MAGIC = 0x314C4254; // TBL1
            TBL1Header.EntrySize = 0x20;
            TBL1Header.SectionTotalSize = Name.Count * TBL1Header.EntrySize;
            TBL1Header.EntryTargetPadding = 0x10;

            TBL1Header.Write(DemonNameWriter);

            foreach (string demon_name in Name)
            {
                DemonNameWriter.WriteString(StringBinaryFormat.FixedLength, demon_name, 0x20);
            }
        }
    }
}
