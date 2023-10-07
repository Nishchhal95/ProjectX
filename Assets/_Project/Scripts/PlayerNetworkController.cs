using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkController : NetworkBehaviour
{
    [SerializeField] private Behaviour[] behavioursToToggle;
    [SerializeField] private GameObject[] gameObjectsToToggle;

    public PlayerController PlayerController { get; set; }
    public PlayerWeaponController PlayerWeaponController { get; set; }
    public PlayerStatsController PlayerStatsController { get; set; }

    public static Dictionary<ulong, PlayerNetworkController> playerNetworkControllers = new Dictionary<ulong, PlayerNetworkController>();

    public override void OnNetworkSpawn()
    {
        for (int i = 0; i < behavioursToToggle.Length; i++)
        {
            behavioursToToggle[i].enabled = IsLocalPlayer;
        }

        for (int i = 0; i < gameObjectsToToggle.Length; i++)
        {
            gameObjectsToToggle[i].SetActive(IsLocalPlayer);
        };

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
        return playerNetworkControllers.ContainsKey(networkObjectId) ? null : playerNetworkControllers[networkObjectId];
    }
}
