using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private GameController gameController = null;
    void Start()
    {
        PhotonNetwork.Instantiate(gameController.name, Vector3.zero, Quaternion.identity);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
    }
}
