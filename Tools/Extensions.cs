using GorillaNetworking;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ComputerPlusPlus.Tools
{
    public static class Extensions
    {
        public static string[] FunctionKeys { get; private set; } = new string[] {
            "option1",
            "option2",
            "option3",
            "up",
            "down",
            "enter",
            "delete"
        };

        public static string[] NumericKeys { get; private set; } = new string[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"
        };
        public static bool IsFunctionKey(this GorillaKeyboardButton button)
        {
            return Array.IndexOf(FunctionKeys, button.characterString) != -1;
        }

        public static bool IsNumericKey(this GorillaKeyboardButton button)
        {
            return Array.IndexOf(NumericKeys, button.characterString) != -1;
        }

        public static Color Darker(this Color color, float percentage = .2f)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            v *= 1 - percentage;
            return Color.HSVToRGB(h, s, v);
        }

        public static Color Lighter(this Color color, float percentage = .2f)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            v *= 1 + percentage;
            if (v == 0)
                v = percentage;
            return Color.HSVToRGB(h, s, v);
        }
    }
}
