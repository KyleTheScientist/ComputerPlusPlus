using System;
using System.IO;
using UnityEngine;
using System.Reflection;
using BepInEx;
using ComputerPlusPlus.Tools;

namespace WalkSimulator.Tools
{
    public static class AssetUtils
    {
        private static string FormatPath(string path)
        {
            return path.Replace("/", ".").Replace("\\", ".");
        }

        public static AssetBundle LoadAssetBundle(string path) // Or whatever you want to call it as
        {
            path = FormatPath(path);
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        public static Texture2D LoadImageFromFile()
        {
            string path = Paths.PluginPath + "/ComputerPlusPlus";
            //create directory if it doesn't exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            try
            {
                byte[] bytes = File.ReadAllBytes(path + "/wallpaper.png");
                Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
                loadTexture.LoadImage(bytes);
                return loadTexture;
            }
            catch (Exception e)
            {
                //create a blank white image
                Logging.Info("Did not find wallpaper.png in ComputerPlusPlus folder.");
                Logging.Exception(e);
                Texture2D texture = new Texture2D(192, 108);
                Color[] colors = new Color[texture.width * texture.height];
                for (int i = 0; i < colors.Length; i++)
                    colors[i] = Color.white;
                texture.SetPixels(colors);
                texture.Apply();
                byte[] placeholder = texture.EncodeToPNG();
                File.WriteAllBytes(path + "/wallpaper.png", placeholder);
                return texture;
            }
        }

        /// <summary>
        /// Returns a list of the names of all embedded resources
        /// </summary>
        public static string[] GetResourceNames()
        {
            var baseAssembly = Assembly.GetCallingAssembly();
            string[] names = baseAssembly.GetManifestResourceNames();
            if (names == null)
            {
                Console.WriteLine("No manifest resources found.");
                return null;
            }
            return names;
        }
    }
}