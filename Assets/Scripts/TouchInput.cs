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
    private bool fingerUp = false;
    void Start() => gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    public void FingerDown()
    {
        #if UNITY_EDITOR
            Debug.Log("Finger Down");
            Ray mouseRay = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (mouseRay.origin.y <= 0)
            {
                Debug.Log("player 1");
                recipient = player1;
                touchList.Add(recipient);
                recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
            }
            if (mouseRay.origin.y >= 0)
            {
                Debug.Log("player 2");
                recipient = player2;
                touchList.Add(recipient);
                recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
            }
            FingerUp();
        #endif

        // foreach(Touch touch in Input.touches)
        // {
        //     if (Input.touchCount > 0 && !gameController.pausing)
        //     {
        //         Ray touchRay = GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0));

        //         if (touchRay.origin.y <= 0)
        //         {
        //             Debug.Log(touchRay.origin.y);
        //             recipient = player1;
        //             touchList.Add(recipient);
        //             recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
        //         }
        //         if (touchRay.origin.y >= 0)
        //         {
        //             Debug.Log(touchRay.origin.y);
        //             recipient = player2;
        //             touchList.Add(recipient);
        //             recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
        //         }
        //         FingerUp();
        //     }
        // }
    }

    void Update()
    {
        if (fingerUp == true)
        {
           #if UNITY_EDITOR
                Debug.Log("update");
                recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
                touchList.Remove(recipient);
                fingerUp = false;
            #endif

        }
    }

    public void FingerUp()
    {
        Debug.Log("finger up");
        fingerUp = true;
        // #if UNITY_EDITOR
        //     Debug.Log("Finger Up");
        //     recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
        //     touchList.Remove(recipient);
        // #endif

        // foreach(Touch touch in Input.touches)
        // {
        //     if (touch.phase == TouchPhase.Ended)
        //     {
        //         recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
        //     }
        // }
    }

    // void Update ()
    // {
    //     #if UNITY_EDITOR
    //         if ((Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)) && !gameController.pausing)
    //         {
            
    //             Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

    //             if (Input.GetMouseButtonDown(0))
    //             {
    //                 if (ray.origin.y <= 0)
    //                 {
    //                     recipient = player1;
    //                     touchList.Add(recipient);
    //                     recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
    //                 }
    //                 if (ray.origin.y >= 0)
    //                 {
    //                     recipient = player2;
    //                     touchList.Add(recipient);
    //                     recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
    //                 }
    //             }
    //         }

    //         if (Input.GetMouseButtonUp(0))
    //         {
    //             recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
    //             touchList.Remove(recipient);
    //         }
    //     #endif

    //     if (Input.touchCount > 0 && !gameController.pausing)
    //     {
    //         foreach(Touch touch in Input.touches)
    //         {
    //             Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);

    //             if (touch.phase == TouchPhase.Began)
    //             {
    //                 if (ray.origin.y <= 0)
    //                 {
    //                     recipient = player1;
    //                     touchList.Add(recipient);
    //                     recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
    //                 }
    //                 if (ray.origin.y >= 0)
    //                 {
    //                     recipient = player2;
    //                     touchList.Add(recipient);
    //                     recipient.SendMessage("BeginShoot", hit.point, SendMessageOptions.DontRequireReceiver);
    //                 }
    //             }
                
    //             if (touch.phase == TouchPhase.Ended)
    //             {
    //                 recipient.SendMessage("EndShoot", hit.point, SendMessageOptions.DontRequireReceiver);
    //             }

    //             if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Moved)
    //             {
    //                 recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
    //             }
    //         }
    //     }
    // }
}
