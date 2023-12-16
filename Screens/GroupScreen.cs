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
    public class GroupScreen : IScreen
    {
        public string Title => "Group";

        public string Description =>
            "To join a public lobby as a group:\n" +
            " - Join a private room with your group\n" +
            " - Gather players at the computer\n" +
            " - Use the numbers to select a map\n" +
            " - Press [Enter]";

        public string GetContent()
        {
            var cpu = GorillaComputer.instance;
            var map = cpu.allowedMapsToJoin[
                Mathf.Min(cpu.allowedMapsToJoin.Length - 1, cpu.groupMapJoinIndex)
            ].ToUpper();

            return "    Map: " + map;
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            ComputerManager.ComputerTraverse.Method("ProcessGroupState", button).GetValue();
        }


        public void Start() { }
    }
}
