using System;
using CiphersLibrary.Data;

namespace CiphersLibrary.Algorithms
{
    public interface IKeyGenerator
    {
        byte[] GenerateKey();

        void Initialize(string InitialState, long bytesMessageLength);
    }
} 
