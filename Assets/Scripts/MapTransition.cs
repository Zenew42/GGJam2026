using System;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D mapBoundary;
    private CinemachineConfiner2D confiner;
    /*[SerializeField] private Direction direction;
    [SerializeField] private float additivePos = 2f;

    private enum Direction
    {
        Up,
        Down,
        Left,
        Right
    } */

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundary;
            confiner.InvalidateBoundingShapeCache();
            //UpdatePlayerPosition(collision.gameObject);
        }
    }
}