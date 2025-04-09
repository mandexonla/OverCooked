using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectScriptObject kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectScriptObject> validKitchenObjectSOList;

    private List<KitchenObjectScriptObject> kitchenObjectSOList;

    protected override void Awake()
    {
        base.Awake();
        kitchenObjectSOList = new List<KitchenObjectScriptObject>();
    }
    public bool TryAddIngredient(KitchenObjectScriptObject kitchenObjectSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // not a valid ingredient
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            // already has this type
            return false;
        }
        else
        {
            AddIngredientServerRpc(
                KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO)
                );

            return true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int _kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(_kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void AddIngredientClientRpc(int _kitchenObjectSOIndex)
    {
        KitchenObjectScriptObject kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(_kitchenObjectSOIndex);

        kitchenObjectSOList.Add(kitchenObjectSO);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            kitchenObjectSO = kitchenObjectSO
        });
    }

    public List<KitchenObjectScriptObject> GetKitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}
