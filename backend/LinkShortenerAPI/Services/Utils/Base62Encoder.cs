using System.Text;

namespace LinkShortenerAPI.Services.Utils
{
    public class Base62Encoder
    {
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static readonly int Base = Alphabet.Length;

        public static string Encode(long number)
        {
            if (number == 0) return Alphabet[0].ToString();

            var result = new StringBuilder();
            while (number > 0)
            {
                result.Insert(0, Alphabet[(int)(number % Base)]);
                number /= Base;
            }

            return result.ToString();
        }

        public static long Decode(string base62)
        {
            long result = 0;

            foreach (char c in base62)
            {
                result = result * Base + Alphabet.IndexOf(c);
            }

            return result;
        }
    }

}
