using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineDrawing : MonoBehaviour
{
    [SerializeField] private LayerMask _LayerMask;
    private ObjectPooler objectPoller;
    private LineRenderer lineRend;  // Referance to Current Line Renderer (Can Hold different Line Renderer if one or more line has been Created)
    private bool isLineStarted = false;
    private GameObject StartPoint;

    public static LineDrawing Instance;

    public delegate void ItemMatched(GameObject AGameObject);
    public static ItemMatched OnItemMatched;

    public delegate void ItemMismatched(GameObject AGameObject);
    public static ItemMismatched OnItemMismatched;

    public delegate void ItemClicked(GameObject AGameObject);
    public static ItemMatched OnItemClicked;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        objectPoller = ObjectPooler.Instance;
    }

    private void UpdateLineBeingUsed()
    {
        Vector3 WorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        WorldPoint.z = 10;
        lineRend.SetPosition(1, WorldPoint);
    }

    void Update()
    {
        // Used when a line has started but not Finished
        if(isLineStarted)
        {
            UpdateLineBeingUsed();
        }


#if UNITY_EDITOR || true


        // When Button is Pressed
        if (Input.GetMouseButtonDown(0))
        {
            ClickDownRayCastFor3DObjects();

            ClickDownCheckFor2DObjects();

            #region UI Click down and Click up Implimentation
            //ClickDownCheckForUIElements(); 
            #endregion
        }


        // When Button is Released
        if(Input.GetMouseButtonUp(0))
        {
            // Check if button was pressed down (when Starting the Line) on an intractable Object
            if(StartPoint)
            {
                ClickUpCheckFor2DObjets();

                Debug.LogWarning("Click Up Check For 3D Object Not Implimented");
            }
        }

#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            //RaycastHit hit;

            //if (Physics.Raycast(ray, out hit))
            //{
            //    if (hit.collider != null)
            //    {
            //        Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
            //        hit.collider.GetComponent<MeshRenderer>().material.color = newColor;
            //    }
            //}

            ClickDownRayCastFor3DObjects();

            ClickDownCheckFor2DObjects();
        }


        if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            // Check if button was pressed down (when Starting the Line) on an intractable Object
            if(StartPoint)
            {
                ClickUpCheckFor2DObjets();

                Debug.LogWarning("Click Up Check For 3D Object Not Implimented");
            }
        }

