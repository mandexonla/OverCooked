using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingleUI : MonoBehaviour
{

    [SerializeField] private Image image;
    public void SetKitchenObjectSO(KitchenObjectScriptObject kitchenObjectScriptObject)
    {
        image.sprite = kitchenObjectScriptObject._sprite;
    }
}
