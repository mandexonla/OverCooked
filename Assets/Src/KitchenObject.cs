using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectScriptObject _kitchenObjectScriptObject;

    private ClearCounter clearCounter;

    public KitchenObjectScriptObject GetKitchenObjectScriptObject()
    {
        return _kitchenObjectScriptObject;
    }

    public void SetCleanCouter(ClearCounter clearCouter)
    {
        this.clearCounter = clearCouter;
    }
    public ClearCounter GetCleanCounter()
    {
        return clearCounter;
    }
}
