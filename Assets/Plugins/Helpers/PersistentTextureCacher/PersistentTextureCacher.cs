using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Plugins.Helpers
{
    public static class PersistentTextureCacher
    {
        private static readonly Dictionary<String, Texture2D> TextureCache = new Dictionary<string, Texture2D>();
        public static bool IsLoggerEnabled = true;
        private const String DefaultCacheFolder = "Cache";

        #region Public Methods

        public static void SaveElement(String cacheKey, Texture2D texture, String folderName = "")
        {
            var usedCacheKey = String.Format("{0}_{1}", folderName, cacheKey);

            if (TextureCache.ContainsKey(usedCacheKey))
            {
                LogEvent(String.Format("Image with this cache key [{0}] is already cached, ignoring", usedCacheKey));
                return;
            }

            var imagePath = GetImagePath(usedCacheKey, folderName);

            if (File.Exists(imagePath))
            {
                LogEvent(String.Format("Image with this cache key [{0}] is found on disk, ignoring", usedCacheKey));
                return;
            }

            try
            {
                TextureCacheData.SaveToDisk(
                    new TextureCacheData
                    {
                        Base64String = Convert.ToBase64String(texture.EncodeToPNG()),
                        TextureWidth = texture.width,
                        TextureHeight = texture.height
                    },
                    imagePath);

                TextureCache.Add(usedCacheKey, texture);
            }
            catch (Exception e)
            {
                LogEvent(String.Format("Exception during saving: {0}", e));
            }
        }

        public static Texture2D GetTexture(String cacheKey, String folderName = "")
        {
            var usedCacheKey = String.Format("{0}_{1}", folderName, cacheKey);
            var imagePath = GetImagePath(usedCacheKey, folderName);

            if (!File.Exists(imagePath))
            {
                LogEvent(String.Format("Image with cache key [{0}] is not found in cache", usedCacheKey));
                return null;
            }

            if (TextureCache.ContainsKey(usedCacheKey))
            {
                LogEvent(String.Format("Image with cache key [{0}] is found", usedCacheKey));
                return TextureCache[usedCacheKey];
            }

            var cachedImageData = TextureCacheData.RestoreFromDisk(imagePath);
            if (cachedImageData != null)
            {
                var image = new Texture2D(cachedImageData.TextureWidth, cachedImageData.TextureHeight);
                image.LoadImage(Convert.FromBase64String(cachedImageData.Base64String));

                TextureCache.Add(usedCacheKey, image);

                LogEvent(String.Format("Image with cache key [{0}] is load", usedCacheKey));

                return image;
            }

            return null;
        }

        public static void WipeCache()
        {
            try
            {
                foreach (var cacheRecord in TextureCache)
                {
                    UnityEngine.Object.Destroy(cacheRecord.Value);
                }
                TextureCache.Clear();

                var files = Directory.GetFiles(Path.Combine(Application.persistentDataPath, DefaultCacheFolder));
                foreach (var file in files)
                {
                    File.Delete(file);
                }

                LogEvent("Cache is cleared");
            }
            catch (Exception e)
            {
                LogEvent(String.Format("Exception during cache clearing: {0}", e));
            }
        }

        #endregion

        #region Private Methods

        private static String GetImagePath(String cacheKey, String directory)
        {
            var cacheFolderPath = Path.Combine(Application.persistentDataPath, DefaultCacheFolder);

            if (!Directory.Exists(cacheFolderPath))
            {
                Directory.CreateDirectory(cacheFolderPath);
            }

            var concretecacheFolderPath = directory != ""
                ? Path.Combine(cacheFolderPath, directory)
                : cacheFolderPath;

            if (!Directory.Exists(concretecacheFolderPath))
            {
                Directory.CreateDirectory(concretecacheFolderPath);
            }

            var imagePath = Path.Combine(concretecacheFolderPath, String.Format("{0}.png", cacheKey));

            return imagePath;
        }

        private static void LogEvent(String message)
        {
            if (IsLoggerEnabled)
            {
                Debug.Log(String.Format("[PersistentTextureCacher]: {0}", message));
            }
        }

        #endregion

        #region Private class

        private sealed class TextureCacheData
        {
            public String Base64String;
            public int TextureWidth;
            public int TextureHeight;

            public static void SaveToDisk(TextureCacheData data, String path)
            {
                try
                {
                    using (var stream = new StreamWriter(path))
                    {
                        stream.WriteLine(data.TextureWidth);
                        stream.WriteLine(data.TextureHeight);
                        stream.WriteLine(data.Base64String);
                        stream.Close();
                    }
                }
                catch (Exception e)
                {
                    LogEvent(String.Format("Exception during saving: {0}", e));
                }
            }

            public static TextureCacheData RestoreFromDisk(String path)
            {
                var result = new TextureCacheData();

                try
                {
                    using (var stream = new StreamReader(path))
                    {
                        result.TextureWidth = Convert.ToInt32(stream.ReadLine());
                        result.TextureHeight = Convert.ToInt32(stream.ReadLine());
                        result.Base64String = stream.ReadLine();

                        stream.Close();

                        return result;
                    }
                }
                catch (Exception e)
                {
                    LogEvent(String.Format("Exception during loading: {0}", e));

                    return null;
                }
            }
        }

        #endregion
    }
}