using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VG.EncryptLib.Shared
{
    //public class RijndaelSimple
    //{
    //    //http://vi.wikipedia.org/wiki/AES_(m%C3%A3_h%C3%B3a)

    //    public const string PassPhrase = "S@$@)FHJHLSAIL:FAHSL.TTDPHH";   // can be any string.
    //    public const string HashAlgorithm = "SHA1";                 // can be "MD5". Hàm băm key
    //    public const int PasswordIterations = 2;                    // can be any number
    //    public const string InitVector = "@07202014-toanvt";        // must be 16 bytes
    //    public const int KeySize = 256;                             // can be 192 or 128

    //    /// <summary>
    //    /// Encrypt byte[] data using Rijndael
    //    /// </summary>
    //    /// <param name="plainTextBytesm"></param>
    //    /// <param name="SaltValue"></param>
    //    /// <returns></returns>
    //    public byte[] Encrypt(byte[] plainTextBytesm, string SaltValue)
    //    {
    //        return Encrypt(plainTextBytesm, PassPhrase, SaltValue, HashAlgorithm, PasswordIterations, InitVector, KeySize);
    //    }

    //    private byte[] Encrypt(byte[] plainTextBytes,
    //                                 string passPhrase,
    //                                 string saltValue,
    //                                 string hashAlgorithm,
    //                                 int passwordIterations,
    //                                 string initVector,
    //                                 int keySize)
    //    {
    //        // Convert strings into byte arrays.
    //        // Let us assume that strings only contain ASCII codes.
    //        // If strings include Unicode characters, use Unicode, UTF7, or UTF8
    //        // encoding.
    //        byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
    //        byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

    //        // Convert our plaintext into a byte array.
    //        // Let us assume that plaintext contains UTF8-encoded characters.
    //        // byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

    //        // First, we must create a password, from which the key will be derived.
    //        // This password will be generated from the specified passphrase and
    //        // salt value. The password will be created using the specified hash
    //        // algorithm. Password creation can be done in several iterations.
    //        var password = new PasswordDeriveBytes(
    //            passPhrase,
    //            saltValueBytes,
    //            hashAlgorithm,
    //            passwordIterations);

    //        // Use the password to generate pseudo-random bytes for the encryption
    //        // key. Specify the size of the key in bytes (instead of bits).
    //        byte[] keyBytes = password.GetBytes(keySize / 8);

    //        // Create uninitialized Rijndael encryption object.
    //        var symmetricKey = new RijndaelManaged();

    //        // It is reasonable to set encryption mode to Cipher Block Chaining
    //        // (CBC). Use default options for other symmetric key parameters.
    //        symmetricKey.Mode = CipherMode.CBC;

    //        // Generate encryptor from the existing key bytes and initialization
    //        // vector. Key size will be defined based on the number of the key
    //        // bytes.
    //        ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
    //            keyBytes,
    //            initVectorBytes);

    //        // Define memory stream which will be used to hold encrypted data.
    //        var memoryStream = new MemoryStream();

    //        // Define cryptographic stream (always use Write mode for encryption).
    //        var cryptoStream = new CryptoStream(memoryStream,
    //                                                     encryptor,
    //                                                     CryptoStreamMode.Write);
    //        // Start encrypting.
    //        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

    //        // Finish encrypting.
    //        cryptoStream.FlushFinalBlock();

    //        // Convert our encrypted data from a memory stream into a byte array.
    //        byte[] cipherTextBytes = memoryStream.ToArray();

    //        // Close both streams.
    //        memoryStream.Close();
    //        cryptoStream.Close();

    //        // Convert encrypted data into a base64-encoded string.
    //        //string cipherText = Convert.ToBase64String(cipherTextBytes);

    //        //// Return encrypted string.
    //        //return cipherText;
    //        return cipherTextBytes;
    //    }

    //    /// <summary>
    //    /// Decrypt byte[] data using Rijndael
    //    /// </summary>
    //    /// <param name="cipher"></param>
    //    /// <param name="SaltValue"></param>
    //    /// <returns></returns>
    //    public byte[] Decrypt(byte[] cipher, string SaltValue)
    //    {
    //        return Decrypt(cipher, PassPhrase, SaltValue, HashAlgorithm, PasswordIterations, InitVector, KeySize);
    //    }

    //    private byte[] Decrypt(byte[] cipherTextBytes,
    //                                 string passPhrase,
    //                                 string saltValue,
    //                                 string hashAlgorithm,
    //                                 int passwordIterations,
    //                                 string initVector,
    //                                 int keySize)
    //    {
    //        // Convert strings defining encryption key characteristics into byte
    //        // arrays. Let us assume that strings only contain ASCII codes.
    //        // If strings include Unicode characters, use Unicode, UTF7, or UTF8
    //        // encoding.
    //        byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
    //        byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

    //        // Convert our ciphertext into a byte array.
    //        //byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

    //        // First, we must create a password, from which the key will be
    //        // derived. This password will be generated from the specified
    //        // passphrase and salt value. The password will be created using
    //        // the specified hash algorithm. Password creation can be done in
    //        // several iterations.
    //        PasswordDeriveBytes password = new PasswordDeriveBytes(
    //            passPhrase,
    //            saltValueBytes,
    //            hashAlgorithm,
    //            passwordIterations);

    //        // Use the password to generate pseudo-random bytes for the encryption
    //        // key. Specify the size of the key in bytes (instead of bits).
    //        byte[] keyBytes = password.GetBytes(keySize / 8);

    //        // Create uninitialized Rijndael encryption object.
    //        RijndaelManaged symmetricKey = new RijndaelManaged();

    //        // It is reasonable to set encryption mode to Cipher Block Chaining
    //        // (CBC). Use default options for other symmetric key parameters.
    //        symmetricKey.Mode = CipherMode.CBC;

    //        // Generate decryptor from the existing key bytes and initialization
    //        // vector. Key size will be defined based on the number of the key
    //        // bytes.
    //        ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
    //            keyBytes,
    //            initVectorBytes);

    //        // Define memory stream which will be used to hold encrypted data.
    //        MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

    //        // Define cryptographic stream (always use Read mode for encryption).
    //        CryptoStream cryptoStream = new CryptoStream(memoryStream,
    //                                                     decryptor,
    //                                                     CryptoStreamMode.Read);

    //        // Since at this point we don't know what the size of decrypted data
    //        // will be, allocate the buffer long enough to hold ciphertext;
    //        // plaintext is never longer than ciphertext.
    //        byte[] plainTextBytes = new byte[cipherTextBytes.Length];

    //        // Start decrypting.
    //        int decryptedByteCount = cryptoStream.Read(plainTextBytes,
    //                                                   0,
    //                                                   plainTextBytes.Length);

    //        // Close both streams.
    //        memoryStream.Close();
    //        cryptoStream.Close();

    //        // Convert decrypted data into a string.
    //        // Let us assume that the original plaintext string was UTF8-encoded.
    //        //string plainText = Encoding.UTF8.GetString(plainTextBytes,
    //        //                                           0,
    //        //                                           decryptedByteCount);

    //        //// Return decrypted string.
    //        //return plainText;
    //        return plainTextBytes.Take(decryptedByteCount).ToArray();
    //    }
    //}

    public class RijndaelSimple
    {
        //http://vi.wikipedia.org/wiki/AES_(m%C3%A3_h%C3%B3a)

        public const string PassPhrase = "S@$@)FHJHLSAIL:FAHSL.TTDPHH";   // can be any string.
        public const string HashAlgorithm = "SHA1";                 // can be "MD5". Hàm băm key
        public const int PasswordIterations = 2;                    // can be any number
        public const string InitVector = "@07202014-toanvt";        // must be 16 bytes
        public const int KeySize = 256;                             // can be 192 or 128

        //public const string PassPhrase = "S@$@)FHJHLSAIL:FAHSL.";   // can be any string.
        //public const string HashAlgorithm = "SHA1";                 // can be "MD5". Hàm băm key
        //public const int PasswordIterations = 2;                    // can be any number
        //public const string InitVector = "@15081987-sanghv";        // must be 16 bytes
        //public const int KeySize = 256;                             // can be 192 or 128
        public string PassHas { get; set; }

        public byte[] Encrypt(byte[] plainTextBytesm, string SaltValue)
        {
            return Encrypt(plainTextBytesm, PassPhrase, SaltValue, HashAlgorithm, PasswordIterations, InitVector, KeySize);
        }

        private byte[] Encrypt(byte[] plainTextBytes,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize)
        {
            // Convert strings into byte arrays.
            // Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8 
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our plaintext into a byte array.
            // Let us assume that plaintext contains UTF8-encoded characters.
            // byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // First, we must create a password, from which the key will be derived.
            // This password will be generated from the specified passphrase and 
            // salt value. The password will be created using the specified hash 
            // algorithm. Password creation can be done in several iterations.
            //var password = new PasswordDeriveBytes(
            //    passPhrase,
            //    saltValueBytes,
            //    hashAlgorithm,
            //    passwordIterations);

            //// Use the password to generate pseudo-random bytes for the encryption
            //// key. Specify the size of the key in bytes (instead of bits).
            //byte[] keyBytes = password.GetBytes(keySize / 8);
            Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations);
            byte[] keyBytes = password.GetBytes(256 / 8);
            PassHas = Convert.ToBase64String(keyBytes);

            //Console.WriteLine(t1);

            // Create uninitialized Rijndael encryption object.
            var symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate encryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(
                keyBytes,
                initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            var memoryStream = new MemoryStream();

            // Define cryptographic stream (always use Write mode for encryption).
            var cryptoStream = new CryptoStream(memoryStream,
                                                         encryptor,
                                                         CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            //string cipherText = Convert.ToBase64String(cipherTextBytes);

            //// Return encrypted string.
            //return cipherText;
            return cipherTextBytes;
        }

        public byte[] Decrypt(byte[] cipher, string SaltValue)
        {
            return Decrypt(cipher, PassPhrase, SaltValue, HashAlgorithm, PasswordIterations, InitVector, KeySize);
        }
        private byte[] Decrypt(byte[] cipherTextBytes,
                                     string passPhrase,
                                     string saltValue,
                                     string hashAlgorithm,
                                     int passwordIterations,
                                     string initVector,
                                     int keySize)
        {
            // Convert strings defining encryption key characteristics into byte
            // arrays. Let us assume that strings only contain ASCII codes.
            // If strings include Unicode characters, use Unicode, UTF7, or UTF8
            // encoding.
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

            // Convert our ciphertext into a byte array.
            //byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            // First, we must create a password, from which the key will be 
            // derived. This password will be generated from the specified 
            // passphrase and salt value. The password will be created using
            // the specified hash algorithm. Password creation can be done in
            // several iterations.
            //PasswordDeriveBytes password = new PasswordDeriveBytes(
            //    passPhrase,
            //    saltValueBytes,
            //    hashAlgorithm,
            //    passwordIterations);
            Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhrase, saltValueBytes, passwordIterations);
            //byte[] keyBytes = password.GetBytes(256 / 8);
            //PassHas = Convert.ToBase64String(keyBytes);

            // Use the password to generate pseudo-random bytes for the encryption
            // key. Specify the size of the key in bytes (instead of bits).
            byte[] keyBytes = password.GetBytes(256 / 8);

            // Create uninitialized Rijndael encryption object.
            RijndaelManaged symmetricKey = new RijndaelManaged();

            // It is reasonable to set encryption mode to Cipher Block Chaining
            // (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC;

            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(
                keyBytes,
                initVectorBytes);

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                         decryptor,
                                                         CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold ciphertext;
            // plaintext is never longer than ciphertext.
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            // Start decrypting.
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,
                                                       0,
                                                       plainTextBytes.Length);

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();


            // Convert decrypted data into a string. 
            // Let us assume that the original plaintext string was UTF8-encoded.
            //string plainText = Encoding.UTF8.GetString(plainTextBytes,
            //                                           0,
            //                                           decryptedByteCount);

            //// Return decrypted string.   
            //return plainText;
            return plainTextBytes.Take(decryptedByteCount).ToArray();
        }
    }
}