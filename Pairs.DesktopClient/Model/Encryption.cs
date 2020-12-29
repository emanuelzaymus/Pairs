using System;
using System.Linq;

namespace Pairs.DesktopClient.Model
{
    class Encryption
    {
        private const int SmallestChar = 174;
        private const int GreatestChar = 733;
        private const int RandomRange = GreatestChar - SmallestChar;

        internal static string Encrypt(string text, int minLength = 50)
        {
            if (text == null)
                text = " ";

            Random random = new Random(text.Select(c => (int)c).Sum());
            int repeatEveryCharTimes = text.Length < minLength ? minLength / text.Length : 1;

            string encrypted = "";
            foreach (var ch in text)
            {
                for (int i = 0; i < repeatEveryCharTimes; i++)
                {
                    encrypted += (char)(SmallestChar + ((ch + random.Next()) % RandomRange));
                }
            }

            for (int i = 0; i < minLength - encrypted.Length; i++)
            {
                encrypted += (char)random.Next(SmallestChar, GreatestChar);
            }

            return encrypted;
        }
    }
}
