using System;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VG.Common
{
    public static class StringExtensions
    {
        public static string SplitUpperCaseToString(this string source)
        {
            return source == null
                ? null
                : string.Join(" ", source.SplitUpperCase());
        }

        public static string[] SplitUpperCase(this string source)
        {
            if (source == null)
                return new string[] { }; //Return empty array.

            if (source.Length == 0)
                return new[] { "" };

            var words = new StringCollection();
            int wordStartIndex = 0;

            char[] letters = source.ToCharArray();
            char previousChar = char.MinValue;
            // Skip the first letter. we don't care what case it is.
            for (int i = 1; i < letters.Length; i++)
            {
                if (char.IsUpper(letters[i]) && !char.IsWhiteSpace(previousChar))
                {
                    //Grab everything before the current index.
                    words.Add(new String(letters, wordStartIndex, i - wordStartIndex));
                    wordStartIndex = i;
                }
                previousChar = letters[i];
            }
            //We need to have the last word.
            words.Add(new String(letters, wordStartIndex, letters.Length - wordStartIndex));

            //Copy to a string array.
            var wordArray = new string[words.Count];
            words.CopyTo(wordArray, 0);
            return wordArray;
        }

        public static bool EqualsIgnoreCase(this string value1, string value2)
        {
            return value1.Equals(value2, StringComparison.OrdinalIgnoreCase);
        }


        public static string ToMD5(this string text)
        {
            //Check wether data was passed
            if ((text == null) || (text.Length == 0))
            {
                return String.Empty;
            }

            //Calculate MD5 hash. This requires that the string is splitted into a byte[].
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(text);
            byte[] result = md5.ComputeHash(textToHash);

            //Convert result back to string.
            return System.BitConverter.ToString(result);
        }


        public static bool IsNotNullOrEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }


        /// <summary>
        /// Convert string to MD5
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string MD5Hash(this string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public static bool ServiceIsSurgeryOrProcedure(this string serviceGroupCode,string serviceGroupViName, string serviceGroupEnName)
        {
            bool returnValue = false;
            returnValue=Constant.ListGroupCodeNameIsSurgicalProcedure.Any(x => serviceGroupCode.ToLower().Contains(x) || serviceGroupViName.ToLower().Contains(x) || serviceGroupEnName.ToLower().Contains(x));
            return returnValue;
        }
        public static bool ServiceIsSurgery(this string serviceGroupViName, string serviceGroupEnName)
        {
            bool returnValue = false;
            returnValue = Constant.ListCodeIsSurgical.Any(x => serviceGroupViName.ToLower().Contains(x) || serviceGroupEnName.ToLower().Contains(x));
            return returnValue;
        }
        public static bool ServiceIsProcedure(this string serviceGroupViName, string serviceGroupEnName)
        {
            bool returnValue = false;
            returnValue = Constant.ListCodeIsProcedure.Any(x => serviceGroupViName.ToLower().Contains(x) || serviceGroupEnName.ToLower().Contains(x));
            return returnValue;
        }
    }
}
