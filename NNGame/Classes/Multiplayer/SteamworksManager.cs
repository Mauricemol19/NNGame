using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Steamworks;
using System.Diagnostics;

namespace NNGame.Classes
{
    public class SteamworksManager
    {
        public bool IsSteamRunning { get; set; } = false;

        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        public string STEAM_NOT_RUNNING_ERROR_MESSAGE = "Please start your steam client to receive data!";

        // Collectible data
        public string SteamUserName { get; set; } = "";
        public string CurrentLanguage { get; set; } = "";
        public string AvailableLanguages { get; set; } = "";
        public string InstallDir { get; set; } = "";
        public Texture2D UserAvatar { get; set; }

        public bool SteamOverlayActive { get; set; }
        public string UserStats { get; set; } = "";
        public string PersonaState { get; set; } = "";
        public string LeaderboardData { get; set; } = "";
        public string NumberOfCurrentPlayers { get; set; } = "";

        public uint PlayTimeInSeconds() => SteamUtils.GetSecondsSinceAppActive();

        public void Initialize(Main main)
        {
            try
            {
                if (SteamAPI.RestartAppIfNecessary((AppId_t)480))
                {
                    Debug.WriteLine("Game wasn't started by Steam-client. Restarting.");
                    main.Exit();
                }
            }
            catch (DllNotFoundException e)
            {                
                Debug.WriteLine("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib." +
                                      " It's likely not in the correct location.\n" +
                                      e);
                main.Exit();
            }

            SteamUtils.SetOverlayNotificationPosition(ENotificationPosition.k_EPositionTopRight);
            //Uncomment the next line to adjust the OverlayNotificationPosition.
            SteamUtils.SetOverlayNotificationInset(400, 0);

            //Set collectable data
            CurrentLanguage = $"CurrentGameLanguage: {SteamApps.GetCurrentGameLanguage()}";
            AvailableLanguages = $"Languages: {SteamApps.GetAvailableGameLanguages()}";
            UserStats = $"Reqesting Current Stats - {SteamUserStats.RequestCurrentStats()}";
            //mNumberOfCurrentPlayers.Set(SteamUserStats.GetNumberOfCurrentPlayers());
            var hSteamApiCall = SteamUserStats.FindLeaderboard("Quickest Win");
            //mCallResultFindLeaderboard.Set(hSteamApiCall);

            string folder;
            var length = SteamApps.GetAppInstallDir(SteamUtils.GetAppID(), out folder, 260);
            InstallDir = $"AppInstallDir: {length} {folder}";

            // Get your Steam Avatar (Image) as a Texture2D.
            UserAvatar = GetSteamUserAvatar(main.GraphicsDevice);

            // Get your trimmed Steam User Name.
            var untrimmedUserName = SteamFriends.GetPersonaName();
            // Remove unsupported chars like emojis or other stuff our font cannot handle.
            //untrimmedUserName = ReplaceUnsupportedChars(Font, untrimmedUserName);
            SteamUserName = untrimmedUserName.Trim();

            main.Exiting += Game_Exiting;
        }

        private void Game_Exiting(object sender, EventArgs e)
        {
            SteamAPI.Shutdown();
            UserAvatar?.Dispose();
        }        

