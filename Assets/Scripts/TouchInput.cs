using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld; 
    RaycastHit hit;
    public GameObject player1;
    public GameObject player2;
    GameObject recipient;
    public GameController gameController;
    void Start() => gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    void Update ()
    {
        #if UNITY_EDITOR
            if ((Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) && !gameController.pausing)
            {
            
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Input.GetMouseButtonDown(0))
                {
                    if (ray.origin.y <= 0)
                    {
                        recipient = player1;
                        touchList.Add(recipient);
                        recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (ray.origin.y >= 0)
                    {
                        recipient = player2;
                        touchList.Add(recipient);
                        recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                touchList.Remove(recipient);
            }
        #endif

        if (Input.touchCount > 0 && !gameController.pausing)
        {
            foreach(Touch touch in Input.touches)
            {
                Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);

                if (touch.phase == TouchPhase.Began)
                {
                    if (ray.origin.y <= 0)
                    {
                        recipient = player1;
                        touchList.Add(recipient);
                        recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (ray.origin.y >= 0)
                    {
                        recipient = player2;
                        touchList.Add(recipient);
                        recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
                
                if (touch.phase == TouchPhase.Ended)
                {
                    recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                }

                if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Moved)
                {
                    recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }
}
