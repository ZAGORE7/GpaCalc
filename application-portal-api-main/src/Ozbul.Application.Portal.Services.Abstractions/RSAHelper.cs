using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ozbul.Application.Portal.Services.Abstractions
{
    public class RSAHelper
    {
        // Generate RSA key pair
        public static void GenerateKeyPair(out string publicKey, out string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }
        }

        // Encrypt data using RSA public key
        public  byte[] EncryptData(string data, string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                return rsa.Encrypt(dataBytes, false);
            }
        }
        public byte[] EncryptInt(int data, string publicKey)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);

                    byte[] dataBytes = BitConverter.GetBytes(data);
                    byte[] encryptedData = rsa.Encrypt(dataBytes, true);

                    return encryptedData;
                }
            }
            catch (Exception ex)
            {
                // Handle encryption exception
                Console.WriteLine($"Encryption failed: {ex.Message}");
                return null;
            }
        }

        // Decrypt data using RSA private key
        public  string DecryptData(byte[] encryptedData, string privateKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] decryptedData = rsa.Decrypt(encryptedData, false);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }
        public int DecryptInt(byte[] encryptedData, string privateKey)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);

                    byte[] decryptedData = rsa.Decrypt(encryptedData, true);
                    int decryptedInt = BitConverter.ToInt32(decryptedData, 0);

                    return decryptedInt;
                }
            }
            catch (Exception ex)
            {
                // Handle decryption exception
                Console.WriteLine($"Decryption failed: {ex.Message}");
                return 0; // Return a default value or handle accordingly
            }
        }
        public byte[] EncryptDouble(double data, string publicKey)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);

                    byte[] dataBytes = BitConverter.GetBytes(data);
                    byte[] encryptedData = rsa.Encrypt(dataBytes, true);

                    return encryptedData;
                }
            }
            catch (Exception ex)
            {
                // Handle encryption exception
                Console.WriteLine($"Encryption failed: {ex.Message}");
                return null;
            }
        }

        public double DecryptDouble(byte[] encryptedData, string privateKey)
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(privateKey);

                    byte[] decryptedData = rsa.Decrypt(encryptedData, true);
                    double decryptedDouble = BitConverter.ToDouble(decryptedData, 0);

                    return decryptedDouble;
                }
            }
            catch (Exception ex)
            {
                // Handle decryption exception
                Console.WriteLine($"Decryption failed: {ex.Message}");
                return 0.0; // Return a default value or handle accordingly
            }
        }
    }
}
