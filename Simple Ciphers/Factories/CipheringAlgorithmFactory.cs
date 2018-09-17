namespace SimpleCiphers
{
    /// <summary>
    /// Factory for creating algorithm instance
    /// </summary>
    public class CipheringAlgorithmFactory : ICipheringAlgorithmFactory
    {
        public ICipheringAlgorithm NewAlgorithm(CiphersUsed Cipher)
        {
            switch(Cipher)
            {
                case CiphersUsed.Columns:
                    return new ColumnarTransposition();
                case CiphersUsed.RailFence:
                    return new RailFence();
                case CiphersUsed.Vigenere:
                    return new Vigenere();
                case CiphersUsed.Grille:
                    return new TurningGrille();
                default:
                    return new Vigenere();
            }
        }

        
    }
}
