using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace MerkleRoot
{
    class Program
    {
        public static void Main()
        {
            string[] transactionBlockHashes = {
                "8c14f0db3df150123e6f3dbbf30f8b955a8249b62ac1d1ff16284aefa3d06d87",
                "fff2525b8931402dd09222c50775608f75787bd2b87e56995a7bdd30f79702c4",
                "6359f0868171b1d194cbee1af2f16ea598ae8fad666d9b012c8ed2b79a236ec4",
                "e9a66845e05d5abc0ad04ec80f774a7e585c6e8db975962d069a522137b80c1d"
            };
            Console.WriteLine(CalculateMerkleRoot(transactionBlockHashes));
        }

        public static string CalculateMerkleRoot(string[] transactions)
        {
            while (true)
            {
                if (transactions.Length == 1)
                {
                    return transactions[0];
                }

                List<string> newHashList = new List<string>();

                int lengthOfT = 0;
                if (transactions.Length % 2 != 0)
                {
                    lengthOfT = transactions.Length - 1;
                }
                else
                {
                    lengthOfT = transactions.Length;
                }

                for (int i = 0; i < lengthOfT; i = i + 2)
                {
                    newHashList.Add(CalculateHash(transactions[i], transactions[i + 1]));
                }
                if (lengthOfT < transactions.Length)
                {
                    newHashList.Add(CalculateHash(transactions[transactions.Length - 1], transactions[transactions.Length - 1]));
                }
                transactions = newHashList.ToArray();

            }
        }


        static string CalculateHash(string a, string b)
        {

            byte[] a1 = StringToByteArray(a);
            Array.Reverse(a1);
            byte[] b1 = StringToByteArray(b);
            Array.Reverse(b1);
            var c = a1.Concat(b1).ToArray();
            SHA256 sha256 = SHA256.Create();
            byte[] firstHash = sha256.ComputeHash(c);
            byte[] hashOfHash = sha256.ComputeHash(firstHash);
            Array.Reverse(hashOfHash);
            string hashStr = BitConverter.ToString(hashOfHash);
            return hashStr.Replace("-", "").ToLower();
        }
        //f3e94742aca4b5ef85488dc37c06c3282295ffec960994b2c0d5ac2a25a95766

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
