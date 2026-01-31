using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundary;
    public Transform teleportPoint;
    public Transform playerTP;
    public CinemachineFollow cameraTP;
    private CinemachineConfiner2D confiner;

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("This script is running");
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundary;
            confiner.InvalidateBoundingShapeCache();
            Debug.Log("In tp range");
            playerTP.position = teleportPoint.position;
            cameraTP.ForceCameraPosition(playerTP.position, playerTP.rotation);
        }
    }
}