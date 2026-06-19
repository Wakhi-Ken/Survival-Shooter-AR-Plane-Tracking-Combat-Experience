using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlanePlacement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    public GameObject gameWorld; // Parent object for enemies/spawner

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool gamePlaced = false;

    void Update()
    {
        if (gamePlaced)
            return;

        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        if (raycastManager.Raycast(
            touch.position,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            gameWorld.transform.position = hitPose.position;
            gameWorld.SetActive(true);

            gamePlaced = true;

            LockPlane();
        }
    }

    void LockPlane()
    {
        planeManager.enabled = false;

        foreach (ARPlane plane in planeManager.trackables)
        {
            if (plane.gameObject != hits[0].trackable.gameObject)
            {
                plane.gameObject.SetActive(false);
            }
        }
    }
}