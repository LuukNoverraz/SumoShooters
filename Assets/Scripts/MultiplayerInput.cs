using System;
using Photon.Pun;
using UnityEngine;

public class MultiplayerInput : MonoBehaviourPun
{
    private GameController gameController;

    void Start() => gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    void Update()
    {
        if (photonView.IsMine)
        {
            if ((Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) && !gameController.pausing)
            {
                Debug.Log("yeahhhh");
                GetComponent<PlayerController>().BeginShoot();
            }
            if (Input.GetMouseButtonUp(0))
            {
                GetComponent<PlayerController>().EndShoot();
            }
        }
    }
}
