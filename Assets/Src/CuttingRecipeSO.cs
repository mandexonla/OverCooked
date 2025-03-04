using UnityEngine;


[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectScriptObject input;
    public KitchenObjectScriptObject output;
    public int _cuttingProgressMax;
}
