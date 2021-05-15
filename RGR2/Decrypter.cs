using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RGR2
{
    abstract class Decrypter
    {
        public string[] StartWords { get; private protected set; }
        public Dictionary<int, List<string>> AllLengthDictionaries { get; private protected set; }
        public List<int[,]> UsingMatrixes { get; private protected set; }

        public abstract void Decrypt();
        
    }
}
