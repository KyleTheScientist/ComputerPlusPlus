using ComputerPlusPlus.Tools;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputerPlusPlus.Screens
{
    public class RoomScreen : IScreen
    {
        public string Title => "Room";

        public string Description =>
            "Press [Option 1] to exit the current room.\n" +
            "Press [Enter] to join a room code.";

        public string Template =
            "    Current room code: {0}\n" +
            "\n" +
            "    Join Code: {2}\n" +
            "\n" +
            "    Players: {1}\n";

        public string GetContent()
        {
            string code = PhotonNetwork.CurrentRoom?.Name;
            string players = PhotonNetwork.CurrentRoom?.PlayerCount.ToString();
            string roomToJoin = GorillaComputer.instance.roomToJoin;
            return string.Format(Template, code, players, roomToJoin);
        }

        public void OnKeyPressed(GorillaKeyboardButton button)
        {
            if (!button.IsFunctionKey() && GorillaComputer.instance.roomToJoin.Length < 10)
            {
                GorillaComputer.instance.roomToJoin += button.characterString;
            }
            switch (button.characterString)
            {
                case "delete":
                    string code = GorillaComputer.instance.roomToJoin;
                    GorillaComputer.instance.roomToJoin = code.Substring(0, code.Length - 1);
                    break;
                case "option1":
                    PhotonNetworkController.Instance.AttemptDisconnect();
                    break;
                case "enter":
                    Traverse.Create(GorillaComputer.instance)
                        .Method("ProcessRoomState", button).GetValue();
                    break;
            }
        }

        public void Start() { }
    }
}
