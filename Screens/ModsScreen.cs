using BepInEx;
using BepInEx.Configuration;
using ComputerPlusPlus.Tools;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PluginInfo = BepInEx.PluginInfo;

namespace ComputerPlusPlus.Screens
{
    public class ModsScreen : IScreen
    {
        public string Title => "Mods";

        public string Description =>
            "Press [W/S] to scroll.\n" +
            "Press [Enter] to toggle mods on/off.";

        public string Template = "    {0}<color={1}>[{2}]  {3}</color>\n";

        int selectedPluginIndex = 0;
        BepInEx.PluginInfo selectedPlugin;
        int page = 0, perPage = 7;

        public string GetContent()
        {
            string content = "";
            for(int i = 0; i < perPage; i++)
            {
                if (i + page * perPage >= pluginInfos.Count)
                    break;
                var plugin = pluginInfos[i + page * perPage];
                content += string.Format(Template,
                    plugin == selectedPlugin ? ">" : " ",
                    plugin.Instance.enabled ? ComputerManager.Instance.EnabledColor : ComputerManager.Instance.DisabledColor,
                    plugin.Instance.enabled ? "On": "Off",
                    plugin.Metadata.Name);
            }
            // if there is another page below, add a ...
            if (pluginInfos.Count > (page + 1) * perPage)
                content += "     ...";
            
            return content;
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            switch (button.characterString.ToLower())
            {
                case "w":
                    selectedPluginIndex--;
                    Logging.Debug("Selected plugin index:", selectedPluginIndex);
                    if (selectedPluginIndex < 0)
                    {
                        selectedPluginIndex = pluginInfos.Count - 1;
                        page = pluginInfos.Count / perPage;
                    }

                    if (selectedPluginIndex < page * perPage)
                        page--;
                    break;
                case "s":
                    selectedPluginIndex++;  
                    Logging.Debug("Selected plugin index:", selectedPluginIndex);
                    if (selectedPluginIndex >= pluginInfos.Count)
                    {
                        selectedPluginIndex = 0;
                        page = 0;
                    }
                    if (selectedPluginIndex >= (page + 1) * perPage)
                        page++;
                    break;
                case "enter":
                    selectedPlugin.Instance.enabled = !selectedPlugin.Instance.enabled;
                    Plugin.Instance.Config.Bind("EnabledMods", selectedPlugin.Metadata.Name, true).Value = selectedPlugin.Instance.enabled;
                    break;
            }
            selectedPlugin = pluginInfos[selectedPluginIndex];
        }

        List<BepInEx.PluginInfo> pluginInfos = new List<BepInEx.PluginInfo>();

        public void Start()
        {
            // Loop through each BaseUnityPlugin component and add it to the list
            foreach (var plugin in BepInEx.Bootstrap.Chainloader.PluginInfos.Values)
            {
                try
                {
                    Logging.Debug("Found plugin:", plugin.Metadata.Name);
                    if(plugin.Instance == Plugin.Instance)
                    {
                        Logging.Debug("Skipping Computer++");
                        continue;
                    }
                    if(plugin.Metadata.Name == "Utilla")
                    {
                        Logging.Debug("Skipping Utilla");
                        continue;
                    }

                    var entry = Plugin.Instance.Config.Bind("EnabledMods", plugin.Metadata.Name, true);
                    plugin.Instance.enabled = entry.Value;
                    pluginInfos.Add(plugin);
                }
                catch (Exception e)
                {
                    Logging.Exception(e);
                }
            }
        }
    }
}