#endif
    }

    private void ClickDownCheckFor2DObjects()
    {
        Ray rayFor2D = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 orgin = new Vector2(rayFor2D.origin.x, rayFor2D.origin.y);
        RaycastHit2D hitFor2D = Physics2D.Raycast(orgin, -Vector2.up, Mathf.Infinity, _LayerMask);
        if (hitFor2D.collider != null)
        {
            ClickDownCheckForObjects(hitFor2D.collider.gameObject);
        }
    }

    private void ClickDownRayCastFor3DObjects()
    {
        Ray rayFor3D = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitFor3D;
        if (Physics.Raycast(rayFor3D, out hitFor3D, Mathf.Infinity, _LayerMask))
        {
            ClickDownCheckForObjects(hitFor3D.collider.gameObject);
        }
    }

    
    private void ClickDownCheckForObjects(GameObject ItemHitted)
    {

        Interactable interactable = ItemHitted.gameObject.GetComponent<Interactable>();
        if (!interactable)
        {
            print("Object Doesn't have Intractable Script Attached");
            return;
        }

        // Call the event when Player presses on the interactable object
        if (interactable.isMoving)
        {
            OnItemClicked?.Invoke(ItemHitted);
            return;
        }


        if (interactable.isConnected == false)
        {
            StartPoint = ItemHitted;
            SpawnALineStartingAtPoint(new Vector3(StartPoint.transform.position.x, StartPoint.transform.position.y, 10));
            isLineStarted = true;
        }
    }

    private void ClickUpCheckFor2DObjets()
    {
        Ray rayFor2D = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 orgin = new Vector2(rayFor2D.origin.x, rayFor2D.origin.y);
        RaycastHit2D hitFor2D = Physics2D.Raycast(orgin, -Vector2.up, Mathf.Infinity, _LayerMask);

        if (hitFor2D.collider == null)
        {
            Debug.Log("No interactable Object at Pointer up position. Item Clicked.");

            ResetLocalParameters();
            AddLineRendererBackToPool();

            return;
        }

        // If we release button on the same object as the starting object
        if (hitFor2D.collider.gameObject == StartPoint)
        {
#if SHOW_DETAIL_LOGS
            Debug.Log("Start and End Object are Same. Item Clicked."); 
#endif

            ResetLocalParameters();
            AddLineRendererBackToPool();

            return;
        }

        // If Items Mismatched
        if (hitFor2D.collider.gameObject.name != StartPoint.name)
        {
            Debug.Log("Items Mismatched");

            ResetLocalParameters();
            AddLineRendererBackToPool();

            OnItemMismatched?.Invoke(hitFor2D.collider.gameObject);
            return;
        }

        // make sure that intractable Object has Intractable Script attached to it
        Interactable interactable = hitFor2D.collider.gameObject.GetComponent<Interactable>();
        if (!interactable)
        {
            Debug.Log("Object Doesn't have Intractable Script Attached");

            ResetLocalParameters();
            AddLineRendererBackToPool();

            return;
        }

        if (interactable.isConnected)    // if connected previously just do not connect starting Object with it and erase the line
        {
            Debug.Log("The Item is previously Connected When Realeasing");
            ResetLocalParameters();
            AddLineRendererBackToPool();
            return;
        }


        // Connect the line to both the starting point and ending point and change the color
        isLineStarted = false;
        lineRend.positionCount = 3;
        Vector3 EndPoint = hitFor2D.collider.gameObject.transform.position;
        Vector3 midPoint = new Vector3((StartPoint.transform.position.x + EndPoint.x) / 2, (StartPoint.transform.position.y + EndPoint.y) / 2, 10);
        lineRend.SetPosition(1, midPoint);
        lineRend.SetPosition(2, new Vector3(EndPoint.x, EndPoint.y, 10));
        hitFor2D.collider.gameObject.GetComponent<Interactable>().isConnected = true;
        StartPoint.GetComponent<Interactable>().isConnected = true;



        // A simple 2 color gradient with a fixed alpha of 1.0f.
        // As line is connected Change Line Color to Green
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color32(128, 255, 128, 1), 0.0f),
                                                new GradientColorKey(new Color32(0, 128, 0, 1), 0.5f),
                                                new GradientColorKey(new Color32(128, 255, 128, 1), 1.0f)},
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRend.colorGradient = gradient;
        lineRenderersUsed.Add( lineRend );

        ResetLocalParameters();

        OnItemMatched?.Invoke(hitFor2D.collider.gameObject);
    }

    private void SpawnALineStartingAtPoint(Vector3 StartingPoint)
    {
        lineRend = objectPoller.SpawnFromPool("Line Renderer", transform.position, Quaternion.identity).GetComponent<LineRenderer>();
        lineRend.positionCount = 2;
        lineRend.SetPosition(0, StartingPoint);
        Vector3 WorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        WorldPoint.z = 10;
        lineRend.SetPosition(1, WorldPoint);
    }

    private void AddLineRendererBackToPool()
    {
        if(lineRend == null)
        {
            Debug.LogError("No Active Line Renderer to Add to pool");
            return;
        }
        lineRend.positionCount = 0;
        objectPoller.poolDictionary["Line Renderer"].Enqueue(lineRend.gameObject);
        lineRend.gameObject.SetActive(false);
        lineRend = null;
    }

    List <LineRenderer> lineRenderersUsed = new List<LineRenderer>();

    private void ResetLocalParameters()
    {
        StartPoint = null;
        isLineStarted = false;
    }

    public void ClearUsedLineRendrers()
    {
        if(lineRenderersUsed.Count == 0)        return;

        for (int i = lineRenderersUsed.Count - 1; i >= 0;  i--)
        {
            Destroy(lineRenderersUsed[i].gameObject);
            lineRenderersUsed.RemoveAt(i);
        }
    }


    #region UI Click down and Click up Implimentation
    // Create a new PointerEventData
    //private PointerEventData eventData = new PointerEventData(EventSystem.current);

    //// Create a list to store the results of the raycast
    //private List<RaycastResult> results = new List<RaycastResult>();

    //private void ClickDownCheckForUIElements()
    //{
    //    results.Clear();
    //    // Set the position of the PointerEventData to the mouse position
    //    eventData.position = Input.mousePosition;

    //    // Raycast against all graphics using the GraphicRaycaster
    //    EventSystem.current.RaycastAll(eventData, results);

    //    // Iterate through the results and handle the UI elements
    //    foreach (RaycastResult result in results)
    //    {
    //        Debug.Log("Hit UI Element: " + result.gameObject.name);
    //        if (result.gameObject.tag != "Interactables") continue;
    //        // Perform actions or retrieve information based on the hitObject
    //        ClickDownCheckForObjects(result.gameObject);
    //        Debug.Log("UI Element Pressed");
    //        return;
    //    }
    //}

    #endregion
}
