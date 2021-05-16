using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RGR2
{
    abstract class ProcessThreadGroundService
    {
        public ManualResetEvent ResetEvent { get; private protected set; }
        public int Number { get; private protected set; }
        public string Word { get; private protected set; }
        public List<int[,]> AllMatrixes { get; private protected set; }
        public string[] DictionaryEvenLength { get; private protected set; }
        public string[] DictionaryOddLength { get; private protected set; }
        public string PathDirectoryEncryptedWords { get; private protected set; }
        public abstract void TryToDecryptWord();
    }
}
