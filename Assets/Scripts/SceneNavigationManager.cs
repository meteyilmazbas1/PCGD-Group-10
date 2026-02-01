using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UrbanNinja;

public class SceneNavigationManager : MonoBehaviour
{
    public static SceneNavigationManager Instance;

    private InputAction escapeAction;

   // [Header("UI Panels Prefabs")]
    //[SerializeField] GameObject pauseUIPrefab;
    //[SerializeField] PauseUI pauseUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }


    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (escapeAction == null)
        {
            escapeAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
            escapeAction.performed += ctx => OnEscapePressed();
        }
        escapeAction.Enable();

        //Disable escape action on game finish
        //GlobalEvents.onGameFinished += isSuccess =>
        //{
         //   escapeAction.Disable();
       // };
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        escapeAction?.Disable();

        //GlobalEvents.onGameFinished -= isSuccess =>
        //{
        //    //escapeAction.Disable();
        //};
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Re-enable escape action on scene load
        escapeAction.Enable();
    }

    private void OnEscapePressed()
    {
        if (SceneManager.GetActiveScene().buildIndex == (int)Scenes.MainMenu)
        {
            print("Open Quit");
        }
        else if (SceneManager.GetActiveScene().buildIndex == (int)Scenes.GameScene)
        {

            if (Time.timeScale == 1)
            {
                //CursorManager.s_instance.ToggleCursor(true);
                print("Spawn Pause Menu");
                Time.timeScale = 0;
                //if (pauseUI == null)
                //{
                //pauseUI = Instantiate(pauseUIPrefab, null).GetComponent<PauseUI>();
                //}
                //pauseUI.gameObject.SetActive(true);
            }
            else
            {
                //CursorManager.s_instance.ToggleCursor(false);
                print("Close Pause Menu");
                Time.timeScale = 1;
                //pauseUI.Close();
            }
        }
    }

    public void LoadScene(Scenes scene)
    {
        SceneManager.LoadScene((int)scene);
    }
}

public enum Scenes
{
    MainMenu = 0,
    NameSelection = 1,
    GameScene = 2,
    Highscore=3,

}