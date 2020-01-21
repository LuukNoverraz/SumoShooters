// VER  5.12.0
// ♥
//Script per gestire interazioni tramite tocco. Supporta mouse e multitocco! :)

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

#region [.] GESTURE
///<summary>Touchable: Pacchetto contenente informazioni sulla gesture</summary>
[System.Serializable]
public class Gesture
{
    Touchable manager;

    ///<summary> Ritorna il touchable a cui appartiene questa Gesture </summary>
    public Touchable GetTouchable() { return manager; }

    public Tocco tocco_1, tocco_2;

    ///<summary>Il gameObject al quale appartiene il tocco (ovvero quello al quale è attaccato il Touchable)</summary>
    public GameObject gameObject;

    ///<summary>Durata della gesture in secondi</summary>
    public float tempoGesture;
    ///<summary>Momento di inizio della gesture in secondi</summary>
    public float tempoInizioGesture;
    ///<summary>Momento di fine della gesture in secondi</summary>
    public float tempoFineGesture;

    ///<summary>Posizione media tra i due tocchi della gesture all'inizio</summary>
    public Vector3 posizioneMediaInizialeScreen;
    ///<summary>Posizione mediana tra i due tocchi della gesture all'inizio</summary>
    public Vector3 posizioneMediaInizialeWorld;
    ///<summary>Posizione media tra i due tocchi della gesture</summary>
    public Vector3 posizioneMediaScreen;
    ///<summary>Posizione media tra i due tocchi della gesture</summary>
    public Vector3 posizioneMediaWorld;
    ///<summary>Differenza della posizione media dal frame precedente</summary>
    public Vector3 deltaPosizioneScreen;
    ///<summary>Differenza della posizione media dal frame precedente</summary>
    public Vector3 deltaPosizioneWorld;

    ///<summary>Distanza iniziale tra le posizioni dei due tocchi della gesture</summary>
    public float distanzaTocchiInizialeScreen;
    ///<summary>Distanza tra le posizioni dei due tocchi della gesture</summary>
    public float distanzaTocchiScreen;
    ///<summary>Differenza dal frame precedente tra le posizioni dei due tocchi della gesture</summary>
    public float deltaDistanzaTocchiScreen;

    ///<summary>Distanza iniziale tra le posizioni dei due tocchi della gesture</summary>
    public float distanzaTocchiInizialeWorld;
    ///<summary>Distanza tra le posizioni dei due tocchi della gesture</summary>
    public float distanzaTocchiWorld;
    ///<summary>Differenza dal frame precedente tra le posizioni dei due tocchi della gesture</summary>
    public float deltaDistanzaTocchiWorld;

    //TODO da 0 a 360 (ora 180,-180)
    ///<summary>Angolo tra le due dita all'inizio della gesture (partendo dall'alto)</summary>
    public float rotazioneIniziale;
    ///<summary>Angolo tra le due dita (partendo dall'alto)</summary>
    public float rotazione;
    ///<summary>Differenza angolo tra le due dita dal frame precedente (partendo dall'alto)</summary>
    public float deltaRotazione;

    ///<summary>Ha eseguito un PINCH?</summary>
    public bool hasPinched;
    ///<summary>Ha eseguito un PAN?</summary>
    public bool hasPanned;
    ///<summary>Ha eseguito un ROTATE?</summary>
    public bool hasRotated;


    public Gesture(Touchable owner, Tocco tocco1, Tocco tocco2)
    {
        manager = owner;
        gameObject = owner.gameObject;
        tocco_1 = tocco1;
        tocco_2 = tocco2;

        Start();

    }


    void Start()
    {
        tempoInizioGesture = Time.unscaledTime;

        distanzaTocchiScreen = Vector3.Distance(tocco_1.posizioneScreen, tocco_2.posizioneScreen);
        distanzaTocchiInizialeScreen = distanzaTocchiScreen;

        posizioneMediaScreen = Vector3.Lerp(tocco_1.posizioneScreen, tocco_2.posizioneScreen, 0.5f);
        posizioneMediaWorld = Vector3.Lerp(tocco_1.posizioneWorld, tocco_2.posizioneWorld, 0.5f);
        posizioneMediaInizialeScreen = posizioneMediaScreen;
        posizioneMediaInizialeWorld = posizioneMediaWorld;

        rotazione = GetRotation();
        rotazioneIniziale = rotazione;




        OnGestureBegin();

    }

    public void Update()
    {

        deltaDistanzaTocchiScreen = Vector3.Distance(tocco_1.posizioneScreen, tocco_2.posizioneScreen) - distanzaTocchiScreen;
        distanzaTocchiScreen = Vector3.Distance(tocco_1.posizioneScreen, tocco_2.posizioneScreen);

        deltaDistanzaTocchiWorld = Vector3.Distance(tocco_1.posizioneWorld, tocco_2.posizioneWorld) - distanzaTocchiWorld;
        distanzaTocchiWorld = Vector3.Distance(tocco_1.posizioneWorld, tocco_2.posizioneWorld);

        deltaPosizioneScreen = Vector3.Lerp(tocco_1.posizioneScreen, tocco_2.posizioneScreen, 0.5f) - posizioneMediaScreen;
        posizioneMediaScreen = Vector3.Lerp(tocco_1.posizioneScreen, tocco_2.posizioneScreen, 0.5f);

        deltaPosizioneWorld = Vector3.Lerp(tocco_1.posizioneWorld, tocco_2.posizioneWorld, 0.5f) - posizioneMediaWorld;
        posizioneMediaWorld = Vector3.Lerp(tocco_1.posizioneWorld, tocco_2.posizioneWorld, 0.5f);

        deltaRotazione = GetRotation() - rotazione;
        rotazione = GetRotation();

        tempoGesture += manager.deltaTime;
        OnGestureKeep();



        if (!hasPinched)
        {
            if ((manager.permettiSoloUnGestureAllaVolta && !hasPanned && !hasRotated) || (!manager.permettiSoloUnGestureAllaVolta))
                if (Mathf.Abs(distanzaTocchiScreen - distanzaTocchiInizialeScreen) >= manager.sogliaDistanzaPinch)
                {
                    hasPinched = true;
                    OnPinchBegin();
                }

        }
        else
        {
            if ((manager.permettiSoloUnGestureAllaVolta && !hasPanned && !hasRotated) || (!manager.permettiSoloUnGestureAllaVolta))
                if (deltaDistanzaTocchiScreen != 0)
                {
                    OnPinchChanged();
                }
        }

        if (!hasPanned)
        {
            if ((manager.permettiSoloUnGestureAllaVolta && !hasPinched && !hasRotated) || (!manager.permettiSoloUnGestureAllaVolta))
                if (Vector3.Distance(posizioneMediaInizialeScreen, posizioneMediaScreen) >= manager.sogliaDistanzaPan && Mathf.Abs(distanzaTocchiInizialeScreen - distanzaTocchiScreen) < manager.sogliaDistanzaTocchiPan)
                {
                    hasPanned = true;
                    OnPanBegin();
                }
        }
        else
        {
            if ((manager.permettiSoloUnGestureAllaVolta && !hasPinched && !hasRotated) || (!manager.permettiSoloUnGestureAllaVolta))
                if (deltaPosizioneScreen != Vector3.zero)
                    OnPanChanged();
        }

        if (!hasRotated)
        {
            if ((manager.permettiSoloUnGestureAllaVolta && !hasPinched && !hasPanned) || (!manager.permettiSoloUnGestureAllaVolta))
                if (Mathf.Abs(rotazione - rotazioneIniziale) > manager.sogliaRotazione)
                {
                    hasRotated = true;
                    OnRotateBegin();
                }
        }
        else
        {
            if ((manager.permettiSoloUnGestureAllaVolta && !hasPinched && !hasPanned) || (!manager.permettiSoloUnGestureAllaVolta))
                if (deltaRotazione != 0)
                    OnRotateChanged();
        }
    }

