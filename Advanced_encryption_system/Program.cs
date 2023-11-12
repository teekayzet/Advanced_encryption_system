using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LastWillAndTestament
{
    public class Will
    {
        private const string WillFilePath = "will.txt";
        private const string EncryptionKeyFilePath = "encryptionKey.txt";
        private const int KeySize = 256;
        private const int BlockSize = 128;
        private const int Iterations = 10000;

        private static readonly byte[] Salt = new byte[] { 0x26, 0x19, 0x91, 0x45, 0x78, 0x3F, 0x2A, 0xBC };

        public void CreateWill()
        {
            Console.WriteLine("Creating a new will...");

            Console.Write("Enter a 16-digit key combination: ");
            string accessKey = Console.ReadLine()!;
            
            Console.Write("Enter a 64-character combination key: ");
            string encryptionKey = Console.ReadLine()!;

            using (var aes = CreateAes(encryptionKey))
            {
                Console.Write("Enter your will: ");
                string willContent = Console.ReadLine()!;

                byte[] encryptedBytes = EncryptTextToBytes(willContent, aes.Key, aes.IV);

                File.WriteAllBytes(WillFilePath, encryptedBytes);
                File.WriteAllText(EncryptionKeyFilePath, encryptionKey);
            }

            Console.WriteLine("Will created successfully.");
        }

        public void ReadWill()
        {
            if (!File.Exists(WillFilePath) || !File.Exists(EncryptionKeyFilePath))
            {
                Console.WriteLine("No will found.");
                return;
            }

            Console.Write("Enter the 16-digit key combination to access the will: ");
            string accessKey = Console.ReadLine()!;

            if (!ValidateAccessKey(accessKey))
            {
                Console.WriteLine("Invalid access key.");
                return;
            }

            Console.WriteLine("Reading the will...");

            string encryptionKey = File.ReadAllText(EncryptionKeyFilePath);

            using (var aes = CreateAes(encryptionKey))
            {
                byte[] encryptedBytes = File.ReadAllBytes(WillFilePath);

                string decryptedText = DecryptBytesToText(encryptedBytes, aes.Key, aes.IV);

                Console.WriteLine("Will content:");
                Console.WriteLine(decryptedText);
            }
        }

        public void EditWill()
        {
            if (!File.Exists(WillFilePath) || !File.Exists(EncryptionKeyFilePath))
            {
                Console.WriteLine("No will found.");
                return;
            }

            Console.Write("Enter the 16-digit key combination to access the will: ");
            string accessKey = Console.ReadLine()!;

            if (!ValidateAccessKey(accessKey))
            {
                Console.WriteLine("Invalid access key.");
                return;
            }

            Console.Write("Enter the 64-character combination key to edit the will: ");
            string encryptionKey = Console.ReadLine()!;

            if (!ValidateEncryptionKey(encryptionKey))
            {
                Console.WriteLine("Invalid encryption key.");
                return;
            }

            using (var aes = CreateAes(encryptionKey))
            {
                Console.WriteLine("Enter your updated will: ");
                string willContent = Console.ReadLine()!;

                byte[] encryptedBytes = EncryptTextToBytes(willContent, aes.Key, aes.IV);

                File.WriteAllBytes(WillFilePath, encryptedBytes);
            }

            Console.WriteLine("Will updated successfully.");
        }

        public void DeleteWill()
        {
            if (!File.Exists(WillFilePath) || !File.Exists(EncryptionKeyFilePath))
            {
                Console.WriteLine("No will found.");
                return;
            }

            Console.Write("Enter the 16-digit key combination to access the will: ");
            string accessKey = Console.ReadLine()!;

            if (!ValidateAccessKey(accessKey))
            {
                Console.WriteLine("Invalid access key.");
                return;
            }

            Console.Write("Enter the 64-character combination key to delete the will: ");
            string encryptionKey = Console.ReadLine()!;

            if (!ValidateEncryptionKey(encryptionKey))
            {
                Console.WriteLine("Invalid encryption key.");
                return;
            }

            File.Delete(WillFilePath);
            File.Delete(EncryptionKeyFilePath);

            Console.WriteLine("Will deleted successfully.");
        }

        private bool ValidateAccessKey(string accessKey)
        {
            // Add your validation logic here
            return accessKey.Length == 16;
        }

        private bool ValidateEncryptionKey(string encryptionKey)
        {
            // Add your validation logic here
            return encryptionKey.Length == 64;
        }

        private Aes CreateAes(string encryptionKey)
        {
            var aes = Aes.Create();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Generate a unique IV using RandomNumberGenerator
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                var iv = new byte[aes.BlockSize / 8];
                rng.GetBytes(iv);
                aes.IV = iv;
            }

            // Use a secure method to derive the key
            using (var deriveBytes = new Rfc2898DeriveBytes(encryptionKey, Salt, Iterations, HashAlgorithmName.SHA256))
            {
                var key = deriveBytes.GetBytes(KeySize / 8);
                aes.Key = key;
            }

            return aes;
        }
        private byte[] EncryptTextToBytes(string plainText, byte[] key, byte[] iv)
        {
            byte[] encryptedBytes;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor())
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    encryptedBytes = memoryStream.ToArray();
                }
            }

            return encryptedBytes;
        }

        private string DecryptBytesToText(byte[] encryptedBytes, byte[] key, byte[] iv)
        {
            string decryptedText;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var memoryStream = new MemoryStream(encryptedBytes))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    decryptedText = streamReader.ReadToEnd();
                }
            }

            return decryptedText;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var will = new Will();

            Console.WriteLine("Last Will and Testament");
            Console.WriteLine("------------------------");

            while (true)
            {
                Console.WriteLine("1. Create a new will");
                Console.WriteLine("2. Read the will");
                Console.WriteLine("3. Edit the will");
                Console.WriteLine("4. Delete the will");
                Console.WriteLine("5. Exit");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        will.CreateWill();
                        Console.Clear();
                        break;
                    case "2":
                        will.ReadWill();
                        Console.Clear();
                        break;
                    case "3":
                        will.EditWill();
                        Console.Clear();
                        break;
                    case "4":
                        will.DeleteWill();
                        Console.Clear();
                        break;
                    case "5":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}
