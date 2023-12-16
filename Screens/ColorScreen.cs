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
    public class ColorScreen : IScreen
    {
        public string Title => "Color";

        public string Description =>
            "Press [Option 1-3] to pick a color\n" +
            "Press [0-9] to set the color value.\n" +
            "Press [Enter] to cycle through color presets.";

        public string Template =
            "    {0}{1}: {2}\n";

        public int selectedColorIndex = 0, presetIndex = -1;
        Traverse computerTraverse, colorCursorLine;


        //Two-dimensional int array of color presets
        public int[,] presets = new int[10, 3]
        {
            { 9, 0, 0 }, //Red
            { 9, 5, 0 }, //Orange
            { 9, 9, 0 }, //Yellow
            { 0, 9, 0 }, //Green
            { 0, 9, 9 }, //Cyan
            { 0, 0, 9 }, //Blue
            { 5, 0, 9 }, //Purple
            { 9, 0, 9 }, //Magenta
            { 0, 0, 0 }, //Black
            { 9, 9, 9 }, //White
        };


        public string GetContent()
        {
            string content = "";
            string[] colors = { "red", "green", "blue" };
            for (int i = 0; i < colors.Length; i++)
            {
                int number = (int)(PlayerPrefs.GetFloat(colors[i] + "Value", 0f) * 10);
                if (number == 10)
                    number = 9;
                content += string.Format(
                    Template,
                    i == colorCursorLine.GetValue<int>() ? ">" : "",
                    colors[i],
                    number
                );
            }
            return content;
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            var computer = GorillaComputer.instance;
            if (button.IsNumericKey())
            {
                computerTraverse.Method("ProcessColorState", button).GetValue();
                return;
            }
            switch (button.characterString.ToLower())
            {
                case "option1":
                    colorCursorLine.SetValue(0);
                    break;
                case "option2":
                    colorCursorLine.SetValue(1);
                    break;
                case "option3":
                    colorCursorLine.SetValue(2);
                    break;
                case "enter":
                    presetIndex++;
                    if (presetIndex >= presets.GetLength(0))
                        presetIndex = 0;
                    Logging.Debug("===Preset index:", presetIndex);
                    for (int i = 0; i < presets.GetLength(1); i++)
                    {
                        Logging.Debug("===Color:", presets[presetIndex, i]);
                        colorCursorLine.SetValue(i);
                        computerTraverse.Method("ProcessColorState",
                            ComputerManager.GetKey(presets[presetIndex, i])
                        ).GetValue();

                    }
                    computerTraverse.Method("ProcessColorState", ComputerManager.Keys["enter"]).GetValue();
                    break;
            }
        }


        public void Start() { 
            computerTraverse = Traverse.Create(GorillaComputer.instance);
            colorCursorLine = computerTraverse.Field("colorCursorLine");
        }
    }
}