    float GetRotation()
    {
        return Vector3.SignedAngle((tocco_1.posizioneWorld - tocco_2.posizioneWorld).normalized, manager.telecamera.transform.up, -manager.telecamera.transform.forward);
    }


    ///<summary> NON CHIAMARE DA SCRIPT ESTERNI TRANNE IL TOUCHABLE 
    ///<para> Distrugge il tocco chiamando tutti gli eventi collegato </para>
    ///</summary>
    public void Distruggi()
    {
        tempoFineGesture = Time.unscaledTime;
        OnGestureEnd();
    }


    ///<summary> NON CHIAMARE DA SCRIPT ESTERNI TRANNE IL TOUCHABLE  
    ///<para> Cancella il tocco senza chiamare nessun evento collegato </para>
    ///</summary>
    public void Cancella()
    {
        tempoFineGesture = Time.unscaledTime;
        OnGestureCancel();
    }


    //•••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••//
    //EVENTI

    void OnGestureBegin()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnGestureBegin");

        if (manager.eventiGestures.onGestureBegin.GetPersistentEventCount() > 0)
            manager.eventiGestures.onGestureBegin.Invoke(this);
    }


    void OnPinchBegin()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnPinchBegin");

        if (manager.eventiGestures.onPinchBegin.GetPersistentEventCount() > 0)
            manager.eventiGestures.onPinchBegin.Invoke(this);
    }

    void OnPinchChanged()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnPinchChanged");

        if (manager.eventiGestures.onPinchChanged.GetPersistentEventCount() > 0)
            manager.eventiGestures.onPinchChanged.Invoke(this);
    }

    void OnPanBegin()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnPanBegin");

        if (manager.eventiGestures.onPanBegin.GetPersistentEventCount() > 0)
            manager.eventiGestures.onPanBegin.Invoke(this);
    }

    void OnPanChanged()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnPanChanged");

        if (manager.eventiGestures.onPanChanged.GetPersistentEventCount() > 0)
            manager.eventiGestures.onPanChanged.Invoke(this);
    }

    void OnRotateBegin()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnRotateBegin");

        if (manager.eventiGestures.onRotateBegin.GetPersistentEventCount() > 0)
            manager.eventiGestures.onRotateBegin.Invoke(this);
    }

    void OnRotateChanged()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnRotateChanged");

        if (manager.eventiGestures.onRotateChanged.GetPersistentEventCount() > 0)
            manager.eventiGestures.onRotateChanged.Invoke(this);
    }

    void OnGestureKeep()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnGestureKeep");

        if (manager.eventiGestures.onGestureKeep.GetPersistentEventCount() > 0)
            manager.eventiGestures.onGestureKeep.Invoke(this);
    }

    void OnGestureEnd()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnGestureEnd");

        if (manager.eventiGestures.onGestureEnd.GetPersistentEventCount() > 0)
            manager.eventiGestures.onGestureEnd.Invoke(this);
    }

    void OnGestureCancel()
    {
        if (manager.debug_log)
            manager.DebugPrint("OnGestureCancel");

        if (manager.eventiGestures.onGestureCancel.GetPersistentEventCount() > 0)
            manager.eventiGestures.onGestureCancel.Invoke(this);
    }



}
#endregion


//////////////////////////////////////////////////////


#region [,] TOCCO
///<summary>Touchable: Pacchetto contenente informazioni sul tocco</summary>
[System.Serializable]
public class Tocco
{
    ///<summary> L'identificativo unico del tocco. 
    /// <para>Se >=0 è TOUCH </para> 
    /// <para>Se tra -1 e -10 è MOUSE </para> 
    /// <para>Se <=-11 è VIRTUALE </para> 
    ///</summary>
    public int fingerId;

    public TipoTocco tipoTocco;
    public enum TipoTocco
    {
        ///<summary> Un Tocco() generato da un Touch </summary>
        Touch,
        ///<summary> Un Tocco() generato da un Mouse </summary>
        Mouse,
        ///<summary> Un Tocco() generato da uno script </summary>
        Virtuale
    }

    Touchable manager;

    ///<summary> Ritorna il touchable a cui appartiene questo Tocco </summary>
    public Touchable GetTouchable() { return manager; }


    ///<summary>Il gameObject al quale appartiene il tocco (ovvero quello al quale è attaccato il Touchable)</summary>
    public GameObject gameObject;

