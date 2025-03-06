using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectScriptObject _kitchenObjectScriptObject;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectScriptObject GetKitchenObjectScriptObject()
    {
        return _kitchenObjectScriptObject;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;


        if (this.kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("kitchenObjectParent already has a kitchen object");
        }

        this.kitchenObjectParent.SetKitchenObject(this);

        transform.parent = this.kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    public void DestroySelf()
    {
        kitchenObjectParent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectScriptObject _kitchenObjectScriptObject,
        IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(_kitchenObjectScriptObject.prefab);

        KitchenObject _kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        _kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return _kitchenObject;
    }

}
