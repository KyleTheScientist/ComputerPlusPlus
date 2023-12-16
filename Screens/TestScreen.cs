using ComputerPlusPlus.Tools;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ComputerPlusPlus.Screens
{
    public class TestScreen : IScreen
    {
        public string Title => "Test";

        public string Description => "Press [Option 1] for something to happen.";

        bool somethingHappened = false;

        public string GetContent()
        {
            if (somethingHappened)
                return "Something happened!";
            else
                return "";
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            if (button.characterString == "option1")
                somethingHappened = true;
        }
        public void Start() { }
    }
}
