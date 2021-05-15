using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR2
{
    //TODO доделать этот класс
    class FileProcessing
    {

        public static void RefactorResultFiles()
        {
            DirectoryInfo dir = new DirectoryInfo("EncryptedWords");
            var files = dir.GetFiles().OrderBy(x => int.Parse(x.Name.Substring(0, x.Name.IndexOf('.'))));

            Directory.CreateDirectory("Total");
            using (StreamWriter stream = new StreamWriter("Total\\EnWords.txt", false))
            using (StreamWriter stream2 = new StreamWriter("Total\\EnWords2.txt", false))
                foreach (var file in files)
                    RewriteFiles(stream, stream2, file.FullName, file.Name);
        }
        static void RewriteFiles(StreamWriter writer, StreamWriter writer2, string path, string name)
        {
            IReaderService readerService = new TextFileReader(path);
            List<string> strings = readerService.ReadStrings();

            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            foreach (var str in strings)
            {
                string[] strs = str.Split(' ');
                if (!dic.ContainsKey(strs[0]))
                    dic.Add(strs[0], "{" + $"{strs[1]} {strs[2]} {strs[3]} {strs[4]}" + "}");
                else
                    dic[strs[0]] = dic[strs[0]].Replace("}", "") + ", " + $"{strs[1]} {strs[2]} {strs[3]} {strs[4]}" + "}";
            }

            Directory.CreateDirectory("EncryptedStructuredWords");
            writer.WriteLine(name.Replace(".txt", ""));
            writer2.Write(name.Replace(".txt", " "));
            using (StreamWriter stream = new StreamWriter($"EncryptedStructuredWords\\{name}", false))
                foreach (var i in dic)
                {
                    stream.WriteLine(i.Key + " " + i.Value);
                    writer.WriteLine(i.Key);
                    writer2.Write(i.Key + " ");
                }
        }
    }
}
