using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using System.Text;

namespace VG.Common
{
    public static class StringUtil
    {
        /// <summary>
        /// Convert store param to string
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ConvertParamToString(object item)
        {
            if (item == null)
                return "NULL";
            else
                return "N'" + item.ToString() + "'";
        }

        /// <summary>
        /// Build message string from a exception
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <returns></returns>
        public static string ExceptionToMessage(Exception ex)
        {
            return "UserMessage: \r\n" + ex.Message
                + "\r\n\r\n StackTrace: \r\n\t" + ex.StackTrace
                + "\r\n\r\n Source: \r\n\t" + ex.Source
                + InnerExceptionToMessage(ex);
        }


        public static string InnerExceptionToMessage(Exception ex)
        {
            return ex.InnerException != null ? "\r\n\r\n InnerException: \r\n\t " + ex.InnerException.Message : ""
                    + ((ex.InnerException != null && ex.InnerException.InnerException != null)
                                    ? "\r\n\r\n InnerException: \r\n\t " + ex.InnerException.InnerException.Message : "");
        }

        /// <summary>
        /// Generate cache key by list param that used to get data from store proc
        /// </summary>
        /// <param name="paramObj"></param>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public static string GenerateCachingKey(object[] paramObj, string cacheKey)
        {
            string key = cacheKey;
            foreach (var item in paramObj)
            {
                key += "_" + item.ToString();
            }

            return key;
        }
        public static string GenerateCachingKey(string clsType, string cacheKey, object[] paramObj)
        {
            string key = clsType + "_" + cacheKey;
            foreach (var item in paramObj)
            {
                key += "_" + item.ToString();
            }

            return key;
        }

        /// <summary>
        /// Get resource value by key
        /// </summary>
        /// <param name="T">Resource type</param>
        /// <param name="key">Key name</param>
        /// <returns></returns>
        public static string GetResourceString(Type T, string key)
        {
            if (string.IsNullOrEmpty(key))
                return key;

            ResourceManager rm = new ResourceManager(T);
            string str = rm.GetString(key);

            if (string.IsNullOrEmpty(str))
                return key;

            return str;
        }

        #region Encryption

        /// <summary>
        /// Generate Nonce string
        /// </summary>
        /// <returns></returns>
        public static string GenerateNonce()
        {
            Random random = new Random();
            // Just a simple implementation of a random number between 123400 and 9999999
            return random.Next(12340000, 99099999).ToString();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static object LogEncryptRSA = new object();
        public static object LogDecryptRSA = new object();

        /// <summary>
        /// Encrypt string with RSA
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="Base64Publickey"></param>
        /// <returns></returns>
        public static string EncryptRSA(string inputString, string Base64Publickey)
        {
            lock (LogEncryptRSA)
            {
                string encryptString = "";
                RSACryptoServiceProvider localRsa = new RSACryptoServiceProvider();
                string decodeRSA = StringUtil.Base64Decode(Base64Publickey);

                localRsa.FromXmlString(decodeRSA);
                byte[] encMessage = null;
                encMessage = localRsa.Encrypt(Encoding.UTF8.GetBytes(inputString), false);

                encryptString = Convert.ToBase64String(encMessage);
                return encryptString;
            }
        }

        /// <summary>
        /// Decrypt RSA string
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="Base64Privatekey"></param>
        /// <returns></returns>
        public static string DecryptRSA(string inputString, string Base64Privatekey)
        {
            lock (LogDecryptRSA)
            {
                byte[] decMessage;
                try
                {
                    decMessage = Convert.FromBase64String(inputString);
                }
                catch (Exception)
                {
                    try
                    {
                        inputString = inputString.Replace(" ", "+");
                        int mod4 = inputString.Length % 4;
                        if (mod4 > 0)
                        {
                            inputString += new string('=', 4 - mod4);
                        }
                        decMessage = Convert.FromBase64String(inputString);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                RSACryptoServiceProvider localRsa = new RSACryptoServiceProvider();
                localRsa.FromXmlString(StringUtil.Base64Decode(Base64Privatekey));
                byte[] messag = localRsa.Decrypt(decMessage, false);
                return Encoding.UTF8.GetString(messag).Replace("\u0000", "");
            }
        }

        /// <summary>
        /// Encode base64
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }

        /// <summary>
        /// Decode base64
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Base64Decode(string data)
        {
            try
            {
                var encoder = new UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        #endregion Encryption

        #region Convert string

        public const string uniChars =
            "àáảãạâầấẩẫậăằắẳẵặèéẻẽẹêềếểễệđìíỉĩịòóỏõọôồốổỗộơờớởỡợùúủũụưừứửữựỳýỷỹỵÀÁẢÃẠÂẦẤẨẪẬĂẰẮẲẴẶÈÉẺẼẸÊỀẾỂỄỆĐÌÍỈĨỊÒÓỎÕỌÔỒỐỔỖỘƠỜỚỞỠỢÙÚỦŨỤƯỪỨỬỮỰỲÝỶỸỴÂĂĐÔƠƯ";

        public const string KoDauChars =
            "aaaaaaaaaaaaaaaaaeeeeeeeeeeediiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAAEEEEEEEEEEEDIIIOOOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYAADOOU";

        /// <summary>
        /// Convert VN text to text without sign
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UnicodeToKoDau(this string s)
        {
            string retVal = String.Empty;
            int pos;
            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += KoDauChars[pos];
                else
                    retVal += s[i];
            }
            return retVal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UnicodeToKoDauAndGach(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";
            string retVal = String.Empty;
            int pos;

            for (int i = 0; i < s.Length; i++)
            {
                pos = uniChars.IndexOf(s[i].ToString());
                if (pos >= 0)
                    retVal += KoDauChars[pos];
                else
                    retVal += s[i];
            }
            String temp = retVal;
            for (int i = 0; i < retVal.Length; i++)
            {
                pos = Convert.ToInt32(retVal[i]);
                if (!((pos >= 97 && pos <= 122) || (pos >= 65 && pos <= 90) || (pos >= 48 && pos <= 57) || pos == 32))
                    temp = temp.Replace(retVal[i].ToString(), "");
            }
            temp = temp.Replace(" ", "-");
            while (temp.EndsWith("-"))
                temp = temp.Substring(0, temp.Length - 1);

            while (temp.IndexOf("--") >= 0)
                temp = temp.Replace("--", "-");

            retVal = temp;

            return retVal.ToLower();
        }


        public static string ToTmsLongDatetimeString(this DateTime input)
        {
            return input.ToString("MM/dd/yyyy HH:mm:ss");
        }

        public static string ToSystemDatetimeString(this DateTime input)
        {
            return input.ToString("yyyy/MM/dd HH:mm:ss");
        }
        #endregion Convert string

        public static int[] Split2Int(this string input)
        {
            List<int> result = new List<int> { };
            try
            {
                input = input ?? "";

                var tmp = input.Split(',');
                foreach (var item in tmp)
                {
                    if (item.Trim() != "")
                    {
                        result.Add(int.Parse(item.Trim()));
                    }
                }
            }
            catch (Exception ex)
            {}

            return result.ToArray();
        }
    }
}