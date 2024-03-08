using Amicitia.IO.Binary;

namespace SMT4NKMSortIndexTextDump.NKMSortIndex
{
    internal class RaceName
    {
        public TBL1 TBL1Header { get; set; } = new TBL1();
        public List<string> Name { get; set; } = new List<string>();

        public void Read(BinaryObjectReader RaceNameReader)
        {
            TBL1Header.Read(RaceNameReader);

            var numOfEntries = TBL1Header.SectionTotalSize / TBL1Header.EntrySize; // EntrySize should be 0x28

            Console.WriteLine($"Race Names: Expecting {numOfEntries} entries");

            for (var i = 0; i < numOfEntries; i++)
            {
                Name.Add(RaceNameReader.ReadString(StringBinaryFormat.FixedLength, 0x10));
                var Repeat = RaceNameReader.ReadString(StringBinaryFormat.FixedLength, 0x10);
                var padding = RaceNameReader.ReadUInt64();
            }
        }

        public void Write(BinaryObjectWriter RaceNameWriter)
        {
            TBL1Header.MAGIC = 0x314C4254; // TBL1
            TBL1Header.EntrySize = 0x28;
            TBL1Header.SectionTotalSize = Name.Count * TBL1Header.EntrySize;
            TBL1Header.EntryTargetPadding = 0x10;

            TBL1Header.Write(RaceNameWriter);

            foreach (string race_name in Name)
            {
                RaceNameWriter.WriteString(StringBinaryFormat.FixedLength, race_name, 0x10);
                RaceNameWriter.WriteString(StringBinaryFormat.FixedLength, race_name, 0x10);
                RaceNameWriter.WriteUInt64(0);
            }

            RaceNameWriter.Align(TBL1Header.EntryTargetPadding);
        }
    }
}
