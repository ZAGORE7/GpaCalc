using Ozbul.Application.Portal.Repository.Entities;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Ozbul.Application.Portal.Services.Abstractions
{


    public class DesKeyManager
    {
        private readonly byte[] _desKey;
        private readonly byte[] _IV;


        public DesKeyManager()
        {
            _desKey = GenerateDesKey();
            _IV = GenerateRandomIV();
            
        }

        public byte[] GenerateDesKey()
        {
            using (var desProvider = new DESCryptoServiceProvider())
            {
                desProvider.GenerateKey();
                return desProvider.Key;
            }
        }
        public byte[] GenerateRandomIV()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                
                byte[] iv = new byte[8]; // Adjust the size based on the block size of the encryption algorithm
                rng.GetBytes(iv);
                return iv;
            }
        }

        public byte[] EncryptData(string plainText, byte[] desKey, byte[] IV)
        {
            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                desProvider.Key = desKey;
                desProvider.IV = IV;
                desProvider.Padding = PaddingMode.PKCS7; // You may need to set a proper IV; here, it's set to all zeros

                using (ICryptoTransform encryptor = desProvider.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedData = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return encryptedData;
                }
            }
        }
        public byte[] EncryptInt(int value, byte[] desKey, byte[]IV)
        {
            // Convert the int value to bytes
            byte[] intBytes = BitConverter.GetBytes(value);

            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                desProvider.Key = desKey;
                desProvider.IV = IV;
                desProvider.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = desProvider.CreateEncryptor())
                {
                    // Encrypt the bytes representing the int value
                    byte[] encryptedData = encryptor.TransformFinalBlock(intBytes, 0, intBytes.Length);
                    return encryptedData;
                }
            }
        }

        public byte[] EncryptDateTime(DateTime dateTime, byte[] desKey, byte[] IV)
        {
            byte[] dateTimeBytes = BitConverter.GetBytes(dateTime.ToBinary());
            return EncryptData(Convert.ToBase64String(dateTimeBytes),desKey,IV);
        }

        public string DecryptData(byte[] encryptedData, byte[] desKey, byte[]IV )
        {
            using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
            {
                desProvider.Key = desKey;
                desProvider.IV = IV;
                desProvider.Padding = PaddingMode.PKCS7; // Specify the padding mode

                using (ICryptoTransform decryptor = desProvider.CreateDecryptor())
                {
                    byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    return Encoding.UTF8.GetString(decryptedData);
                }
            }
        }


        public DateTime DecryptDateTime(byte[] encryptedData, byte[] desKey, byte[] IV)
        {
            string base64Encoded = DecryptData(encryptedData,desKey,IV);
            byte[] dateTimeBytes = Convert.FromBase64String(base64Encoded);
            long ticks = BitConverter.ToInt64(dateTimeBytes, 0);
            return DateTime.FromBinary(ticks);
        }
        public int DecryptInt(byte[] encryptedData, byte[] desKey, byte[] IV)
        {
            string decryptedString = DecryptData(encryptedData, desKey,IV);

            // Convert the decrypted string to an integer
            if (int.TryParse(decryptedString, out int decryptedInt))
            {
                return decryptedInt;
            }

            // Handle the case where the decryption result is not a valid integer
            throw new InvalidOperationException("Decryption result is not a valid integer.");
        }
    }

}