    //World
    ///<summary>Posizione inziale del tocco in coordinate WORLD</summary>
    public Vector3 posizioneInizialeWorld;
    ///<summary>Posizione del tocco in coordinate WORLD</summary>
    public Vector3 posizioneWorld;
    ///<summary>Differenza posizione del tocco dal frame precedente in coordinate WORLD</summary>
    public Vector3 deltaPosizioneWorld;
    ///<summary>Differenza posizione del tocco dal frame precedente accumulato a quello del frame prima in coordinate WORLD</summary>
    public Vector3 deltaPosizioneWorldSmooth;
    ///<summary>Differenza posizione del tocco dal frame precedente a quello del frame prima in coordinate WORLD</summary>
    public Vector3 deltaPosizioneWorldFramePrecedente;
    ///<summary>Differenza posizione del tocco dal centro dell'oggetto in coordinate WORLD</summary>
    public Vector3 offsetToccoInizialeWorld;
    ///<summary> Contiene tutti i dati del RaycastHit (normale, UV texcoord, indice del triangolo colpito...) </summary>
    public RaycastHit informazioniRaycast;

    //Screen
    ///<summary>Posizione inziale del tocco in coordinate normalizzate</summary>
    public Vector3 posizioneInizialeScreen;
    ///<summary>Posizione del tocco in coordinate normalizzate. L'angolo in alto a sinistra è x=0 y=1</summary>
    public Vector3 posizioneScreen;
    ///<summary>Differenza posizione del tocco dal frame precedente in coordinate normalizzate</summary>
    public Vector3 deltaPosizioneScreen;
    ///<summary>Differenza posizione del tocco dal frame precedente accumulato a quello del frame prima in coordinate normalizzate</summary>
    public Vector3 deltaPosizioneScreenSmooth;
    ///<summary>Differenza posizione del tocco dal frame precedente a quello del frame prima in coordinate normalizzate</summary>
    public Vector3 deltaPosizioneScreenFramePrecedente;
    ///<summary>Differenza posizione del tocco dal centro dell'oggetto in coordinate normalizzate</summary>
    public Vector3 offsetToccoInizialeScreen;

    //Pixel
    ///<summary>Posizione inziale del tocco in coordinate normalizzate</summary>
    public Vector3 posizioneInizialePixel;
    ///<summary>Posizione del tocco in coordinate normalizzate. L'angolo in alto a sinistra è x=0 y=1</summary>
    public Vector3 posizionePixel;

    //Virtuale
    ///<summary> La posizione in pixel del tocco virtuale (non si applica agli altri tipi) </summary>
    public Vector3 posizioneVirtuale;

    ///<summary>Il tocco si trova sull'oggetto di appartenenza?</summary>
    public bool isOnGameObject;
    ///<summary>Il tocco è iniziato sull'oggetto?</summary>
    public bool oggettoPremuto;
    ///<summary>Abbiamo trascinato?</summary>
    public bool hasDragged;
    ///<summary>Abbiamo premuto a lungo?</summary>
    public bool hasLongPressed;
    ///<summary>Il tocco si trova sopra l'UI?</summary>
    public bool isOverUI { get { return IsOverUI(); } }
    ///<summary>Transform dell'oggetto al quale appartiene il tocco</summary>
    public Transform transformOggetto;

    ///<summary>Durata del tocco in secondi</summary>
    public float tempoTocco;
    ///<summary>Momento iniziale del tocco in secondi</summary>
    public float tempoInizioTocco;
    ///<summary>Momento finale del tocco in secondi</summary>
    public float tempoFineTocco;

    ///<summary>Distanza percorsa in coordinate WORLD</summary>
    public float distanzaPercorsaWorld;
    ///<summary>Distanza percorsa in coordinate normalizzate</summary>
    public float distanzaPercorsaScreen;

    ///<summary>Direzione dello swipe</summary>
    public Vector3 direzioneSwipeScreen;
    ///<summary>Direzione dello swipe con coordinate WORLD</summary>
    public Vector3 direzioneSwipeWorld;

    ///<summary> Il tocco è al suo primo frame di vita? </summary>
    public bool inizializziato = false;

    ///<summary> Valore in cache per capire se il tocco è ancora sull'oggetto </summary>
    bool valoreIsOnGameObjectDaSettare = false;




    public Tocco(Touchable owner, int id, TipoTocco tipoTocco, Vector3 posizioneVirtualeIniziale = default(Vector3))
    {
        manager = owner;
        gameObject = owner.gameObject;
        fingerId = id;
        transformOggetto = owner.myTrans;
        this.tipoTocco = tipoTocco;
        if (tipoTocco == TipoTocco.Virtuale)
            posizioneVirtuale = posizioneVirtualeIniziale;


        Start();
    }

    ///<summary> Velocita media del tocco dall'inizio ad ora in coordinate SCREEN normalizzate </summary>
    public float VelocitaMediaScreen()
    {
        return distanzaPercorsaScreen / tempoTocco;
    }

    ///<summary> Velocita media del tocco dall'inizio ad ora in coordinate WORLD </summary>
    public float VelocitaMediaWorld()
    {
        return distanzaPercorsaWorld / tempoTocco;
    }

    void Start()
    {

        tempoInizioTocco = Time.unscaledTime;

        posizioneInizialePixel = GetPixelPosition();
        posizionePixel = posizioneInizialePixel;

        posizioneInizialeWorld = GetWorldPosition(true, true);
        posizioneWorld = posizioneInizialeWorld;
        offsetToccoInizialeWorld = transformOggetto.position - posizioneInizialeWorld;

        posizioneInizialeScreen = GetScreenPosition(true);
        posizioneScreen = posizioneInizialeScreen;
        // offsetToccoInizialeScreen = manager.telecamera.WorldToViewportPoint(transformOggetto.position).ModificaZ(transformOggetto.position.z) - posizioneInizialeScreen;


        // Ora non serve più perchè controlliamo direttamente quando chiamiamo GetWorldPosition()
        // Millisecondi preziosi guadagnati!
        //CheckIsOnGameObject();
        CheckIsOnGameObject();

        if (isOnGameObject)
        {
            oggettoPremuto = true;
            OnPress();
        }

        inizializziato = true;
    }



