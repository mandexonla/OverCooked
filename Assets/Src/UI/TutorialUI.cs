using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI keyGamePadInteractText;
    [SerializeField] private TextMeshProUGUI keyGamePadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyGamePadPauseText;


    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateVisual();
        //DEBUG TRIGGER GAME START AUTOMATICALLY
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
        else
        {
            Show();
        }
        //Show();
    }

    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        keyPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        keyGamePadInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact);
        keyGamePadInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlternate);
        keyGamePadPauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
