using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using GameDataLibrary;

namespace BitSits_Framework
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields


        MenuEntry resolutionMenuEntry;
        MenuEntry isFullScreenMenuEntry;
        MenuEntry soundMenuEntry;
        MenuEntry musicMenuEntry;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base()
        {
            titleString = "Options";
            titlePosition = new Vector2(500, 500);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            resolutionMenuEntry.Text = "Resolution: " + Settings.ResolutionStrings[BitSitsGames.Settings.CurrentResolution];
            isFullScreenMenuEntry.Text = "Full Screen: " + (BitSitsGames.Settings.IsFullScreen ? "on" : "off");
            soundMenuEntry.Text = "Sound: " + (BitSitsGames.Settings.SoundEnabled ? "on" : "off");
            musicMenuEntry.Text = "Music: " + (BitSitsGames.Settings.MusicEnabled ? "on" : "off");
        }


        public override void LoadContent()
        {
            // Create our menu entries.
            resolutionMenuEntry = new MenuEntry(this, string.Empty, new Vector2(50, 230));
            isFullScreenMenuEntry = new MenuEntry(this, string.Empty, new Vector2(50, 380));
            soundMenuEntry = new MenuEntry(this, string.Empty, new Vector2(50, 430));
            musicMenuEntry = new MenuEntry(this, string.Empty, new Vector2(50, 480));

            SetMenuEntryText();

            MenuEntry backMenuEntry = new MenuEntry(this, "Back", new Vector2(50, 530));

            // Hook up menu event handlers.
            resolutionMenuEntry.Selected += ResolutionMenuEntrySelected;
            isFullScreenMenuEntry.Selected += IsFullScreenMenuEntrySelected;
            soundMenuEntry.Selected += SoundEntrySelected;
            musicMenuEntry.Selected += MusicMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
#if WINDOWS
            MenuEntries.Add(resolutionMenuEntry);
            MenuEntries.Add(isFullScreenMenuEntry);
#endif
            MenuEntries.Add(soundMenuEntry);
            MenuEntries.Add(musicMenuEntry);

            MenuEntries.Add(backMenuEntry);
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Resolution menu entry is selected.
        /// </summary>
        void ResolutionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BitSitsGames.Settings.CurrentResolution = (BitSitsGames.Settings.CurrentResolution + 1) 
                % Settings.Resolutions.Length;

            ScreenManager.GraphicsDeviceManager.PreferredBackBufferWidth =
                Settings.Resolutions[BitSitsGames.Settings.CurrentResolution].X;
            ScreenManager.GraphicsDeviceManager.PreferredBackBufferHeight =
                Settings.Resolutions[BitSitsGames.Settings.CurrentResolution].Y;
            ScreenManager.GraphicsDeviceManager.ApplyChanges();

            Camera2D.ResolutionScale = (float)Settings.Resolutions[BitSitsGames.Settings.CurrentResolution].X 
                / Camera2D.BaseScreenSize.X;

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void IsFullScreenMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BitSitsGames.Settings.IsFullScreen = !BitSitsGames.Settings.IsFullScreen;

            ScreenManager.GraphicsDeviceManager.IsFullScreen = BitSitsGames.Settings.IsFullScreen;
            ScreenManager.GraphicsDeviceManager.ApplyChanges();

            SetMenuEntryText();
        }


        void SoundEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BitSitsGames.Settings.SoundEnabled = !BitSitsGames.Settings.SoundEnabled;

            if (BitSitsGames.Settings.SoundEnabled) SoundEffect.MasterVolume = 1;
            else SoundEffect.MasterVolume = 0;

            SetMenuEntryText();
        }


        void MusicMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            BitSitsGames.Settings.MusicEnabled = !BitSitsGames.Settings.MusicEnabled;

            if (BitSitsGames.Settings.MusicEnabled) ScreenManager.GameContent.PlayMusic();
            else MediaPlayer.Pause();

            SetMenuEntryText();
        }


        #endregion
    }
}
