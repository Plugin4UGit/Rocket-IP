using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rocket_IP
{
    class RocketIP : RocketPlugin<RocketIPConfiguration>
    {
        public static RocketIP Instance;
        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList(){
                    { "command_usage", "Usage of command: /gip {player}" },
                    { "result", "IP of {0}: {1}" },
                    { "connected", "{0} connected with IP: {1}" }
                };
            }
        }

        protected override void Load()
        {
            Instance = this;

            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
            
            Rocket.Core.Logging.Logger.Log("Get IP has been loaded!", ConsoleColor.DarkGreen);
        }

        protected override void Unload()
        {
            Instance = null;

            U.Events.OnPlayerConnected -= Events_OnPlayerConnected;

            Rocket.Core.Logging.Logger.Log("Get IP has been unloaded!", ConsoleColor.DarkGreen);
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            if (Configuration.Instance.GetIPOnConnect)
            {
                Rocket.Core.Logging.Logger.LogWarning(player.DisplayName +" connected with IP " + player.CSteamID.GetIP());
                if (Configuration.Instance.DisplayIPToEveryone)
                {
                    UnturnedChat.Say(Translate("connected", player.DisplayName, player.CSteamID.GetIP()));
                }
            }
        }
    }

    public static class Extensions
    {
        public static string GetIP(this CSteamID cSteamID)
        {
            // Grab an active players ip address from CSteamID.
            P2PSessionState_t sessionState;
            SteamGameServerNetworking.GetP2PSessionState(cSteamID, out sessionState);
            return Parser.getIPFromUInt32(sessionState.m_nRemoteIP);
        }
    }
}
