namespace SimpleCiphers
{
    interface ICipheringAlgorithmFactory
    {
        ICipheringAlgorithm NewAlgorithm(CiphersUsed Cipher);
    }
}
