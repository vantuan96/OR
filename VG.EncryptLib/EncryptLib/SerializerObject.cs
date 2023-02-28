using ProtoBuf;
using System;
using System.IO;

namespace VG.EncryptLib.Shared
{
    public class SerializerObject
    {
        public static object ObjLockSerialize = new object();
        public static object ObjLockDeserialize = new object();

        /// <summary>
        /// Serialize object using protobuf
        /// </summary>
        /// <param name="item"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static byte[] ProtoBufSerialize(Object item, string saltValue)
        {
            lock (ObjLockSerialize)
            {
                if (item != null)
                {
                    try
                    {
                        var ms = new MemoryStream();
                        Serializer.Serialize(ms, item);
                        var rt = ms.ToArray();
                        if (string.IsNullOrEmpty(saltValue))
                        {
                            return rt;
                        }
                        else
                        {
                            var objRijndaelSimple = new RijndaelSimple();
                            return objRijndaelSimple.Encrypt(rt, saltValue);
                        }
                    }
                    catch (Exception ex)
                    {                        
                        throw new Exception("Unable to serialize object due to " + ex.Message, ex);
                    }
                }
                else
                {                    
                    throw new NullReferenceException("Serialized object is null");                    
                }
            }
        }
        
        /// <summary>
        /// Deserialize object using protobuf
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArray"></param>
        /// <param name="saltValue"></param>
        /// <returns></returns>
        public static T ProtoBufDeserialize<T>(byte[] byteArray, string saltValue)
        {
            lock (ObjLockDeserialize)
            {
                if (byteArray != null && byteArray.Length > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(saltValue))
                        {
                            var objRijndaelSimple = new RijndaelSimple();
                            byteArray = objRijndaelSimple.Decrypt(byteArray, saltValue);
                        }
                        var ms = new MemoryStream(byteArray);
                        return Serializer.Deserialize<T>(ms);
                    }
                    catch (Exception ex)
                    {                        
                        throw new Exception("Unable to deserialize object due to " + ex.Message, ex);
                    }
                }
                else
                {
                    throw new NullReferenceException("Deserialized object is null or empty");
                }
            }
        }

        /// <summary>
        /// Deserialize object using protobuf
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArray"></param>
        /// <param name="saltValue"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static T ProtoBufDeserialize<T>(byte[] byteArray, string saltValue, out bool status)
        {
            lock (ObjLockDeserialize)
            {
                if (byteArray != null && byteArray.Length > 0)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(saltValue))
                        {
                            var objRijndaelSimple = new RijndaelSimple();
                            byteArray = objRijndaelSimple.Decrypt(byteArray, saltValue);
                        }
                        var ms = new MemoryStream(byteArray);
                        var obj = Serializer.Deserialize<T>(ms);
                        status = true;
                        return obj;
                    }
                    catch (Exception ex)
                    {                        
                        throw new Exception("Unable to deserialize object due to " + ex.Message, ex);
                    }
                }
                else
                {
                    throw new NullReferenceException("Deserialized object is null or empty");
                }
            }
        }
    }
}