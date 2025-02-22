using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private GameInput _gameInput;

    private bool _isWalking;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
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

    public bool IsWalking()
    {
        return _isWalking;
    }
}
