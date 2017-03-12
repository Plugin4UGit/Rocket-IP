using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Rocket.API;
using Rocket.Unturned.Player;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Chat;

namespace Rocket_IP
{
    public class CommandGetIP : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Both; }
        }

        public string Name
        {
            get { return "GetIP"; }
        }

        public string Help
        {
            get { return "Gets the IP of a user."; }
        }

        public string Syntax
        {
            get { return "<User>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>() { "GIP" }; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length == 1)
            {
                UnturnedPlayer Target = UnturnedPlayer.FromName(command[0]);
                if (caller is ConsolePlayer)
                {
                    Rocket.Core.Logging.Logger.LogWarning("IP of " + Target.DisplayName + ": " + Target.CSteamID.GetIP());
                }
                else if (caller != null)
                {
                    Rocket.Core.Logging.Logger.LogWarning("IP of " + Target.DisplayName + ": " + Target.CSteamID.GetIP());
                    if (RocketIP.Instance.Configuration.Instance.DisplayIPToEveryone)
                    {
                        UnturnedChat.Say(RocketIP.Instance.Translate("result", Target.DisplayName, Target.CSteamID.GetIP()));
                    }
                    else
                    {
                        UnturnedChat.Say(caller, RocketIP.Instance.Translate("result", Target.DisplayName, Target.CSteamID.GetIP()));
                    }
                }
            }
            else
            {
                if (caller is ConsolePlayer)
                {
                    Rocket.Core.Logging.Logger.LogWarning("Usage of command: /gip {player}");
                }
                else if (caller != null)
                {
                    UnturnedChat.Say(caller, RocketIP.Instance.Translate("command_usage"));
                }
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                    "GetIP"
                };
            }
        }
    }
}
