using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Plugins.Helpers
{
    public static class PersistentTextureCacher
    {
        private static Dictionary<String, Texture2D> _textureCache = new Dictionary<string, Texture2D>();
        public static bool IsLoggerEnabled = true;

        public static void SaveElement(String cacheKey, Texture2D texture)
        {
            if (_textureCache.ContainsKey(cacheKey))
            {
                LogEvent(String.Format("Image with this cache key [{0}] is already cached, ignoring", cacheKey));
                return;
            }

            var imagePath = GetImagePath(cacheKey);

            if (File.Exists(imagePath))
            {
                LogEvent(String.Format("Image with this cache key [{0}] is found on disk, ignoring", cacheKey));
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

                _textureCache.Add(cacheKey, texture);
            }
            catch (Exception e)
            {
                LogEvent(String.Format("Exception during saving: {0}", e));
            }
        }

        public static Texture2D GetTexture(String cacheKey)
        {
            var imagePath = GetImagePath(cacheKey);

            if (!File.Exists(imagePath))
            {
                LogEvent(String.Format("Image with cache key [{0}] is not found in cache", cacheKey));
                return null;
            }

            if (_textureCache.ContainsKey(cacheKey))
            {
                LogEvent(String.Format("Image with cache key [{0}] is found", cacheKey));
                return _textureCache[cacheKey];
            }

            var cachedImageData = TextureCacheData.RestoreFromDisk(imagePath);
            if (cachedImageData != null)
            {
                var image = new Texture2D(cachedImageData.TextureWidth, cachedImageData.TextureHeight);
                image.LoadImage(Convert.FromBase64String(cachedImageData.Base64String));

                _textureCache.Add(cacheKey, image);

                LogEvent(String.Format("Image with cache key [{0}] is load", cacheKey));

                return image;
            }

            return null;
        }

        private static String GetImagePath(String cacheKey)
        {
            var imageDirectoryPath = Path.Combine(Application.persistentDataPath, "DefaultCache");

            if (!Directory.Exists(imageDirectoryPath))
            {
                Directory.CreateDirectory(imageDirectoryPath);
            }

            var imagePath = Path.Combine(imageDirectoryPath, String.Format("{0}.png", cacheKey));

            return imagePath;
        }

        private static void LogEvent(String message)
        {
            if (IsLoggerEnabled)
            {
                Debug.Log(String.Format("[PersistentTextureCacher]: {0}", message));
            }
        }

        private class TextureCacheData
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
    }
}