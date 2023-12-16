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
    public class VoiceScreen : IScreen
    {
        public string Title => "Voice";

        public string Description =>
            "Press [Option 1] for All Chat.\n" +
            "Press [Option 2] for Push to Talk.\n" +
            "Press [Option 3] for Push to Mute.\n" +
            "Press [Enter] to toggle voice chat.";

        string template = 
            "    Mic Mode: {0}\n" +
            "    Voice Enabled: {1}\n";

        public string GetContent()
        {
            string mode = GorillaComputer.instance.pttType;
            string voice = GorillaComputer.instance.voiceChatOn;
            return string.Format(template, mode, voice);
                
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            switch (button.characterString)
            {
                case "option1":
                case "option2":
                case "option3":
                    ComputerManager.ComputerTraverse.Method("ProcessMicState", button).GetValue();
                    break;
                case "enter":
                    if(GorillaComputer.instance.voiceChatOn == "FALSE")
                        ComputerManager.ComputerTraverse.Method(
                            "ProcessVoiceState",
                            ComputerManager.Keys["option1"]
                        ).GetValue();
                    else
                        ComputerManager.ComputerTraverse.Method(
                                "ProcessVoiceState",
                                ComputerManager.Keys["option2"]
                            ).GetValue();
                    break;
            }
        }


        public void Start() { }
    }
}
