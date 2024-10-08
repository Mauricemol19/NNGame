using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamworks;

namespace NNGame.Classes
{
    public class Server
    {
        const string SERVER_VERSION = "1.0.0.0";

        const ushort SERVER_PORT = 27015;

        const ushort SERVER_AUTHENTICATION_PORT = 8766;

        const ushort SERVER_MASTER_UPDATER_PORT = 27016;

        public bool bInitialized = false;

        private SteamworksManager _steamworksManager;
        public Server() { }

        public bool Init()
        {
            EServerMode eMode = EServerMode.eServerModeAuthenticationAndSecure;
            
            //EServerMode eMode = EServerMode.eServerModeNoAuthentication;

            bInitialized = GameServer.Init(0, SERVER_AUTHENTICATION_PORT, SERVER_PORT, eMode, SERVER_VERSION);

            return bInitialized;
        }
    }
}
