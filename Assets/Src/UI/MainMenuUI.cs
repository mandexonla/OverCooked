using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playMultiplayerButton;
    [SerializeField] private Button playSinglelayerButton;
    [SerializeField] private Button quitButton;


    private void Awake()
    {
        playMultiplayerButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.playMultiplayer = true;
            Loader.Load(Loader.Scene.LobbyScene);
        });
        playSinglelayerButton.onClick.AddListener(() =>
        {
            KitchenGameMultiplayer.playMultiplayer = false;
            Loader.Load(Loader.Scene.LobbyScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Time.timeScale = 1f;
    }
}