    public void Update()
    {
        posizionePixel = GetPixelPosition();
        // TODO Quando servirà, implementa qui le coordinate in Pixel


        deltaPosizioneWorldFramePrecedente = deltaPosizioneWorld;
        Vector3 nuovaPosizioneWorld = GetWorldPosition(true, true);
        deltaPosizioneWorld = nuovaPosizioneWorld - posizioneWorld;
        deltaPosizioneWorldSmooth = (deltaPosizioneWorldFramePrecedente + deltaPosizioneWorld) * .5f;
        posizioneWorld = nuovaPosizioneWorld;
        distanzaPercorsaWorld += deltaPosizioneWorld.magnitude;

        deltaPosizioneScreenFramePrecedente = deltaPosizioneScreen;
        Vector3 nuovaPosizioneScreen = GetScreenPosition(true);
        deltaPosizioneScreen = nuovaPosizioneScreen - posizioneScreen;
        deltaPosizioneScreenSmooth = (deltaPosizioneScreenFramePrecedente + deltaPosizioneScreen) * .5f;
        posizioneScreen = nuovaPosizioneScreen;
        distanzaPercorsaScreen += deltaPosizioneScreen.magnitude;

        // Ora non serve più perchè controlliamo direttamente quando chiamiamo GetWorldPosition()
        // Millisecondi preziosi guadagnati!
        //CheckIsOnGameObject();
        CheckIsOnGameObject();


        tempoTocco += Time.deltaTime;
        OnKeep();

        if (oggettoPremuto)
        {
            if (!hasLongPressed && !hasDragged && tempoTocco >= manager.sogliaTempoLongPress)
            {
                OnLongPress();
                hasLongPressed = true;
            }

            if (deltaPosizioneScreen != Vector3.zero)
                if (Vector3.Distance(posizioneScreen, posizioneInizialeScreen) > manager.sogliaDistanzaDrag)
                {
                    if (!hasDragged)
                    {
                        OnBeginDrag();
                        hasDragged = true;
                    }
                    OnDrag();
                }
        }
    }

    ///<summary> Imposta la posizione in PIXEL del tocco virtuale </summary>
    ///<para name="usaPosizioneWorld"> Se TRUE converte in automatico la posizione WORLD indicata </para>
    public void SetPosizioneVirtuale(Vector3 posizioneVirtuale, bool usaPosizioneWorld = false)
    {
        if (usaPosizioneWorld)
            this.posizioneVirtuale = manager.telecamera.WorldToScreenPoint(posizioneVirtuale);
        else
            this.posizioneVirtuale = posizioneVirtuale;
    }

