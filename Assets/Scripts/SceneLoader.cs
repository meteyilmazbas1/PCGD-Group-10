using UnityEngine;
using UnityEngine.SceneManagement;

namespace UrbanNinja
{
    public class SceneLoader : MonoBehaviour
    {
        const int MENU_SCENE_INDEX = 0;
        const int NAME_SELECT_SCENE_INDEX = 1;
        const int GAME_SCENE_INDEX = 2;
        const int HIGHSCORE_SCENE_INDEX = 3;
        
        public static void LoadGameScene()
        {
            SceneManager.LoadScene(GAME_SCENE_INDEX);
        }
        public static void LoadMainMenuScene()
        {
            SceneManager.LoadScene(MENU_SCENE_INDEX);
        }
        public static void LoadNameSelectScene()
        {
            SceneManager.LoadScene(NAME_SELECT_SCENE_INDEX);
        }
        public static void LoadHighScoreScene()
        {
            SceneManager.LoadScene(HIGHSCORE_SCENE_INDEX);
        }
    }
}
