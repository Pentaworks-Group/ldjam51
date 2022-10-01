using System;

namespace Assets.Scripts.Base
{
    public static class Core
    {
        public static GameSettings SelectedGameMode { get; set; }

        private static Lazy<Scripts.Core.Game> lazyGame = new Lazy<Scripts.Core.Game>(true);
        public static Scripts.Core.Game Game
        {
            get
            {
                return lazyGame.Value;
            }
        }
    }
}
