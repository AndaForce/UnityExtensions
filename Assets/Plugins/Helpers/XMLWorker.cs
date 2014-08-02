using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Plugins.Helpers
{
    public class XMLWorker {
	
        static readonly string PasswordHash = "GsRGr3t#T34tgrebdfAR#$gsrg$%Yey6";
        static readonly string SaltKey = "InfoTECH Mobile";
        static readonly string VIKey = "Fr&4Ds(E3$%YFDsI";
	
        //Сохранение в кастомное место
        public static void SaveTo <T>(string relativePath, ref T data, bool needEncryption = false)
        {
            try
            {
                FileStream fs = new FileStream(relativePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
			
                XmlSerializer xmls = new XmlSerializer(typeof(T));
			
                StringBuilder sb = new StringBuilder();
                TextWriter serializeString = new StringWriter(sb);
                xmls.Serialize(serializeString, data );
			
                string txt = sb.ToString();
			
                if (needEncryption)
                    txt = Encrypt(txt);
			
                sw.Write(txt);
                sw.Close();
                fs.Close();
            }
            catch (System.Exception e)
            {
                Debug.Log("error occured while save .xml file to path\"" + relativePath + "\n" + e.ToString());
            }
        }
	
        //сохранение в PersistendDataPath
        public static void SaveToPersistendDataPath<T>(string relativePath, ref T data, bool needEncryption = false)
        {
            SaveTo(Path.Combine(Application.persistentDataPath, relativePath), ref data, needEncryption);
        }
	
        //загрузка из ресурсов
        public static T LoadFromResources<T>(string relativePath, ref T defaultData, bool needEncryption = false) 
        {
            if (relativePath.Contains("."))
                relativePath = relativePath.Substring(0, relativePath.IndexOf('.'));
		
            TextAsset textAss = Resources.Load<TextAsset>(relativePath);
		
            if (textAss == null)
                Debug.LogError("Resource couldn't found " + relativePath);
            else
            {
                string str = new StringReader(textAss.text).ReadToEnd();
			
                if (needEncryption)
                    str = Decrypt(str);
			
                Deserialize(str, ref defaultData);
            }
		
            return defaultData;
        }
        //загрузка из PersistendDataPath
        public static T LoadFromPersistendDataPath<T>(string relativePath, ref T defaultData, bool needEncryption = false)
        {
            return Load (Path.Combine (Application.persistentDataPath, relativePath), ref defaultData, needEncryption);
        }
	
        //загрузка файла
        public static T Load<T>(string path, ref T defaultData, bool needDecryption = false)
        {
            try
            {
                File.SetAttributes(path, FileAttributes.Normal);
			
                string str;
			
                if (Application.isEditor || !path.Contains("Resources"))
                {
                    FileStream fs = new FileStream(path , FileMode.Open);
                    str = new StreamReader(fs).ReadToEnd();
                    fs.Close();
                }
                else
                {
                    if (path.Contains("."))
                        path = path.Substring(0, path.IndexOf('.'));
				
                    TextAsset textAss = (TextAsset)Resources.Load(path);
                    str = new StringReader(textAss.text).ReadToEnd();
                }
			
                if (needDecryption)
                    str = Decrypt(str);
			
                Deserialize(str,ref defaultData);
			
                File.SetAttributes(path, FileAttributes.Normal);
			
                return defaultData;
            }
            catch (System.Exception e)
            {
                Debug.Log("Exception catch during load xml file " + e.ToString());
                return defaultData;
            }
        }
	
        //десериализатор текста
        public static T Deserialize<T>(string data, ref T type)
        {
            XmlSerializer xmls = new XmlSerializer(typeof(T));
		
            TextReader tr = new StringReader(data);
            try
            {
                type = (T)xmls.Deserialize(tr);
            }
            catch(System.Exception e) { Debug.LogError("unable to deserialize " + e.ToString()); }
		
            return type;
        }
	
        //шифровка
        private static string Encrypt(string decryptedString)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(decryptedString);
		
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
		
            byte[] cipherTextBytes;
		
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return System.Convert.ToBase64String(cipherTextBytes);
        }
	
        //расшифровка
        private static string Decrypt(string encryptedString)
        {
            byte[] cipherTextBytes = System.Convert.FromBase64String(encryptedString);
            byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };
		
            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
		
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}

