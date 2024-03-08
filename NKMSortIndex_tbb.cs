using Amicitia.IO.Binary;
using SMT4NKMSortIndexTextDump.NKMSortIndex;

namespace SMT4NKMSortIndexTextDump
{
    internal class NKMSortIndex_tbb
    {
        public TBCR Header { get; set; } = new TBCR();
        public RaceName RaceNameBlock { get; set; } = new RaceName();
        public DemonName DemonNameBlock { get; set; } = new DemonName();
        public DataSection DataSection3 { get; set; } = new DataSection();
        public DataSection DataSection4 { get; set; } = new DataSection();

        public void Read(BinaryObjectReader NKMSortIndexFile)
        {
            Header.Read(NKMSortIndexFile);

            NKMSortIndexFile.Seek(Header.SectionPointers[0], SeekOrigin.Begin);
            RaceNameBlock.Read(NKMSortIndexFile);

            NKMSortIndexFile.Seek(Header.SectionPointers[1], SeekOrigin.Begin);
            DemonNameBlock.Read(NKMSortIndexFile);

            NKMSortIndexFile.Seek(Header.SectionPointers[2], SeekOrigin.Begin);
            DataSection3.Read(NKMSortIndexFile);

            NKMSortIndexFile.Seek(Header.SectionPointers[3], SeekOrigin.Begin);
            DataSection4.Read(NKMSortIndexFile);
        }
    }
}
