using Newtonsoft.Json;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
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
                    { "connected", "{0} connected with IP: {1} from : {2} " }
                };
            }
        }


        public static string GetCountry(string ip)
        {
            Info ipInfo = new Info();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<Info>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return ipInfo.Country;
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
                Rocket.Core.Logging.Logger.LogWarning(player.DisplayName + " connected with IP " + player.IP + " from " + GetCountry(player.IP));
                if (Configuration.Instance.DisplayIPToEveryone)
                {
                    UnturnedChat.Say(Translate("connected", player.DisplayName, player.IP , GetCountry(player.IP)));
                }
            }
        }
    }

    public class Info
    {

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("loc")]
        public string Loc { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("postal")]
        public string Postal { get; set; }
    }
}
