using ComputerPlusPlus.Tools;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerPlusPlus.Screens
{
    public class NameScreen : IScreen
    {
        public string Title => "Name";

        public string Description => "Press [Enter] to set your name.";

        public string template =
            "    Current: {0}\n" +
            "    New: {1}\n";

        public string GetContent()
        {
            return string.Format(template,
                GorillaComputer.instance.savedName,
                GorillaComputer.instance.currentName
            );
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            ComputerManager.ComputerTraverse.Method("ProcessNameState", button).GetValue();

        }

        public void Start() { }
    }
}
