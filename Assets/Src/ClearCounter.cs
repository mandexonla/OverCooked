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
}
