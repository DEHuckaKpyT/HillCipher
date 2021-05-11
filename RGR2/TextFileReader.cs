using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR2
{
    class TextFileReader : IReaderService
    {
        public string Path { get; }
        public TextFileReader(string path)
        {
            Path = path;
        }
        public List<string> ReadStrings()
        {
            List<string> strings = new List<string>();
            using (StreamReader reader = new StreamReader(Path))
                while (!reader.EndOfStream)
                    strings.Add(reader.ReadLine());
            return strings;
        }
    }
}
