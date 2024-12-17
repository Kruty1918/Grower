namespace Grower
{
    /// <summary>
    /// Represents the various game states that the game can be in.
    /// </summary>
    public enum GameStateType
    {
        /// <summary>
        /// The game is currently being played.
        /// </summary>
        Playing,

        /// <summary>
        /// The scene is being reloaded.
        /// </summary>
        ReloadingScene,

        /// <summary>
        /// The game is in the main menu.
        /// </summary>
        MainMenu,

        /// <summary>
        /// The player is viewing the in-game shop.
        /// </summary>
        ViewingShop,

        /// <summary>
        /// The player is watching an advertisement.
        /// </summary>
        ViewingAds
    }
}