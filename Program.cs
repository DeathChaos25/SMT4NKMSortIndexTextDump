using System.Text;
using Amicitia.IO.Binary;

namespace SMT4NKMSortIndexTextDump
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.WriteLine("SMT4NKMSortIndexTextDump:\nUsage:\nDrag and Drop either a NKMSortIndex.tbb file or a NKMSortIndex_txt folder into the program's exe\nPress any key to exit");
                Console.ReadKey();
            }
            else
            {
                FileInfo arg0 = new FileInfo(args[0]);

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                if (arg0.Name.ToLower().Contains(".tbb"))
                {
                    Console.WriteLine($"Attempting to convert {arg0.Name}");

                    using (BinaryObjectReader NKMSortIndexFile = new BinaryObjectReader(args[0], Endianness.Little, Encoding.UTF8))
                    {
                        NKMSortIndex_tbb inFile = new NKMSortIndex_tbb();

                        inFile.Read(NKMSortIndexFile);

                        var saveDir = Path.Combine(Path.GetDirectoryName(args[0]), "NKMSortIndex_txt");
                        Directory.CreateDirectory(saveDir);

                        var savePath = Path.Combine(saveDir, "01 - Race Names.txt");
                        File.WriteAllLines(savePath, inFile.RaceNameBlock.Name, Encoding.UTF8);

                        savePath = Path.Combine(saveDir, "02 - Demon Names.txt");
                        File.WriteAllLines(savePath, inFile.DemonNameBlock.Name, Encoding.UTF8);

                        savePath = Path.Combine(saveDir, "03 - DataBlock3.dat");
                        File.WriteAllBytes(savePath, inFile.DataSection3.Data.ToArray());

                        savePath = Path.Combine(saveDir, "04 - DataBlock4.dat");
                        File.WriteAllBytes(savePath, inFile.DataSection4.Data.ToArray());
                    }

                    Console.WriteLine();
                }
                else if (arg0.Name == "NKMSortIndex_txt")
                {
                    Console.WriteLine("NAME tbl folder was input\n");

                    var savePath = Path.Combine(arg0.FullName.Replace(arg0.Name, "") + "NKMSortIndex.tbb");

                    Console.WriteLine($"Writing output tbb file to {savePath}");

                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                    NKMSortIndex_tbb outFile = new NKMSortIndex_tbb();

                    using (BinaryObjectWriter NKMSortIndexFile = new BinaryObjectWriter(savePath, Endianness.Little, Encoding.UTF8))
                    {
                        outFile.Header.MAGIC = 0x52434254;
                        outFile.Header.HeaderSize = 0x20;
                        outFile.Header.SectionNum = 4;

                        NKMSortIndexFile.WriteInt32(outFile.Header.MAGIC);
                        NKMSortIndexFile.Align(outFile.Header.HeaderSize); // create dummy header with dummy ptrs
                        uint currPos = 0;

                        currPos = (uint)NKMSortIndexFile.Position;
                        Console.WriteLine($"Section 1: Current Reader Position is 0x{NKMSortIndexFile.Position:X8}; expected Ptr Value is 0x{currPos - outFile.Header.HeaderSize:X8}");
                        outFile.Header.SectionPointers.Add(currPos); // should be 0x20
                        outFile.RaceNameBlock.Name = File.ReadAllLines(Path.Combine(arg0.FullName, "01 - Race Names.txt"), Encoding.UTF8).ToList();
                        outFile.RaceNameBlock.Write(NKMSortIndexFile);

                        currPos = (uint)NKMSortIndexFile.Position;
                        Console.WriteLine($"Section 2: Current Reader Position is 0x{NKMSortIndexFile.Position:X8}; expected Ptr Value is 0x{currPos - outFile.Header.HeaderSize:X8}");
                        outFile.Header.SectionPointers.Add(currPos); // should be 0x8F0
                        outFile.DemonNameBlock.Name = File.ReadAllLines(Path.Combine(arg0.FullName, "02 - Demon Names.txt"), Encoding.UTF8).ToList();
                        outFile.DemonNameBlock.Write(NKMSortIndexFile);

                        currPos = (uint)NKMSortIndexFile.Position;
                        Console.WriteLine($"Section 3: Current Reader Position is 0x{NKMSortIndexFile.Position:X8}; expected Ptr Value is 0x{currPos - outFile.Header.HeaderSize:X8}");
                        outFile.Header.SectionPointers.Add(currPos); // should be 0x9F20
                        outFile.DataSection3.Data = File.ReadAllBytes(Path.Combine(arg0.FullName, "03 - DataBlock3.dat")).ToList();
                        outFile.DataSection3.Write(NKMSortIndexFile);

                        currPos = (uint)NKMSortIndexFile.Position;
                        Console.WriteLine($"Section 4: Current Reader Position is 0x{NKMSortIndexFile.Position:X8}; expected Ptr Value is 0x{currPos - outFile.Header.HeaderSize:X8}");
                        outFile.Header.SectionPointers.Add(currPos); // should be 0x9F20
                        outFile.DataSection4.Data = File.ReadAllBytes(Path.Combine(arg0.FullName, "04 - DataBlock4.dat")).ToList();
                        outFile.DataSection4.Write(NKMSortIndexFile);

                        NKMSortIndexFile.Seek(0, SeekOrigin.Begin);
                        outFile.Header.Write(NKMSortIndexFile); // go back and fix header and write pointers
                    }
                }
                else Console.WriteLine("https://youtu.be/huTMNGaqoAA");
            }
        }
    }
}
