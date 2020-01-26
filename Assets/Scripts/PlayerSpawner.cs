using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
    }
}
