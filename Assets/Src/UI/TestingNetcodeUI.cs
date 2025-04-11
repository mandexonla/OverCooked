using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button startHostButton;
    [SerializeField] private Button startclientButton;

    private void Awake()
    {
        startHostButton.onClick.AddListener(() =>
        {
            Debug.Log("Starting Host");
            KitchenGameMultiplayer.Instance.StartHost();
            Hide();
        });
        startclientButton.onClick.AddListener(() =>
        {
            Debug.Log("Starting Client");
            KitchenGameMultiplayer.Instance.StartClient();
            Hide();
        });

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
