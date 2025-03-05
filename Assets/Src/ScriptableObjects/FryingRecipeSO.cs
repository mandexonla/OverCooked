using UnityEngine;


[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectScriptObject input;
    public KitchenObjectScriptObject output;
    public float _fryingTimeMax;
}
