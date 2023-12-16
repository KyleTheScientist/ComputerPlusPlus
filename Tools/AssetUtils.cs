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
            try
            {
                string path = Paths.PluginPath + "/ComputerPlusPlus/wallpaper.png";
                byte[] bytes = File.ReadAllBytes(path);
                Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
                loadTexture.LoadImage(bytes);
                return loadTexture;
            }
            catch (Exception e)
            {
                Logging.Info("Did not find wallpaper.png in Computer++ folder.");
                Logging.Exception(e);
            }
            return null;
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