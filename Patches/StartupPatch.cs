using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using Utilla;

namespace ComputerPlusPlus.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Start")]
    static class PostInitializedPatch
    {
        static bool initialized;

        private static void Postfix()
        {
            if (!initialized)
            {
                Plugin.Instance.Setup();
                initialized = true;
            }
        }
    }
}
