using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    GameObject m_PlacedPrefab;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
    ARSessionOrigin Origin;
    private Toggle changetog;
    private List<GameObject> spawned;
    float timer;


    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        Origin= GetComponent<ARSessionOrigin>();
        changetog = GameObject.FindGameObjectWithTag("changetog").GetComponent<Toggle>();
        timer = 0;

    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {

        if (Input.touchCount > 0)
        {
            
            touchPosition = Input.GetTouch(0).position;
            
            return true;
        }

        touchPosition = default;
        return false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (!TryGetTouchPosition(out Vector2 touchPosition)||timer<3.5f)
            return;

        if (!IsPointOverUIObject(touchPosition) && m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            Vector3 tmp = Vector3.zero;
            tmp.y = Origin.camera.transform.rotation.eulerAngles.y;


            var hitPose = s_Hits[0].pose;

            if (changetog.isOn)
            {
                spawned.Add(Instantiate(m_PlacedPrefab, hitPose.position, Quaternion.Euler(tmp)));
                

            }
            else
            {
                spawned[spawned.Count-1].transform.position = hitPose.position;
                spawned[spawned.Count - 1].transform.rotation = Quaternion.Euler(tmp);

            }
            timer = 0;
        }
        
    }


    bool IsPointOverUIObject(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }
}