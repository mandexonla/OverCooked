using System.Collections.Generic;
using UnityEngine;
public class PlateKitchenObject : KitchenObject
{

    [SerializeField] private List<KitchenObjectScriptObject> validKitchenObjectSOList;

    private List<KitchenObjectScriptObject> kitchenObjectSOList;

    private void Awake()
    {
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
            kitchenObjectSOList.Add(kitchenObjectSO);
            return true;
        }
    }
}
