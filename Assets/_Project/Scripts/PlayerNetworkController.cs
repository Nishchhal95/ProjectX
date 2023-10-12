using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkController : NetworkBehaviour
{
    [SerializeField] private Behaviour[] behavioursToEnableForLocalPlayer;
    [SerializeField] private GameObject[] gameObjectsToEnableForLocalPlayer;
    [SerializeField] private GameObject[] gameObjectsToDisableForLocalPlayer;

    public PlayerController PlayerController { get; set; }
    public PlayerWeaponController PlayerWeaponController { get; set; }
    public PlayerStatsController PlayerStatsController { get; set; }

    public static Dictionary<ulong, PlayerNetworkController> playerNetworkControllers = new Dictionary<ulong, PlayerNetworkController>();

    public override void OnNetworkSpawn()
    {
        for (int i = 0; i < behavioursToEnableForLocalPlayer.Length; i++)
        {
            behavioursToEnableForLocalPlayer[i].enabled = IsLocalPlayer;
        }

        for (int i = 0; i < gameObjectsToEnableForLocalPlayer.Length; i++)
        {
            gameObjectsToEnableForLocalPlayer[i].SetActive(IsLocalPlayer);
        }

        for (int i = 0; i < gameObjectsToDisableForLocalPlayer.Length; i++)
        {
            gameObjectsToDisableForLocalPlayer[i].SetActive(!IsLocalPlayer);
        }

        PlayerController = GetComponent<PlayerController>();
        PlayerWeaponController = GetComponent<PlayerWeaponController>();
        PlayerStatsController = GetComponent<PlayerStatsController>();

        playerNetworkControllers.Add(NetworkObjectId, this);
    }

    public override void OnNetworkDespawn()
    {
        if (playerNetworkControllers.ContainsKey(NetworkObjectId))
        {
            playerNetworkControllers.Remove(NetworkObjectId);
        }
    }

    public static PlayerNetworkController GetPlayerNetworkController(ulong networkObjectId)
    {
        return playerNetworkControllers.ContainsKey(networkObjectId) ? playerNetworkControllers[networkObjectId] : null;
    }
}
