using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RGR2
{
    class DecrypterWithStreams : Decrypter
    {
        public int StreamsCount { get; private set; }

        ManualResetEvent[] events;
        public DecrypterWithStreams(int streamsCount, string[] startWords, Dictionary<int, List<string>> allLengthDictionaries, List<int[,]> possibleMatrixes)
        {
            StartWords = startWords;
            AllLengthDictionaries = allLengthDictionaries;
            UsingMatrixes = possibleMatrixes;
            StreamsCount = streamsCount < startWords.Length ? streamsCount : startWords.Length;
            events = new ManualResetEvent[StreamsCount];
        }
        public override void Decrypt()
        {
            for (int i = 0; i < StreamsCount; i++)
            {
                events[i] = new ManualResetEvent(false);
                ProcessThreadGroundService treadGrond = new ProcessThreadGroundFastWay(events[i], i, StartWords[i],
                    UsingMatrixes,
                    AllLengthDictionaries[StartWords[i].Length].ToArray(),
                    AllLengthDictionaries[StartWords[i].Length - 1].ToArray());
                new Thread(new ThreadStart(treadGrond.TryToDecryptWord)).Start();
            }

            for (int i = StreamsCount; i < StartWords.Length; i++)
            {
                int numb = WaitHandle.WaitAny(events);
                events[numb].Reset();
                ProcessThreadGroundService treadGrond = new ProcessThreadGroundFastWay(events[numb], i, StartWords[i],
                    UsingMatrixes,
                    AllLengthDictionaries[StartWords[i].Length].ToArray(),
                    AllLengthDictionaries[StartWords[i].Length - 1].ToArray());
                new Thread(new ThreadStart(treadGrond.TryToDecryptWord)).Start();
            }

            WaitHandle.WaitAll(events);
        }
    }
}
