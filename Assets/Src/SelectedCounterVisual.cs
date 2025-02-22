using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private CleanCouter _cleanCouter;
    [SerializeField] private GameObject visualGameObject;

    private void Start()
    {
        Player.instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == _cleanCouter)
        {
            //visualGameObject.SetActive(true);
            Show();
        }
        else
        {
            //    visualGameObject.SetActive(false);
            Hide();
        }
    }
    private void Show()
    {
        visualGameObject.SetActive(true);
    }
    private void Hide()
    {
        visualGameObject.SetActive(false);
    }
}
