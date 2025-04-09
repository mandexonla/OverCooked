using System;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] BurningRecipeSOArray;

    private NetworkVariable<State> _state = new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> _fryingTimer = new NetworkVariable<float>(0f);
    private FryingRecipeSO fryingRecipeSO;
    private NetworkVariable<float> _burningTimer = new NetworkVariable<float>(0f);
    private BurningRecipeSO burningRecipeSO;

    public override void OnNetworkSpawn()
    {
        _fryingTimer.OnValueChanged += FryingTimer_OnValueChange;
        _burningTimer.OnValueChanged += BurningTimer_OnValueChange;
        _state.OnValueChanged += State_OnValueChange;
    }

    private void FryingTimer_OnValueChange(float previousValue, float newValue)
    {
        float _fryingTimeMax = fryingRecipeSO != null ? fryingRecipeSO._fryingTimeMax : 1f;

        OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            _progressNormalized = _fryingTimer.Value / _fryingTimeMax
        });
    }

    private void BurningTimer_OnValueChange(float previousValue, float newValue)
    {
        float _burningTimeMax = burningRecipeSO != null ? burningRecipeSO._burningTimeMax : 1f;

        OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            _progressNormalized = _burningTimer.Value / _burningTimeMax
        });
    }

    private void State_OnValueChange(State previousState, State newState)
    {

        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = _state.Value });

        if (_state.Value == State.Burned || _state.Value == State.Idle)
        {
            OnProgressChanged.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                _progressNormalized = 1f
            });
        }
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }

        if (HasKitchenObject())
        {
            switch (_state.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer.Value += Time.deltaTime;

                    if (_fryingTimer.Value > fryingRecipeSO._fryingTimeMax)
                    {
                        // Frying is done
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        _state.Value = State.Fried;
                        _burningTimer.Value = 0f;
                        SetBurningRecipeSOClientRpc(
                            KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectScriptObject()));
                    }
                    break;
                case State.Fried:
                    _burningTimer.Value += Time.deltaTime;

                    if (_burningTimer.Value > burningRecipeSO._burningTimeMax)
                    {
                        // Frying is done 
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        _state.Value = State.Burned;

                    }
                    break;
                case State.Burned:

                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // there is no kitchen object on the counter
            if (player.HasKitchenObject())
            {
                // player is carrying sth
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectScriptObject()))
                {
                    // Player carring sth that can be fried
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);

                    InteractLogicPlaceObjectOnCounterServerRpc(
                        KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectScriptObject())
                        );
                }
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
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectScriptObject()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());

                        SetStateIdleServerRpc();
                    }
                }
            }
            else
            {
                // player is not carrying anything 
                GetKitchenObject().SetKitchenObjectParent(player);

                SetStateIdleServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        _state.Value = State.Idle;
    }


    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicPlaceObjectOnCounterServerRpc(int _kitchenObjectSOIndex)
    {
        _fryingTimer.Value = 0f;

        _state.Value = State.Frying;

        SetFryingRecipeSOClientRpc(_kitchenObjectSOIndex);
    }

    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int _kitchenObjectSOIndex)
    {
        KitchenObjectScriptObject kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(_kitchenObjectSOIndex);
        fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObjectSO);
    }

    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int _kitchenObjectSOIndex)
    {
        KitchenObjectScriptObject kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(_kitchenObjectSOIndex);
        burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObjectSO);
    }

    private bool HasRecipeWithInput(KitchenObjectScriptObject inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }


    private KitchenObjectScriptObject GetOutputForInput(KitchenObjectScriptObject inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }
        else
        {
            return null;
        }
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectScriptObject inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectScriptObject inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in BurningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        return null;
    }

    public bool IsFried()
    {
        return _state.Value == State.Fried;
    }
}
