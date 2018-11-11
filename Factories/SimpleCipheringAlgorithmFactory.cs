using CiphersLibrary.Algorithms;
using CiphersLibrary.Data;

namespace CiphersLibrary.Core
{
    /// <summary>
    /// Factory for creating simple algorithm instance
    /// </summary>
    public class SimpleCipheringAlgorithmFactory : ISimpleCipheringAlgorithmFactory
    {
        public ISimpleCipheringAlgorithm NewAlgorithm(SimpleCiphers Cipher)
        {
            switch(Cipher)
            {
                case SimpleCiphers.Columns:
                    return new ColumnarTransposition();
                case SimpleCiphers.RailFence:
                    return new RailFence();
                case SimpleCiphers.Vigenere:
                    return new Vigenere();
                case SimpleCiphers.Grille:
                    return new TurningGrille();
                default:
                    return new Vigenere();
            }
        }

        
    }
}
