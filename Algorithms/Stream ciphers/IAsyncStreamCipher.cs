using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CiphersLibrary.Data;

namespace CiphersLibrary.Algorithms
{
    interface IAsyncStreamCipher
    {
        void Initialize(IKeyGenerator keyGenerator);

        Task Encrypt(string filePath, string InitialState, IProgress<ProgressChangedEventArgs> progress);

        Task Decrypt(string filePath, string InitialState, IProgress<ProgressChangedEventArgs> progress);
    } 
}
