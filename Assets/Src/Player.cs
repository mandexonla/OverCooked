using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    //private static Player instance;
    //public static Player Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //    set
    //    {
    //        instance = value;
    //    }
    //}

    public static Player Instance { get; private set; }

    public static Player instanceField;
    public static Player GetInstanceField()
    {
        return instanceField;
    }

    public static void SetInstanceField(Player instanceField)
    {
        Player.instanceField = instanceField;
    }


    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private GameInput _gameInput;
    [SerializeField] private LayerMask couterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool _isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;
    private KitchenObject _kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Player instance is null. Assigning instance.");
        }
        Instance = this;
    }

    private void Start()
    {
        _gameInput.OnInterAction += GameInput_OnInterAction;
    }

    private void GameInput_OnInterAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float _interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, _interactDistance, couterLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter cleanCouter))
            {
                // Has CleanCouter
                if (cleanCouter != selectedCounter)
                {
                    SetSelectedCounter(cleanCouter);
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
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float _moveDistance = _moveSpeed * Time.deltaTime;
        float _playerRadius = .7f;
        float _playerHeight = 2f;
        bool _canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight,
            _playerRadius, moveDir, _moveDistance);

        if (!_canMove)
        {
            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            _canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight,
                _playerRadius, moveDirX, _moveDistance);
            if (_canMove)
            {
                // Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                _canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight,
                    _playerRadius, moveDirZ, _moveDistance);
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

        float _rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDir, Time.deltaTime * _rotateSpeed);

    }
    private void SetSelectedCounter(ClearCounter selectedCounter)
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
}
