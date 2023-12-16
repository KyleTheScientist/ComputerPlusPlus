using ComputerPlusPlus.Tools;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Rendering;

namespace ComputerPlusPlus.Screens
{
    public class VersionScreen : IScreen
    {
        public string Title => "Version";

        public string Description => string.Empty;
        public string GetContent()
        {
            string result = "\n\n\n" + ComputerManager.Center("Version: " + PluginInfo.Version);
            if (counter > 10000)
            {
                var text = "<color={0}>Really? You're <color={1}>that</color> bored?</color>";
                text = string.Format(text, 
                    ComputerManager.Instance.DisabledColor, 
                    ComputerManager.Instance.EnabledColor
                ); 
                result += "\n\n\n         " + text;
            }
            return result;
        }

        int counter;
        public void OnKeyPressed(GorillaKeyboardButton button) => counter++;

        public void Start() { }
    }
}
