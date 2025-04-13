using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent
{


    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPickedSomething;

    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
    }

    public static Player LocalInstance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float _rotateSpeed = 10f;
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private LayerMask couterLayerMask;
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    [SerializeField] private List<Vector3> spawnPosionList;
    //[SerializeField] private PlayerVisual playerVisual;

    private bool _isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject _kitchenObject;


    private void Start()
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInterAction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;

        //PlayerData playerData = KitchenGameMultiplayer.Instance.GetPlayerDataFromClientId(OwnerClientId);
        //playerVisual.SetPlayerColor(KitchenGameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalInstance = this;
        }

        transform.position = spawnPosionList[KitchenGameMultiplayer.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];

        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == OwnerClientId && HasKitchenObject())
        {
            KitchenObject.DestroyKitchenObject(GetKitchenObject());
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInterAction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float _interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, _interactDistance, couterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Has CleanCouter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float _moveDistance = _moveSpeed * Time.deltaTime;
        float _playerRadius = .7f;
        float _playerHeight = 2f;
        bool _canMove = !Physics.BoxCast(transform.position, Vector3.one * _playerRadius,
          moveDir, Quaternion.identity, _moveDistance, collisionLayerMask);

        if (!_canMove)
        {
            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            _canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.BoxCast(transform.position,
               Vector3.one * _playerRadius, moveDirX, Quaternion.identity, _moveDistance, collisionLayerMask);

            if (_canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                _canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.BoxCast(transform.position,
                    Vector3.one * _playerRadius, moveDirZ, Quaternion.identity, _moveDistance, collisionLayerMask);
                if (_canMove)
                {
                    // Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // Cannot move in any direction
                }
            }
        }


        if (_canMove)
        {
            transform.position += moveDir * _moveSpeed * Time.deltaTime;
        }

        _isWalking = moveDir != Vector3.zero;

        transform.forward = Vector3.Lerp(transform.forward, -moveDir * 2, Time.deltaTime * _rotateSpeed);

    }
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this._kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
            OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }
    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }
    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}
