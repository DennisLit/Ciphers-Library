using System;
using CryptoCore.Data;

namespace CryptoCore.Algorithms
{
    public interface IKeyGenerator
    {
        byte[] GenerateKey();

        bool Initialize(string InitialState, long bytesMessageLength);
    }
} 
