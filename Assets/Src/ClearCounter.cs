using UnityEngine;

public class ClearCounter : MonoBehaviour
{
    [SerializeField] private KitchenObjectScriptObject _kitchenObjectScriptObject;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private ClearCounter secondCounter;


    private KitchenObject _kitchenObject;
    public void Interact()
    {
        if (_kitchenObject == null)
        {
            Transform kitchenObjectTransform = Instantiate(_kitchenObjectScriptObject.prefab, counterTopPoint);
            kitchenObjectTransform.localPosition = Vector3.zero;

            _kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
            _kitchenObject.SetCleanCouter(this);

        }
        else
        {
            Debug.Log(_kitchenObject.GetCleanCounter());
        }
    }

}
