using System;
using Cryptography.PolyCrypt.CryptoCraftConsole.Utility;
using PolyCrypt;

namespace Cryptography.PolyCrypt.CryptoCraftConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region get parameters

            const string requiredParams = @"a|password";
            const string optionalParams = @"n|v|outgroups|p|c";

            var prams = new ParseRunArguments(args, requiredParams, optionalParams);

            // Needs to have an plain message or a crypt message param
            // bool hasMessageParam = prams.ContainsKey("p") || prams.ContainsKey("c");

            if (prams.HasErrors)
            {
                Console.WriteLine(string.Join("\n", prams.ErrorMessages()));
                WriteHelp();
                return;
            }

            #endregion

            #region generate poly alphabets

            var alphabetindex
                = prams.ContainsKey("a")
                    ? Convert.ToInt32(prams["a"].Value)
                    : 0; // Default to alphabet zero

            var password =
                prams.ContainsKey("password")
                    ? prams["password"].Value
                    : prams.ContainsKey("pswd")
                        ? prams["pswd"].Value
                        : string.Empty;

            var numberOfAlphabets = prams.ContainsKey("n") ? Convert.ToInt32(prams["n"].Value) : 1;

            var crypt = prams.ContainsKey("v")
                ? new Crypt(alphabetindex, password)
                : new Crypt(alphabetindex, password, numberOfAlphabets);

            Console.WriteLine(crypt.ToString());

            #endregion

            #region encrypt and decrypt message

            if (prams.ContainsKey("p") && prams.ContainsKey("c"))
            {
                Console.WriteLine("Requires either a p->plain-text to encrypt or c->crypt-text to decrypt.");
                return;
            }

            var result = prams.ContainsKey("p")
                ? crypt.ToCrypt(prams["p"].Value)
                : crypt.ToPlain(prams["c"].Value);

            #endregion

            #region write result

            var grouplength = prams.ContainsKey("outgroup") ? Convert.ToInt32(prams["outgroup"].Value) : 5;
            // Default to outgroups to 5
            Console.WriteLine($"{(grouplength > 0 ? Crypt.GroupsOf(result, grouplength) : result)}");

            #endregion
        }

        /// <summary>
        ///     Help usage text
        /// </summary>
        private static void WriteHelp()
        {
            string[] helpText =
            {
                "Usage: ", "Required:",
                "          -a=\\d+ : Which Aphabet,  0 -  ABC..., 1- abcd... 2 - ABC0123, 3 - ABCDabcd0123, 4 - base64, 5 - 0123456789, 6 - Hex",
                "          -password|p=passwordorphrase", "          -p=MESSAGE  : Plain Text to encrypt",
                "          -c=CRYPTMSG : Encrypted message to decrypt", string.Empty, "Optional:",
                "          -outgroup|g=\\d+ (default to 5 letters)",
                "          -n=\\d+     : Number of alphabets to use",
                "          -v          : Use Vigenere square default offset 1, n is invalid when v exists"
            };

            Console.WriteLine(string.Join("\n", helpText));
        }
    }
}