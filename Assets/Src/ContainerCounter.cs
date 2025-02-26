using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{

    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectScriptObject _kitchenObjectScriptObject;

    public override void Interact(Player player)
    {
        Transform kitchenObjectTransform = Instantiate(_kitchenObjectScriptObject.prefab);
        kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }


}
