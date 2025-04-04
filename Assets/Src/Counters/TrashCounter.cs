using System;
using Unity.Netcode;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyObjectTrashed;

    new public static void ResetStaticData()
    {
        OnAnyObjectTrashed = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(player.GetKitchenObject());

            InteractLoginServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLoginServerRpc()
    {
        InteractLoginClientRpc();
    }

    [ClientRpc]
    private void InteractLoginClientRpc()
    {
        OnAnyObjectTrashed(this, EventArgs.Empty);
    }
}
