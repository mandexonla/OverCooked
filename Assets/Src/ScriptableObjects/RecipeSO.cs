using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectScriptObject> kitchenObjectSOList;
    public string _recipeName;
}
