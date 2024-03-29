﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSA_Algorithm
{
    class RSA_Algorithm
    {
        private List<int> asciiList;
        private BigInteger p { get; set; }
        private BigInteger q { get; set; }
        private BigInteger n { get; set; }
        private BigInteger phi { get; set; }
        private BigInteger e { get; set; } 
        private BigInteger d { get; set; }

        private BigInteger[] encryptedText;

        public string Encryption()
        {
            try
            {
                this.e = e;
                string result = string.Empty;
                Console.WriteLine(e + "<- E encrypt");
                foreach (int i in asciiList)
                {
                    result += BigInteger.Pow(i, (int)e) % n + " "; // We convert e int to not take up a lot of memory
                }
                result = result.Substring(0, result.Length - 1);


                SQL sql = new SQL();
                sql.InsertValues(result, e, n);
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Encryption ex {0}", ex);
                return null;
            }
        }

        public string Decryption()
        {
            try
            {
                Console.WriteLine("ENCRYPTED TEXT {0}", Convert.ToString(encryptedText));
                Console.WriteLine("N " + n);
                char[] c = new char[encryptedText.Length];

                for (int i = 0; i < encryptedText.Length; i++)
                {
                    c[i] = (char)(BigInteger.Pow(encryptedText[i], (int)d) % n);
                }
                return new string(c);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption ex {0}", ex);
                return null;
            }
        }

        public void rsaEncryption(string text, BigInteger p, BigInteger q)
        {
            try
            {
                asciiList = new List<int>();
                foreach (char c in text)
                {
                    asciiList.Add(Convert.ToInt32(c));
                }
                Console.WriteLine(p + " <- This is P");
                Console.WriteLine(q + " <- This is Q");

                n = p * q;
                phi = (p - 1) * (q - 1);
                Console.WriteLine(e + " <- E");
                // E should not share the same factor with phi
                for (BigInteger i = 2; i < phi; i++) // We choose the largest x point
                {
                    if (HelperMethods.checkPrime(i) && HelperMethods.checkCoPrime(i, phi) && i != p && i != q && i != phi)
                    {
                        e = i;
                        break;
                    }
                }
                Console.WriteLine(n + " <- N");
                Console.WriteLine(phi + " <- Phi");
                Console.WriteLine(e + " <- E");
                //d = (2 * phi + 1) / e;
                HelperMethods.TryModInverse(e, phi, out BigInteger result);
                d = result;
                Console.WriteLine(d + " <- D");
                //Encryption();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption ex {0}", ex);
            }
        }

        public void rsaDecryption(BigInteger[] encrypted, BigInteger n, BigInteger e)
        {
            this.encryptedText = encrypted; 
            this.n = n;
            this.e = e;
            p = 2;
            try
            {
                while (n % p > 0)
                {
                    p++;
                }
                q = n / p;
                Console.WriteLine(p + "<- P decryption");
                Console.WriteLine(q + "<- Q decryption");
                phi = (p - 1) * (q - 1);
                Console.WriteLine(phi + "<- Phi decryption");
                HelperMethods.TryModInverse(e, phi, out BigInteger result);
                d = result;
                Console.WriteLine(d + "<- d decryption");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption ex {0}", ex);
            }
        }
    }
}
