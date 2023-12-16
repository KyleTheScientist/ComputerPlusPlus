using BepInEx;
using BepInEx.Configuration;
using ComputerPlusPlus.Tools;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ComputerPlusPlus.Screens
{
    public class AutoJoinScreen : IScreen
    {
        public string Title => "AutoJoin";

        public string Description =>
            "Auto-Join a lobby when you start the game.\n" +
            "Press [Option 1] to enable/disable\n" +
            "Press [Option 2] to toggle the gamemode. \n" +
            "Type to set the auto-join code.";

        public string template =
            "    Enable Auto-Join: {0}\n" +
            "    Gamemode: {1}\n" +
            "    Code: {2}\n";

        Dictionary<string, string> gamemodeMap = new Dictionary<string, string>()
        {
            { "casual", "CASUAL" },
            { "infection", "INFECTION" },
            { "hunt", "HUNT" },
            { "paintbrawl", "BATTLE" },
            { "modded casual", "MODDED_MODDED_CASUALCASUAL" },
            { "modded infection", "MODDED_MODDED_INFECTIONINFECITON" },
            { "modded hunt", "MODDED_MODDED_HUNTHUNT" },
            { "modded paintbrawl", "MODDED_MODDED_BATTLEBATTLE" }
        };

        public string GetContent()
        {
            return string.Format(
                template,
                enabled.Value,
                gamemode.Value,
                code.Value
            );
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            switch (button.characterString.ToLower())
            {
                case "option1":
                    enabled.Value = !enabled.Value;
                    break;
                case "option2":
                    // cycle through gamemodes
                    string current = gamemode.Value;
                    int index = Array.IndexOf(gamemodeMap.Keys.ToArray(), current);
                    index++;
                    if (index >= gamemodeMap.Keys.Count)
                        index = 0;
                    gamemode.Value = gamemodeMap.Keys.ToArray()[index];
                    break;
                default:
                    if (!button.IsFunctionKey() && code.Value.Length < 10)
                        code.Value += button.characterString;
                    else if (button.characterString == "delete")
                        code.Value = code.Value.Substring(0, code.Value.Length - 1);
                    break;
            }

        }

        ConfigEntry<string> gamemode, code;
        ConfigEntry<bool> enabled;

        public void Start()
        {
            var gamemodeDesc = new ConfigDescription(
                "The gamemode to auto-join.",
                new AcceptableValueList<string>(gamemodeMap.Keys.ToArray())
            );
            gamemode = Plugin.Instance.Config.Bind<string>("AutoJoin", "Gamemode", "modded casual", gamemodeDesc);
            code = Plugin.Instance.Config.Bind<string>("AutoJoin", "Code", "", "The code to auto-join.");
            enabled = Plugin.Instance.Config.Bind<bool>("AutoJoin", "Enabled", false, "Whether or not to auto-join when the game launches.");

            //validate that the code is valid, if not, truncate it
            if (code.Value.Length > 10)
                code.Value = code.Value.Substring(0, 10);

            if (enabled.Value)
                Plugin.Instance.StartCoroutine(JoinLobbyInternal());
        }

        IEnumerator JoinLobbyInternal()
        {
            while (PhotonNetworkController.Instance.CurrentState() != "ConnectedAndWaiting")
            {
                yield return new WaitForSeconds(1f);
            }
            GorillaComputer.instance.currentGameMode = gamemodeMap[gamemode.Value];
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(code.Value);
        }
    }
}
