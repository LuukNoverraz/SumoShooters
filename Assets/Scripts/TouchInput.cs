using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchInput : MonoBehaviour
{
    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld; 
    RaycastHit hit;
    public GameObject player1;
    public GameObject player2;
    GameObject recipient;
    Scene currentScene;
    public GameController gameController;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();  
        currentScene = SceneManager.GetActiveScene();
    }
    void Update ()
    {
        #if UNITY_EDITOR
            if ((Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) && !gameController.pausing)
            {
            
                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Input.GetMouseButtonDown(0))
                {
                    // Debug.Log(ray.origin.y);
                    if (ray.origin.y <= 0)
                    {
                        recipient = player1;
                        touchList.Add(recipient);
                        // player1.GetComponent<PlayerController1>().BeginShoot();
                        // player1.GetComponent<PlayerControllerOnline>().BeginShoot();
                        recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (ray.origin.y >= 0)
                    {
                        recipient = player2;
                        touchList.Add(recipient);
                        // player2.GetComponent<PlayerController2>().BeginShoot();
                        // player2.GetComponent<PlayerControllerOnline>().BeginShoot();
                        recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }

                // foreach(GameObject g in touchesOld)
                // {
                //     if (!touchList.Contains(g))
                //     {
                //         g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                //     }
                // }
            }

            if (Input.GetMouseButtonUp(0))
            {
                // Debug.Log(recipient + " EndShoot");
                recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                touchList.Remove(recipient);
            }

            // if (Input.GetMouseButton(0))
            // {
            //     recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
            // }
        #endif

        if (Input.touchCount > 0 && !gameController.pausing)
        {
            // Debug.Log(Input.touchCount);
            
            // foreach(Touch touch in Input.touches)
            // {
            //     if (touch.phase == TouchPhase.Ended)
            //     {
            //         recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
            //     }
            // }

            foreach(Touch touch in Input.touches)
            {
                Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);

                if (touch.phase == TouchPhase.Began)
                {
                    // Debug.Log(ray.origin.y);
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
                // if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                // {
                //    recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                // }
                
                if (touch.phase == TouchPhase.Ended)
                {
                    // Debug.Log(recipient + " EndShoot");
                    recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                }

                if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Moved)
                {
                    recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
            // foreach(GameObject g in touchesOld)
            // {
            //     if (!touchList.Contains(g))
            //     {
            //         g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
            //     }
            // }
        }
    }
}