        /// <summary>
        ///     Get your steam avatar.
        ///     Important:
        ///     The returned Texture2D object is NOT loaded using a ContentManager.
        ///     So it's your responsibility to dispose it at the end by calling <see cref="Texture2D.Dispose()" />.
        /// </summary>
        /// <param name="device">The GraphicsDevice</param>
        /// <returns>Your Steam Avatar Image as a Texture2D object</returns>
        private Texture2D GetSteamUserAvatar(GraphicsDevice device)
        {
            // Get the icon type as a integer.
            var icon = SteamFriends.GetMediumFriendAvatar(SteamUser.GetSteamID());

            // Check if we got an icon type.
            if (icon != 0)
            {
                uint width;
                uint height;
                var ret = SteamUtils.GetImageSize(icon, out width, out height);

                if (ret && width > 0 && height > 0)
                {
                    var rgba = new byte[width * height * 4];
                    ret = SteamUtils.GetImageRGBA(icon, rgba, rgba.Length);

                    if (ret)
                    {
                        var texture = new Texture2D(device, (int)width, (int)height, false, SurfaceFormat.Color);
                        texture.SetData(rgba, 0, rgba.Length);

                        return texture;
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     Replaces characters not supported by your spritefont.
        /// </summary>
        /// <param name="font">The font</param>
        /// <param name="input">The input string.</param>
        /// <param name="replaceString">The string to replace illegal characters with.</param>
        /// <returns></returns>
        public static string ReplaceUnsupportedChars(SpriteFont font, string input, string replaceString = "")
        {
            string result = "";
            if (input == null)
            {
                return null;
            }

            foreach (char c in input)
            {
                if (font.Characters.Contains(c) || c == '\r' || c == '\n')
                {
                    result += c;
                }
                else
                {
                    result += replaceString;
                }
            }
            return result;
        }

        private static Callback<GameOverlayActivated_t> mGameOverlayActivated;
        private static CallResult<NumberOfCurrentPlayers_t> mNumberOfCurrentPlayers;
        private static CallResult<LeaderboardFindResult_t> mCallResultFindLeaderboard;
        private static Callback<PersonaStateChange_t> mPersonaStateChange;
        private static Callback<UserStatsReceived_t> mUserStatsReceived;
        private static Callback<SteamShutdown_t> mSteamShutdown_t;

        /// <summary>
        ///     Initialize some Steam Callbacks.
        /// </summary>
        public void InitializeCallbacks()
        {
            mGameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
            //mNumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
            //mCallResultFindLeaderboard = CallResult<LeaderboardFindResult_t>.Create(OnFindLeaderboard);
            mPersonaStateChange = Callback<PersonaStateChange_t>.Create(OnPersonaStateChange);
            mUserStatsReceived =
                Callback<UserStatsReceived_t>.Create(
                    pCallback =>
                    {
                        UserStats =
                            $"[{UserStatsReceived_t.k_iCallback} - UserStatsReceived] - {pCallback.m_eResult} -- {pCallback.m_nGameID} -- {pCallback.m_steamIDUser}";
                    });
            mSteamShutdown_t = Callback<SteamShutdown_t>.Create(OnSteamShutdown);
        }

        private void OnSteamShutdown(SteamShutdown_t pCallBack)
        {

        }

        private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
        {
            if (pCallback.m_bActive == 0)
            {
                // GameOverlay is not active.
                SteamOverlayActive = false;
            }
            else
            {
                // GameOverlay is active.
                SteamOverlayActive = true;
            }
        }

        private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIoFailure)
        {
            NumberOfCurrentPlayers =
                $"[{NumberOfCurrentPlayers_t.k_iCallback} - NumberOfCurrentPlayers] - {pCallback.m_bSuccess} -- {pCallback.m_cPlayers}";
        }

        private void OnFindLeaderboard(LeaderboardFindResult_t pCallback, bool bIoFailure)
        {
            LeaderboardData =
                $"[{LeaderboardFindResult_t.k_iCallback} - LeaderboardFindResult] - {pCallback.m_bLeaderboardFound} -- {pCallback.m_hSteamLeaderboard}";
        }

        private void OnPersonaStateChange(PersonaStateChange_t pCallback)
        {
            PersonaState =
                $"[{PersonaStateChange_t.k_iCallback} - PersonaStateChange] - {pCallback.m_ulSteamID} -- {pCallback.m_nChangeFlags}";
        }      
     
        /// <summary>
        ///     Smooth up/down movement.
        /// </summary>
        public Vector2 MoveUpAndDown(GameTime gameTime, float speed)
        {
            var time = gameTime.TotalGameTime.TotalSeconds * speed;
            return new Vector2(0, (float)Math.Sin(time));
        }
    }
}
