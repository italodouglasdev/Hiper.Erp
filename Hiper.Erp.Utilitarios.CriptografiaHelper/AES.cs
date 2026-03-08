using System.Security.Cryptography;
using System.Text;

namespace Hiper.Adm.Utilitarios.CriptografiaHelper
{
    public class AES
    {
        private static readonly string ChaveBase = "SUA_CHAVE_SUPER_SECRETA_32CHAR";
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes

        private static byte[] GerarChave(string chaveCampo)
        {
            using var derive = new Rfc2898DeriveBytes(
                ChaveBase + chaveCampo,
                IV,
                10000,
                HashAlgorithmName.SHA256);

            return derive.GetBytes(32);
        }

        public static string Encrypt(string texto, string chaveCampo)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;

            using var aes = Aes.Create();

            aes.Key = GerarChave(chaveCampo);
            aes.IV = IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(texto);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string textoCriptografado, string chaveCampo)
        {
            if (string.IsNullOrEmpty(textoCriptografado))
                return textoCriptografado;

            using var aes = Aes.Create();

            aes.Key = GerarChave(chaveCampo);
            aes.IV = IV;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            var buffer = Convert.FromBase64String(textoCriptografado);

            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}