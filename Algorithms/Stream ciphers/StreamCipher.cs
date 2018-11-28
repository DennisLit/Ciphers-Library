using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiphersLibrary.Data;

namespace CiphersLibrary.Algorithms
{
    public class StreamCipher : IAsyncStreamCipher
    { 
        //some really big buffer to work with big files
        private static int BufferDefaultLength => 1024 * 1024 * 16;
         
        private IKeyGenerator KeyGeneratorInstance { get; set; }

        public void Initialize(IKeyGenerator keyGenerator)
        {
            this.KeyGeneratorInstance = keyGenerator;
        }

        public async Task Encrypt(string filePath, string InitialState, IProgress<ProgressChangedEventArgs> progress)
        {
            try
            {
                var KeyBuf = new byte[BufferDefaultLength];
                var MessageBuf = new byte[BufferDefaultLength];
                var OutputBuf = new byte[BufferDefaultLength];

                KeyGeneratorInstance.Initialize(InitialState, BufferDefaultLength);

                //key buffer

                KeyBuf = await Task.Run(() => KeyGeneratorInstance.GenerateKey());

                using (var OutFStream = new FileStream(GetOutputPath(filePath), FileMode.Create))
                using (var secBinStream = new BinaryWriter(OutFStream))
                {
                    using (var FsStream = new FileStream(filePath, FileMode.Open))
                    using (var binStream = new BinaryReader(FsStream))
                    {

                        int fullCyclesAmount = (binStream.BaseStream.Length / BufferDefaultLength < 1) ? 1 : (int)binStream.BaseStream.Length / BufferDefaultLength;
                        int currentCycle = 1;

                        while (binStream.BaseStream.Position != binStream.BaseStream.Length)
                        {

                            MessageBuf = await Task.Run(() => binStream.ReadBytes(BufferDefaultLength));

                            //use same keyBuf because period 
                            //of our LFSR equals to 2^m where m is highest power in polynom

                            OutputBuf = await Task.Run(() => XorArrays(MessageBuf, KeyBuf));

                            progress.Report(new ProgressChangedEventArgs() { value = (int)( ((float)currentCycle / fullCyclesAmount) * 100 )});

                            await Task.Run(() => secBinStream.Write(OutputBuf));

                            ++currentCycle;

                            //Used to output data to user 
                        }
                    }
                }

            }
            catch(OutOfMemoryException)
            {
                throw;
            }
            catch(ArgumentException)
            {
                throw;
            }
            
        }

        public async Task Decrypt(string filePath, string InitialState, IProgress<ProgressChangedEventArgs> progress)
        {
            await Encrypt(filePath, InitialState, progress).ConfigureAwait(false);
        }

        public byte[] XorArrays(byte[] first, byte[] second)
        {
            var ListToReturn = new List<byte>();

            for (int i = 0; i < first.Length; ++i)
            {
                ListToReturn.Add((byte)(first[i] ^ second[i]));
            }

            return ListToReturn.ToArray();
        }

        /// <summary>
        /// Gets string with any extension; 
        /// Returns a string with .encrypted extension
        /// </summary>
        /// <param name="InputPath"></param>
        /// <returns></returns>
        private string GetOutputPath(string InputPath)
        {
            return InputPath.Insert(InputPath.LastIndexOf('.'), "Encrypted");
        }
    }
}
