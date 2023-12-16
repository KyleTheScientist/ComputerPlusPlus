using BepInEx.Configuration;
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
    public class ThemeScreen : IScreen
    {
        public string Title => "Theme";

        public string Description =>
            "Press [Option 1-3] to pick a color\n" +
            "Press [0-9] to set the color value.\n" +
            "Press [Enter] to toggle between screen and font color.";


        public string Template =
            "    {0}{1}: {2}\n";

        public int selectedColorIndex = 0, presetIndex = -1;
        bool choosingFontColor;
        //Two-dimensional int array of color presets

        public string GetContent()
        {
            string content = choosingFontColor ? "Font:\n" : "Screen:\n";

            content += string.Format(
                Template,
                selectedColorIndex == 0 ? ">" : "",
                "Red",
                choosingFontColor ? redFG.Value : redBG.Value
            );
            content += string.Format(
                Template,
                selectedColorIndex == 1 ? ">" : "",
                "Green",
                choosingFontColor ? greenFG.Value : greenBG.Value
            );
            content += string.Format(
                Template,
                selectedColorIndex == 2 ? ">" : "",
                "Blue",
                choosingFontColor ? blueFG.Value : blueBG.Value
            );
            return content;
        }

        void UpdateTheme()
        {
            ComputerManager.Instance.backgroundMaterial.color = new Color(
                redBG.Value / 10f,
                greenBG.Value / 10f,
                blueBG.Value / 10f
            );
            GorillaComputer.instance.computerScreenRenderer.material =
                ComputerManager.Instance.backgroundMaterial;

            ComputerManager.Instance.screenText.color = new Color(
                redFG.Value / 10f,
                greenFG.Value / 10f,
                blueFG.Value / 10f
            );
            ComputerManager.Instance.functionsText.color = new Color(
                redFG.Value / 10f,
                greenFG.Value / 10f,
                blueFG.Value / 10f
            );
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            var computer = GorillaComputer.instance;
            if (button.IsNumericKey())
            {
                switch (selectedColorIndex)
                {
                    case 0:
                        if (choosingFontColor)
                            redFG.Value = int.Parse(button.characterString);
                        else
                            redBG.Value = int.Parse(button.characterString);
                        break;
                    case 1:
                        if (choosingFontColor)
                            greenFG.Value = int.Parse(button.characterString);
                        else
                            greenBG.Value = int.Parse(button.characterString);
                        break;
                    case 2:
                        if (choosingFontColor)
                            blueFG.Value = int.Parse(button.characterString);
                        else
                            blueBG.Value = int.Parse(button.characterString);
                        break;
                }
                UpdateTheme();
                return;
            }
            switch (button.characterString.ToLower())
            {
                case "option1":
                    selectedColorIndex = 0;
                    break;
                case "option2":
                    selectedColorIndex = 1;
                    break;
                case "option3":
                    selectedColorIndex = 2;
                    break;
                case "enter":
                    choosingFontColor = !choosingFontColor;
                    break;
            }
        }

        ConfigEntry<int> redBG, greenBG, blueBG;
        ConfigEntry<int> redFG, greenFG, blueFG;

        public void Start()
        {

            redBG = Plugin.Instance.Config.Bind("Background", "Red", 1);
            greenBG = Plugin.Instance.Config.Bind("Background", "Green", 1);
            blueBG = Plugin.Instance.Config.Bind("Background", "Blue", 2);
            redFG = Plugin.Instance.Config.Bind("Foreground", "Red", 9);
            greenFG = Plugin.Instance.Config.Bind("Foreground", "Green", 9);
            blueFG = Plugin.Instance.Config.Bind("Foreground", "Blue", 9);
            UpdateTheme();
        }
    }
}
