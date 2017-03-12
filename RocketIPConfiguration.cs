using Rocket.API;

namespace Rocket_IP
{
    public class RocketIPConfiguration : IRocketPluginConfiguration
    {
        public bool GetIPOnConnect;
        public bool DisplayIPToEveryone;

        public void LoadDefaults()
        {
            GetIPOnConnect = true;
            DisplayIPToEveryone = false;
        }
    }
}
