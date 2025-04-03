using System;
using Unity.Netcode;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSqwaned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectScriptObject plateKitchenObjectSO;

    private float _sqwanPlateTimer;
    private float _sqwanPlateTimeMax = 4f;
    private int _plateSqwanedAmount;
    private int _plateSqwanedAmountMax = 4;

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }

        _sqwanPlateTimer += Time.deltaTime;
        if (_sqwanPlateTimer > _sqwanPlateTimeMax)
        {
            _sqwanPlateTimer = 0f;

            if (KitchenGameManager.Instance.IsGamePlaying() && _plateSqwanedAmount < _plateSqwanedAmountMax)
            {
                SpawnPlateServerRpc();
            }
        }
    }

    [ServerRpc]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }

    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        _plateSqwanedAmount++;

        OnPlateSqwaned?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is empty handed
            if (_plateSqwanedAmount > 0)
            {
                // there's at least one plate here
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                InteractLoginServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLoginServerRpc()
    {
        InteractLoginClientRpc();
    }

    [ClientRpc]
    private void InteractLoginClientRpc()
    {
        _plateSqwanedAmount--;
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
