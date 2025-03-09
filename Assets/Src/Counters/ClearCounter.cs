using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectScriptObject _kitchenObjectScriptObject;

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
                // player is not carrying anything

            }

        }
        else
        {
            // there is a KitchenObject on the counter
            if (player.HasKitchenObject())
            {
                // player is carrything sth
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectScriptObject()))
                    {
                        GetKitchenObject().DestroySelf();
                    }

                }
                else
                {
                    // player is not holding a plate but sth else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // there is a plate on the counter
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectScriptObject()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                // player is not carrying anything 
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}
