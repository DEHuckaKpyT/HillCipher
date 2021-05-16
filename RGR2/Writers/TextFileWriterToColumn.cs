using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR2
{
    class TextFileWriterToColumn : IWriterService
    {
        public string Path { get; }
        public bool Rewrite { get; }
        public Encoding Encoding { get; }
        public TextFileWriterToColumn(string path, bool rewrite = true)
        {
            Path = path;
            Rewrite = rewrite;
            Encoding = Encoding.ASCII;
        }
        public TextFileWriterToColumn(string path, Encoding encoding, bool rewrite = true)
        {
            Path = path;
            Encoding = encoding;
            Rewrite = rewrite;
        }
        public void WriteStrings(List<string> strings)
        {
            using (StreamWriter writer = new StreamWriter(Path, Rewrite, Encoding))
                foreach (string str in strings)
                    writer.WriteLine(str);
        }
    }
}