    ///<summary>Posizione del tocco in PIXEL</summary>
    Vector3 GetPixelPosition()
    {
        switch (tipoTocco)
        {
            // TOUCH
            case TipoTocco.Touch:
                if (Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        Touch touch = Input.GetTouch(i);
                        if (fingerId == touch.fingerId)
                        {
                            return touch.position;
                        }
                    }
                    return Vector3.zero;
                }
                return Vector3.zero;

            // MOUSE
            case TipoTocco.Mouse:
                return (Input.mousePosition);

            // VIRTUAL
            case TipoTocco.Virtuale:
                return posizioneVirtuale;

            // BOH
            default: return Vector3.zero;

        }
    }

    RaycastHit2D[] hits2D = new RaycastHit2D[1];
    ///<summary>Posizione del tocco in WORLD COORDINATES</summary>
    Vector3 GetWorldPosition(bool usaPosizionePixelInCache = false, bool controllaSeIsOnGameObject = false)
    {
        Ray ray = manager.telecamera.ScreenPointToRay(usaPosizionePixelInCache ? posizionePixel : GetPixelPosition());
        if (manager.is2dCollider)
        {

            Physics2D.GetRayIntersectionNonAlloc(ray, hits2D, manager.lunghezzaRaycast, manager.layerMask);

            if (hits2D[0].transform == manager.myTrans)
            {
                Vector3 posizione2D = new Vector3(hits2D[0].point.x, hits2D[0].point.y, transformOggetto.position.z);
                if (controllaSeIsOnGameObject) SetIsOnGameObjectInCache(true);
                return posizione2D;
            }
            else
            {
                if (controllaSeIsOnGameObject) SetIsOnGameObjectInCache(false);
            }

        }
        else
        {

            RaycastHit hit;
            if (manager.canPassThroughUI || (!manager.canPassThroughUI && !IsOverUI()))
            {
                if (Physics.Raycast(ray, out hit, manager.lunghezzaRaycast, manager.layerMask) && (hit.transform == manager.myTrans))
                {
                    informazioniRaycast = hit;
                    if (controllaSeIsOnGameObject) SetIsOnGameObjectInCache(true);
                    return hit.point;
                }
                else
                {
                    if (controllaSeIsOnGameObject) SetIsOnGameObjectInCache(false);
                }
            }
            else
            {
                if (controllaSeIsOnGameObject) { if (isOnGameObject) SetIsOnGameObjectInCache(false); }
            }

        }

        return posizioneWorld;
    }


    ///<summary> Controlla se il tocco è sul gameObject dove è attivo il Touchable </summary>
    // void CheckIsOnGameObject()
    // {
    //     Ray ray = manager.telecamera.ScreenPointToRay(GetPixelPosition());

    //     if (manager.is2dCollider)
    //     {

    //         Physics2D.GetRayIntersectionNonAlloc(ray, hits2D, manager.lunghezzaRaycast, manager.layerMask);
    //         if (hits2D.Length > 0)
    //         {
    //             if (hits2D[0].transform == manager.myTrans)
    //             {
    //                 SetIsOnGameObject(true);

    //             }
    //             else
    //             {
    //                 SetIsOnGameObject(false);
    //             }
    //         }
    //         else
    //         {
    //             if (isOnGameObject)
    //                 SetIsOnGameObject(false);
    //         }
    //     }
    //     else
    //     {

    //         RaycastHit hit;
    //         if (manager.canPassThroughUI || (!manager.canPassThroughUI && !IsOverUI()))
    //         {
    //             if (Physics.Raycast(ray, out hit, manager.lunghezzaRaycast, manager.layerMask) && (hit.transform == manager.myTrans))
    //             {
    //                 SetIsOnGameObject(true);
    //             }
    //             else
    //             {
    //                 SetIsOnGameObject(false);
    //             }
    //         }
    //         else
    //         {
    //             if (isOnGameObject)
    //                 SetIsOnGameObject(false);
    //         }
    //     }

    // }

    void SetIsOnGameObjectInCache(bool valore)
    {
        valoreIsOnGameObjectDaSettare = valore;
    }

    void CheckIsOnGameObject()
    {
        SetIsOnGameObject(valoreIsOnGameObjectDaSettare);
    }

    void SetIsOnGameObject(bool valore)
    {
        if (valore == true)
        {
            if (!isOnGameObject)
            {
                // Se NON abbiamo selezionato l'opzione che OnPress non chiama OnEnter, chiamiamolo a prescindere.
                if (manager.onPressNonChiamaOnEnter == false)
                    OnEnter();
                else
                // Se abbiamo selezionato l'opzione, chiamiamolo solo quando è già stato inizializzato (quindi non al primo frame!)
                if (manager.onPressNonChiamaOnEnter && inizializziato == true)
                    OnEnter();
            }
        }
        else
        {
            if (isOnGameObject)
                OnExit();
        }


        isOnGameObject = valore;
    }


    // TODO implementa!
    /* public Vector3 GetPointerNearPlanePosition(Tocco tocco)
    {
        Vector3 coordinate = manager.telecamera.ScreenPointToRay(Input.mousePosition).GetPoint(manager.nearClipPlane);
        return coordinate;

    }*/


    ///<summary>Posizione del tocco da 0 a 1</summary>
    public Vector3 GetScreenPosition(bool usaPosizionePixelInCache = false)
    {
        Vector3 coordinateNormalizzate = manager.telecamera.ScreenToViewportPoint(usaPosizionePixelInCache ? posizionePixel : GetPixelPosition());
        coordinateNormalizzate.x = Mathf.Clamp(coordinateNormalizzate.x, 0f, 1f);
        coordinateNormalizzate.y = Mathf.Clamp(coordinateNormalizzate.y, 0f, 1f);
        return coordinateNormalizzate;
    }

    ///<summary> Controlla se il tocco è su un elemento UI di una Canvas (con raycast attivo). I tocchi virtuali ritornano sempre FALSE </summary>
    public bool IsOverUI()
    {
        if (!manager.eventSystem)
            return false;

        switch (tipoTocco)
        {
            // TOUCH
            case TipoTocco.Touch:
                if (Input.touchCount > 0)
                {
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        var touch = Input.GetTouch(i);
                        if (touch.fingerId == fingerId)
                        {
                            return manager.eventSystem.IsPointerOverGameObject(i);
                        }
                    }
                }
                return false;

            // MOUSE
            case TipoTocco.Mouse:
                return manager.eventSystem.IsPointerOverGameObject(fingerId);

            // VIRTUALE
            case TipoTocco.Virtuale:
                return false;

            // DEFAULT
            default: return false;
        }
    }

    ///<summary> NON CHIAMARE DA SCRIPT ESTERNI TRANNE IL TOUCHABLE  
    ///<para> Cancella il tocco senza chiamare nessun evento collegato </para>
    ///</summary>
    public void Cancella()
    {
        tempoFineTocco = Time.unscaledTime;
        OnCancel();
    }

    ///<summary> NON CHIAMARE DA SCRIPT ESTERNI TRANNE IL TOUCHABLE 
    ///<para> Distrugge il tocco chiamando tutti gli eventi collegato </para>
    ///</summary>
    public void Distruggi()
    {

        tempoFineTocco = Time.unscaledTime;

        if (oggettoPremuto)
        {
            OnRelease();

            if (!hasDragged && tempoTocco < manager.sogliaTempoTap)
            {
                IstanzaTap tapPrecedente = GetIstanzaTap();
                if (tapPrecedente != null)
                {
                    OnDoubleTap();
                    manager.DistruggiIstanzaTap(tapPrecedente);
                }
                else
                {
                    OnTap();
                    if (manager.debug_log || manager.eventiTocco.onDoubleTap.GetPersistentEventCount() > 0)
                        manager.CreaIstanzaTap(new IstanzaTap(manager, posizioneScreen));
                }

            }
        }

    }

    ///<summary> Controlla se ci sono le condizioni per fare un doppio tap  </summary>
    bool CheckDoubleTap()
    {
        if (manager.eventiTocco.onDoubleTap.GetPersistentEventCount() < 0)
            return false;

        int numeroTap = manager.listaIstanzeTap.Count;

        if (numeroTap <= 0)
            return false;


        for (int i = 0; i < manager.listaIstanzeTap.Count; i++)
        {
            if (Vector2.Distance(posizioneScreen, manager.listaIstanzeTap[i].posizioneScreen) < manager.sogliaDistanzaDoubleTap)
                return true;
        }

        return false;
    }


    IstanzaTap GetIstanzaTap()
    {
        if (manager.eventiTocco.onDoubleTap.GetPersistentEventCount() < 0)
            return null;

        int numeroTap = manager.listaIstanzeTap.Count;

        if (numeroTap <= 0)
            return null;


        for (int i = 0; i < manager.listaIstanzeTap.Count; i++)
        {
            if (Vector2.Distance(posizioneScreen, manager.listaIstanzeTap[i].posizioneScreen) < manager.sogliaDistanzaDoubleTap)
                return manager.listaIstanzeTap[i];
        }

        return null;
    }


    //•••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••••//
    //EVENTI

    void OnPress()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onPress");

        if (manager.eventiTocco.onPress != null)
            manager.eventiTocco.onPress.Invoke(this);
    }

    void OnEnter()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onEnter");

        if (manager.eventiTocco.onEnter != null)
        {
            //Se esistono callback per OnEnter consideriamo un oggetto premuto anche senza l'evento OnPress
            //SENNO DI POI: Ma non capisco perchè onestamente... Io lo commento.
            //SENNO DI POI POI: Ora capisco... così l'evento on enter è valido solo se entra da fuori (credo)
            if (manager.onEnterContaComePremuto)
                oggettoPremuto = true;
            manager.eventiTocco.onEnter.Invoke(this);
        }
    }

    void OnKeep()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onKeep");

        if (manager.eventiTocco.onKeep != null)
            manager.eventiTocco.onKeep.Invoke(this);
    }

    void OnExit()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onExit");

        if (manager.eventiTocco.onExit != null)
            manager.eventiTocco.onExit.Invoke(this);
    }

    void OnRelease()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onRelease");

        if (tempoTocco < manager.sogliaTempoSwipe && distanzaPercorsaScreen > manager.sogliaDistanzaSwipe && VelocitaMediaScreen() > manager.sogliaVelocitaSwipe)
        {

            direzioneSwipeScreen = posizioneScreen - posizioneInizialeScreen;
            direzioneSwipeWorld = posizioneWorld - posizioneInizialeWorld;

            OnSwipe();
        }


        if (manager.eventiTocco.onRelease.GetPersistentEventCount() > 0)
            manager.eventiTocco.onRelease.Invoke(this);
    }

    void OnBeginDrag()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onBeginDrag");

        if (manager.eventiTocco.onBeginDrag.GetPersistentEventCount() > 0)
            manager.eventiTocco.onBeginDrag.Invoke(this);
    }

    void OnDrag()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onDrag");

        if (manager.eventiTocco.onDrag.GetPersistentEventCount() > 0)
            manager.eventiTocco.onDrag.Invoke(this);
    }

    void OnTap()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onTap");

        if (manager.eventiTocco.onTap.GetPersistentEventCount() > 0)
            manager.eventiTocco.onTap.Invoke(this);
    }

    void OnDoubleTap()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onDoubleTap");

        if (manager.eventiTocco.onDoubleTap.GetPersistentEventCount() > 0)
            manager.eventiTocco.onDoubleTap.Invoke(this);
    }

    void OnLongPress()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onLongPress");

        if (manager.eventiTocco.onLongPress.GetPersistentEventCount() > 0)
            manager.eventiTocco.onLongPress.Invoke(this);
    }

    void OnSwipe()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onSwipe");

        if (manager.eventiTocco.onSwipe.GetPersistentEventCount() > 0)
            manager.eventiTocco.onSwipe.Invoke(this);
    }

    void OnCancel()
    {
        if (manager.debug_log)
            manager.DebugPrint(fingerId + " onCancel");

        if (manager.eventiTocco.onCancel.GetPersistentEventCount() > 0)
            manager.eventiTocco.onCancel.Invoke(this);
    }


}

