namespace SimpleCiphers.Models
{
    public interface ICipher
    {
        string Encrypt(string text, string key, string abc);
        string Decrypt(string text, string key, string abc);
        string[,] GetEncryptedAlphabet(string key, string abc);
        string[] GetRowAlphabet(string abc);
        string[] GetColAlphabet(string abc);
    }
}