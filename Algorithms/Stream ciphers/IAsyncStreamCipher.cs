using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoCore.Data;

namespace CryptoCore.Algorithms
{
    interface IAsyncStreamCipher
    {
        void Initialize(IKeyGenerator keyGenerator);

        Task<bool> Encrypt(string filePath, string InitialState, IProgress<ProgressChangedEventArgs> progress);

        Task<bool> Decrypt(string filePath, string InitialState, IProgress<ProgressChangedEventArgs> progress);
    } 
}