#endregion



/////////////////////////////////////////////////



#region [.] ISTANZA TAP

///<summary> Una classe che rappresenta un TAP </summary>
[System.Serializable]
public class IstanzaTap
{

    public IstanzaTap(Touchable scriptTouchable, Vector2 posizione)
    {
        scriptManager = scriptTouchable;
        posizioneScreen = posizione;
    }

    Touchable scriptManager;
    public Vector2 posizioneScreen;
    float tempoVita;

    public void Update()
    {
        tempoVita += scriptManager.deltaTime;
        if (tempoVita > scriptManager.sogliaTempoDoubleTap)
        {
            scriptManager.DistruggiIstanzaTap(this);
        }
    }
}

#endregion


////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////

///<summary>Touchable: Pacchetto che contiene le informazioni sul tocco</summary>
[System.Serializable]
public class EventoTocco : UnityEvent<Tocco> { }

///<summary>Touchable: Pacchetto che contiene le informazioni sulla gesture eseguita</summary>
[System.Serializable]
public class EventoGesture : UnityEvent<Gesture> { }

[System.Serializable]
public class EventiTocco
{
    public EventoTocco onPress, onEnter, onKeep, onExit, onRelease, onBeginDrag, onDrag, onTap, onLongPress, onSwipe, onDoubleTap, onCancel;
}


[System.Serializable]
public class EventiGesture
{
    public EventoGesture onGestureBegin, onGestureKeep, onPinchBegin, onPinchChanged, onPanBegin, onPanChanged, onRotateBegin, onRotateChanged, onGestureEnd, onGestureCancel;
}

///<summary>Componente per gestire input touch e mouse. Togo!</summary>
public class Touchable : MonoBehaviour
{

    [Header("Impostazioni")]

    [Tooltip("La telecamera da considerare per eseguire il raycasting")]
    public Camera telecamera;
    [Tooltip("Il layer da prendere in considerazione")]
    public LayerMask layerMask;
    [Tooltip("Se non ti serve così lunga riducila")]
    public float lunghezzaRaycast = 100f;
    [Range(1, 10),Tooltip("Il numero di tocchi TOTALI su schermo consentiti (non solo quelli su questo Touchable)")]
    public int tocchiMassimiConsentiti = 2;
    [Tooltip("Se TRUE un evento OnEnter rende l'oggetto premuto (e di conseguenza chiama il OnRelease senza aver chiamato un OnPress)")]
    public bool onEnterContaComePremuto = false;
    [Tooltip("Se TRUE un evento OnPress non richiama un evento OnEnter, quindi appena si tocca un'oggetto non verrà chiamato OnEnter (Ma sarà chiamato solo quando il dito esce e rientra)")]
    public bool onPressNonChiamaOnEnter = false;
    [Tooltip("Può attraversare la UI? Di default=no")]
    public bool canPassThroughUI = false;
    [Tooltip("Touch detection sulla FixedUpdate? NON CONSIGLIATO, skippa input!")]
    public bool runOnFixedUpdate = false;
    [Tooltip("Touch e Mouse non creano tocchi, ma solo gli script (utile per la CPU o tutorial per esempio)")]
    public bool permettiSoloTocchiVirtuali = false;

    [Space(10)]
    [Header("Tocchi")]
    [Tooltip("Distanza normalizzata prima di attivare il DRAG")]
    public float sogliaDistanzaDrag = 0.01f;
    [Tooltip("Tempo massimo per essere considerato un TAP")]
    public float sogliaTempoTap = 0.26f;
    [Tooltip("Tempo minimo per attivare LONG PRESS")]
    public float sogliaTempoLongPress = 0.5f;
    [Tooltip("Tempo massimo per registrare SWIPE")]
    public float sogliaTempoSwipe = 0.3f;
    [Tooltip("Distanza normalizzata prima di registrare SWIPE")]
    public float sogliaDistanzaSwipe = 0.03f;
    [Tooltip("Velocita minima per registrare SWIPE")]
    public float sogliaVelocitaSwipe = 0.2f;
    [Tooltip("Tempo massimo tra i tocchi per registrare DOUBLE TAP")]
    public float sogliaTempoDoubleTap = 0.5f;
    [Tooltip("Distanza normalizzata massima per registrare DOUBLE TAP")]
    public float sogliaDistanzaDoubleTap = 0.03f;

    [Space(10)]
    [Header("Gestures")]
    [Tooltip("Se hai attivato un PAN non puoi attivare un PINCH")]
    public bool permettiSoloUnGestureAllaVolta = false;
    [Tooltip("Delta/distanza normalizzata per registrare PINCH")]
    public float sogliaDistanzaPinch = 0.03f;
    [Tooltip("Delta/distanza normalizzata per registrare PAN")]
    public float sogliaDistanzaPan = 0.02f;
    [Tooltip("Delta/Distanza normalizzata massima tra due tocchi per registrare PAN")]
    public float sogliaDistanzaTocchiPan = 1f;
    [Tooltip("Angolo minimo per registrare un ROTATE [in gradi]")]
    public float sogliaRotazione = 4f;


