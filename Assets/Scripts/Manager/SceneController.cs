using Deckfense;
using GoblinGames;
using GoblinGames.DesignPattern;
using Protocol.Network;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoSingleton<SceneController>
{
    [SerializeField] private GameEvent<string> sceneChangedEvnet;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            LoadScene("Splash");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            LoadScene("Loading");
        }
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        sceneChangedEvnet.Invoke(arg1.name);
        GameController.Instance.RequestChangeClientState(new RequestChangeClientState()
        {
            Type = MessageType.RequestChangeClientState,
            CurrentSceneName = arg1.name,
        });

        Debug.Log($"Active Scene Changed: {arg0.name} to {arg1.name}");
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
