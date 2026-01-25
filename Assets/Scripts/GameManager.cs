
namespace UrbanNinja
{
    public static class GameManager
    {
        private static PlayerController s_player;
        public static PlayerController GetPlayerController() { return s_player; }
        public static void SetPlayerController(PlayerController player) { s_player = player; }
    }
}
