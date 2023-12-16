using ComputerPlusPlus.Screens;
using GorillaNetworking;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerPlusPlus.Patches
{
    [HarmonyPatch(typeof(GorillaComputer), "PressButton")]
    class KeyPressPatches
    {
        private static bool Prefix(GorillaKeyboardButton buttonPressed)
        {
            ComputerManager.Instance.OnKeyPressed(buttonPressed);
            return false;
        }
    }
}
