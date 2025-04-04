namespace BookPro.Infrastructure;

public static class CryptHelper
{
    public static string EncryptString(string text, string key, string iv)
    {
        byte[] keyByte = Convert.FromBase64String(key);
        byte[] ivByte = Convert.FromBase64String(iv);

        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = keyByte;
            aesAlg.IV = ivByte;
            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream())
            {

                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }

        }

    }

    public static string DescryptString(string textEncrypt, byte[] key, byte[] iv)
    {
        var aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;

        var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        var msDecrypt = new MemoryStream(Convert.FromBase64String(textEncrypt));
        var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        var srDecrypt = new StreamReader(csDecrypt);

        return srDecrypt.ReadToEnd();
    }
}
