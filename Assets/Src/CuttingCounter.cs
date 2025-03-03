using UnityEngine;

public class CuttingCounter : BaseCounter
{

    [SerializeField] private KitchenObjectScriptObject cutKitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // there is no kitchen object on the counter
            if (player.HasKitchenObject())
            {
                // player is carrying sth
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // player not carrying anything

            }
        }
        else
        {
            // there is a KitchenObject on the counter
            if (player.HasKitchenObject())
            {
                // player is carrything sth
            }
            else
            {
                // player is not carrying anything 
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            // There is a KitchenObject on the counter
            GetKitchenObject().DestroySelf();

            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO, this);
        }
    }
}