    [Space(10)]




    [Header("Debug")]
    [Tooltip("Stampa il log? Toglilo in produzione!")]
    public bool debug_log = false;
    [Tooltip("Raggio pallino gizmo")]
    public float debug_grandezzaGizmo = 0.1f;

    [Space(10)]
    public List<Tocco> listaTocchi = new List<Tocco>();
    public List<Gesture> listaGestures = new List<Gesture>();
    public List<IstanzaTap> listaIstanzeTap = new List<IstanzaTap>();
    Gesture gestureAttiva = null;

    [Header("Callback")]
    public EventiTocco eventiTocco;

    [Header("Callback Gestures")]
    public EventiGesture eventiGestures;



    //Private
    [HideInInspector]
    public float deltaTime;
    [HideInInspector]
    public Transform myTrans;
    [HideInInspector]
    public bool is2dCollider = false;
    // [HideInInspector]
    // float nearClipPlane;
    [HideInInspector]
    public EventSystem eventSystem;


    // PROPERTIES

    ///<summary> Ritorna il numero di tocchi su questo Touchable. SOLO quelli isOnGameObject! </summary>
    public int numeroTocchi { get { int tocchi = 0; for (int i = 0; i < listaTocchi.Count; i++) if (listaTocchi[i].isOnGameObject) tocchi++; return tocchi; } }
    ///<summary> Ritorna il numero di gesture su questo Touchable.</summary>
    public int numeroGestures { get { return listaGestures.Count; } }


    #region [.] MenuItem

#if UNITY_EDITOR
    ///<summary> Crea un touchable 3D già impostato! </summary>
    [MenuItem("GameObject/JindoBlu/Touchable", false, 15)]
    static void MenuContestualeCreaTouchable(MenuCommand menuCommand)
    {

        // Crea un piano e gli assegna il touchable
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.name = "TriggerTouchable";
        GameObjectUtility.SetParentAndAlign(quad, menuCommand.context as GameObject);
        Touchable touchable = quad.AddComponent<Touchable>();
        DestroyImmediate(quad.GetComponent<MeshRenderer>());
        DestroyImmediate(quad.GetComponent<MeshFilter>());

        // if (JindoFramework.LayerEsiste("Trigger"))
        //     quad.layer = LayerMask.NameToLayer("Trigger");

        touchable.layerMask = 1 << quad.layer;
        quad.transform.localScale = Vector3.one * 25f;





        // Registra il parent per l'UNDO
        Undo.RegisterCreatedObjectUndo(quad, "Create object");
        Selection.activeObject = quad;
    }
#endif

    #endregion


    void Start()
    {
        PrendiReference();
    }

    void PrendiReference()
    {
        if (telecamera == null)
            telecamera = Camera.main;
        if (myTrans == null)
            myTrans = transform;
        if (eventSystem == null)
            eventSystem = EventSystem.current;

        if (GetComponent<Collider2D>() != null)
            is2dCollider = true;
    }

    void Update()
    {
        if (runOnFixedUpdate == false)
            UpdateLoop();

    }


    void FixedUpdate()
    {
        if (runOnFixedUpdate == true)
            UpdateLoop();
    }

