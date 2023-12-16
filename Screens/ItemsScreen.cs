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
    public class ItemsScreen : IScreen
    {
        public string Title => "Items";

        public string Description =>
            "Press [Option 1] to enable item particles.\n" +
            "Press [Option 2] to disable item particles.\n" +
            "Press a number to change instrument volumes for other players.";

        string template =
            "    Particles: {0}\n" +
            "    Instrument Volume: {1}\n";

        public string GetContent()
        {
            string particles = GorillaComputer.instance.disableParticles ? "FALSE" : "TRUE";
            string volume = ((int)(GorillaComputer.instance.instrumentVolume * 50)).ToString();
            return string.Format(template, particles, volume);
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            ComputerManager.ComputerTraverse.Method("ProcessVisualsState", button).GetValue();
        }


        public void Start() { }
    }
}
