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
    public class QueueScreen : IScreen
    {
        public string Title => "Queue";

        public string Description =>
            "Press [Option 1] for Default Queue.\n" +
            "Press [Option 2] for Minigames.\n" +
            "Press [Option 3] for Competitive.";

        public string GetContent()
        {
            string mode = GorillaComputer.instance.currentQueue;
            return "    Queued for: " + mode;
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            ComputerManager.ComputerTraverse.Method("ProcessQueueState", button).GetValue();
        }


        public void Start() { }
    }
}