    void UpdateLoop()
    {
        deltaTime = Time.unscaledDeltaTime;

        // Se è un touchable solo per CPU o Tutorial, non permettiamo di creare tocchi dall'input
        if (permettiSoloTocchiVirtuali == false)
        {
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began)
                    {
                        CreaTocco(touch.fingerId, Tocco.TipoTocco.Touch);
                    }
                    if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    {
                        DistruggiTocco(TrovaToccoConFingerId(touch.fingerId));
                    }
                }
            }
            else
            {
                // Controlla click SX, click DX, click WHEEL
                for (int i = 0; i <= 2; i++)
                {
                    if (Input.GetMouseButtonDown(i))
                    {
                        CreaTocco(-i - 1, Tocco.TipoTocco.Mouse);
                    }

                    if (Input.GetMouseButtonUp(i))
                    {
                        DistruggiTocco(TrovaToccoConFingerId(-i - 1));
                    }
                }
            }
        }


        //UPDATE===============================


        for (int i = 0; i < listaTocchi.Count; i++)
        {
            Tocco tocco = listaTocchi[i];

            tocco.Update();

        }

        if (listaGestures.Count > 0)
            if (gestureAttiva != null)
                gestureAttiva.Update();

        for (int i = 0; i < listaIstanzeTap.Count; i++)
        {
            listaIstanzeTap[i].Update();
        }
    }

    void CreaGesture(Tocco tocco_1, Tocco tocco_2)
    {
        gestureAttiva = new Gesture(this, tocco_1, tocco_2);
        listaGestures.Add(gestureAttiva);
        DebugPrint("Gesture creata!");

    }


    ///<summary> Distrugge la Gesture attiva su questo touchable (un touchable gestisce solo una gesture) chiamando l'evento OnEndGesture </summary>
    void DistruggiGesture()
    {
        if (gestureAttiva == null)
            return;
        gestureAttiva.Distruggi();
        listaGestures.Remove(gestureAttiva);
    }

    ///<summary> Cancella la Gesture attiva su questo touchable (un touchable gestisce solo una gesture) senza chiamare l'evento OnEndGesture </summary>
    void CancellaGesture()
    {
        if (gestureAttiva == null)
            return;
        gestureAttiva.Cancella();
        listaGestures.Remove(gestureAttiva);
    }


    Tocco CreaTocco(int fingerId, Tocco.TipoTocco tipoTocco, Vector3 posizioneVirtualeIniziale = default(Vector3))
    {

        if (listaTocchi.Count >= tocchiMassimiConsentiti)
            return null;

        // Se esiste già un tocco con quell'ID, distruggilo (probabilmente è il bug del tocco che non scompare se fai slide sul bordo del display)
        Tocco toccoBuggato = TrovaToccoConFingerId(fingerId);
        if(toccoBuggato!=null)
        DistruggiTocco(toccoBuggato);

        Tocco tocco = new Tocco(this, fingerId, tipoTocco, posizioneVirtualeIniziale);

        listaTocchi.Add(tocco);

        //if (tocchiMassimiConsentiti >= 2)
        if (listaTocchi.Count == 2)
        {
            CreaGesture(listaTocchi[0], listaTocchi[1]);
        }

        return tocco;

    }

    ///<summary> Metodo esposto per creare tocchi virtuali da altri script. Ritorna il tocco creato </summary>
    ///<param name="posizioneVirtualeIniziale">La posizione in PIXEL dove vogliamo creare il tocco virtuale</param>
    ///<param name="usaPosizioneWorld"> Se TRUE al posto della posizione in PIXEL usiamo la posizione WORLD</param>
    public Tocco CreaToccoVirtuale(Vector3 posizioneVirtualePixelIniziale, bool usaPosizioneWorld = false)
    {
        PrendiReference();
        return CreaTocco(TrovaFingerIdPerToccoVirtuale(), Tocco.TipoTocco.Virtuale, usaPosizioneWorld ? telecamera.WorldToScreenPoint(posizioneVirtualePixelIniziale) : posizioneVirtualePixelIniziale);
    }

    ///<summary> Ritorna il primo id disponibile da usare per un nuovo tocco virtuale </summary>
    int TrovaFingerIdPerToccoVirtuale()
    {
        // I tocchi virtuali vanno da -11 a -infinity
        int indice = -11;
        int tocchiVirtualiMassimi = 1000;

        do
        {
            if (TrovaToccoConFingerId(indice) == null)
                return indice;
            indice--;
        } while (indice >= -tocchiVirtualiMassimi);
        return -tocchiVirtualiMassimi;
    }


    ///<summary> Distrugge il tocco chiamando tutti gli eventi collegato </summary>
    public void DistruggiTocco(Tocco tocco)
    {
        if (tocco != null)
        {
            tocco.Distruggi();
            listaTocchi.Remove(tocco);

            if (listaGestures.Count > 0)
                if (listaTocchi.Count < 2)
                    DistruggiGesture();
        }
    }

    ///<summary> Distrugge tutti i tocchi presenti sul Touchable (chiamando gli eventi collegati) </summary>
    public void DistruggiTuttiTocchi()
    {
        for (int i = listaTocchi.Count - 1; i >= 0; i--)
            DistruggiTocco(listaTocchi[i]);
    }

    ///<summary> Cancella il tocco senza chiamare gli eventi collegati </summary>
    public void CancellaTocco(Tocco tocco)
    {
        if (tocco != null)
        {
            tocco.Cancella();
            listaTocchi.Remove(tocco);

            if (listaGestures.Count > 0)
                if (listaTocchi.Count < 2)
                    DistruggiGesture();
        }
    }

    ///<summary> Cancella tutti i tocchi presenti sul Touchable (senza chiamare gli eventi collegati) </summary>
    public void CancellaTuttiTocchi()
    {
        for (int i = listaTocchi.Count - 1; i >= 0; i--)
            CancellaTocco(listaTocchi[i]);
    }




    void OnDisable()
    {
        // Cancelliamo tutti i tocchi...
        CancellaTuttiTocchi();
        // ... la gesture attiva...
        CancellaGesture();
        // ... e i tap
        for (int i = listaIstanzeTap.Count - 1; i >= 0; i--)
        {
            DistruggiIstanzaTap(listaIstanzeTap[i]);
        }
    }





    public Tocco TrovaToccoConFingerId(int fingerId)
    {
        for (int i = 0; i < listaTocchi.Count; i++)
        {
            if (listaTocchi[i].fingerId == fingerId)
            {
                return listaTocchi[i];
            }
        }

        return null;
    }

    public void CreaIstanzaTap(IstanzaTap tap)
    {
        listaIstanzeTap.Add(tap);
    }

    public void DistruggiIstanzaTap(IstanzaTap tap)
    {
        if (listaIstanzeTap.Contains(tap))
            listaIstanzeTap.Remove(tap);
    }





#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (listaTocchi.Count > 0)
            for (int i = 0; i < listaTocchi.Count; i++)
            {
                switch (listaTocchi[i].fingerId)
                {
                    case 0: Gizmos.color = Color.red; break;
                    case 1: Gizmos.color = Color.blue; break;
                    case 2: Gizmos.color = Color.yellow; break;
                    case 3: Gizmos.color = Color.green; break;
                    case 4: Gizmos.color = Color.white; break;
                    case 5: Gizmos.color = Color.black; break;
                    default: Gizmos.color = Color.red; break;
                }
                if (listaTocchi[i].isOnGameObject && !listaTocchi[i].isOverUI)
                    Gizmos.DrawSphere(listaTocchi[i].posizioneWorld, debug_grandezzaGizmo);

            }



    }

    void OnDrawGizmosSelected()
    {
        if (telecamera != null)
        {
            Gizmos.DrawLine(telecamera.transform.position, telecamera.transform.position + telecamera.transform.forward * lunghezzaRaycast);
        }
    }
#endif

#if UNITY_EDITOR
    void Reset()
    {
        layerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));
        telecamera = Camera.main;

    }

    void OnValidate()
    {
        layerMask = LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer));

        // SE usiamo questo Touchable per le gesture, non possiamo accettare più di 2 dita
        if (eventiGestures != null)
            if (eventiGestures.onGestureBegin?.GetPersistentEventCount() > 0 || eventiGestures.onGestureKeep?.GetPersistentEventCount() > 0 || eventiGestures.onPinchBegin?.GetPersistentEventCount() > 0 || eventiGestures.onPinchChanged?.GetPersistentEventCount() > 0 || eventiGestures.onPanBegin?.GetPersistentEventCount() > 0 || eventiGestures.onPanChanged?.GetPersistentEventCount() > 0 || eventiGestures.onRotateBegin?.GetPersistentEventCount() > 0 || eventiGestures.onRotateChanged?.GetPersistentEventCount() > 0 || eventiGestures.onGestureEnd?.GetPersistentEventCount() > 0)
            {
                tocchiMassimiConsentiti = 2;
            }
    }
#endif

    public bool CheckMouseInput()
    {
        return Input.mousePresent;
    }

    public void DebugPrint(object mess)
    {
        if (debug_log)
            Debug.Log(mess);
    }


}