using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{

    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectScriptObject _kitchenObjectScriptObject;

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //player is not carrying anything
            KitchenObject.SpawnKitchenObject(_kitchenObjectScriptObject, player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
