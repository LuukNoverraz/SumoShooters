using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    private List<GameObject> touchList = new List<GameObject>();
    private RaycastHit hit;
    public GameObject player1;
    public GameObject player2;
    private GameObject recipient;
    public GameController gameController;

    void Start() => gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    public void FingerDown()
    {
        // Check for which part of screen has been clicked and add player as recipient
        Ray mouseRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (mouseRay.origin.y <= 0)
        {
            recipient = player1;
            touchList.Add(recipient);
            recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
        }
        if (mouseRay.origin.y >= 0)
        {
            recipient = player2;
            touchList.Add(recipient);
            recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
        }
        FingerUp();
    }

    public void FingerUp()
    {
        // Called when player releases finger
        recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
        touchList.Remove(recipient);
    }
}
