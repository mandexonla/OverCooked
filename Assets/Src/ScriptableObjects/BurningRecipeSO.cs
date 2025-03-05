using UnityEngine;


[CreateAssetMenu()]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectScriptObject input;
    public KitchenObjectScriptObject output;
    public float _burningTimeMax;
}
