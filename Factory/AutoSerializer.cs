using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Common_Core.Extentions{
    public static class AutoSerializer {
        public enum SerializationMethod{
            Auto,
            RawBinary,
            CompressedBinary,
            RawJson,
            CompressedJson
        }
        public static SerializationMethod defaultSerializationMethod = SerializationMethod.CompressedJson;

        public static T AutoDeserialize<T>(this string raw, SerializationMethod method = SerializationMethod.Auto, bool catchFail = true) {
            if(method == SerializationMethod.Auto) method = defaultSerializationMethod;

            try {
                switch(method) {
                    case SerializationMethod.RawBinary:
                        return RawBase64Deserialize<T>(raw);
                    case SerializationMethod.CompressedBinary:
                        return Base64Deserialize<T>(raw);
                    case SerializationMethod.RawJson:
                        return JsonDeserialize<T>(raw);
                    case SerializationMethod.CompressedJson:
                        return JsonBase64Deserialize<T>(raw);
                    default:
                        return default(T);
                }
            }catch {
                //log exception
                return default(T);
            }
        }
        public static string AutoSerialize<T>(this T raw, SerializationMethod method = SerializationMethod.Auto) {
            if(method == SerializationMethod.Auto) method = defaultSerializationMethod;

            switch(method) {
                case SerializationMethod.RawBinary:
                    return RawBase64Serialize<T>(raw);
                case SerializationMethod.CompressedBinary:
                    return Base64Serialize<T>(raw);
                case SerializationMethod.RawJson:
                    return JsonSerialize<T>(raw);
                case SerializationMethod.CompressedJson:
                    return JsonBase64Serialize<T>(raw);
                default:
                    return "";
            }
        }

        #region Raw Binary
        public static T RawBase64Deserialize<T>(this string raw) {
            byte[] bytes = Base64UrlTextEncoder.Decode(raw);
            MemoryStream binary = new MemoryStream(bytes);
            try {
                return (T)(new BinaryFormatter().Deserialize(binary));
            } catch(InvalidCastException) {
                //log exception
                return default(T);
            }
        }
        public static string RawBase64Serialize<T>(this T raw) {
            using(MemoryStream binary = new MemoryStream()) {
                new BinaryFormatter().Serialize(binary, raw);
                return Base64UrlTextEncoder.Encode(binary.ToArray());
            }
        }
        #endregion
        #region Compressed Binary
        public static T Base64Deserialize<T>(this string raw) {
            byte[] bytes = Base64UrlTextEncoder.Decode(raw).Decompress();
            MemoryStream binary = new MemoryStream(bytes);
            try {
                return (T)(new BinaryFormatter().Deserialize(binary));
            } catch(InvalidCastException) {
                //log exception
                return default(T);
            }
        }
        public static string Base64Serialize<T>(this T raw) {
            using(MemoryStream binary = new MemoryStream()) {
                new BinaryFormatter().Serialize(binary, raw);
                return Base64UrlTextEncoder.Encode(binary.ToArray().Compress());
            }
        }
        #endregion
        #region Raw Json
        private static JsonSerializer ser = new JsonSerializer() { 
            TypeNameHandling = TypeNameHandling.Objects 
        };
        public static T JsonDeserialize<T>(this string raw) {
            StringReader re = new StringReader(raw);
            return (T)ser.Deserialize(re, typeof(T));
        }
        public static string JsonSerialize<T>(this T raw) {
            StringWriter wr = new StringWriter();
            ser.Serialize(wr, raw, typeof(T));
            return wr.ToString();
        }
        #endregion
        #region Compressed Json
        public static T JsonBase64Deserialize<T>(this string raw) {
            return raw.Decompress().JsonDeserialize<T>();
        }
        public static string JsonBase64Serialize<T>(this T raw) {
            return raw.JsonSerialize<T>().Compress();
        }
        #endregion

        #region compression
        public static string Compress(this string raw) {
            return Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(raw).Compress());
        }
        public static string Decompress(this string b64gzip) {
            return Encoding.UTF8.GetString(Base64UrlTextEncoder.Decode(b64gzip).Decompress());
        }
        public static byte[] Compress(this byte[] raw) {
            using(MemoryStream memory = new MemoryStream()) {
                using(GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true)) {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
        public static byte[] Decompress(this byte[] gzip) {
            using(GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress)) {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using(MemoryStream memory = new MemoryStream()) {
                    int count = 0;
                    do {
                        count = stream.Read(buffer, 0, size);
                        if(count > 0) {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while(count > 0);
                    return memory.ToArray();
                }
            }
        }
        #endregion
    }
}