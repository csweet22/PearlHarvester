using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Navigator3D : MonoBehaviour
{
    private Dictionary<Vector3Int, Sample> _grid = new Dictionary<Vector3Int, Sample>();

    private class Sample
    {
        public Vector3Int GridPosition;
        public Vector3 WorldPosition;
        public float DistanceToDest = Mathf.Infinity;
        public Sample Up = null;
        public Sample Down = null;
        public Sample Left = null;
        public Sample Right = null;
        public Sample Forward = null;
        public Sample Back = null;

        public Sample(Vector3Int gridPosition, Vector3 worldPosition)
        {
            GridPosition = gridPosition;
            WorldPosition = worldPosition;
        }
    }

    // 8 by 8 by 8 grid
    [SerializeField] private int resolution = 8;

    // Amount of space between samples
    [SerializeField] private float scale = 1f;

    [SerializeField] private int updateFrequency = 5;

    [SerializeField] private Transform src;
    [SerializeField] private Transform dest;

    public int tick = 0;

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        _grid.Clear();

        int halfResolution = resolution / 2;

        for (int x = -halfResolution; x < halfResolution; x++){
            for (int y = -halfResolution; y < halfResolution; y++){
                for (int z = -halfResolution; z < halfResolution; z++){
                    Vector3Int gridPos = new Vector3Int(x, y, z);
                    Vector3 worldPos = transform.position + ((Vector3) gridPos * scale);
                    Sample newSample = new Sample(gridPos, worldPos);

                    _grid.Add(gridPos, newSample);
                }
            }
        }

        for (int x = -halfResolution; x < halfResolution; x++){
            for (int y = -halfResolution; y < halfResolution; y++){
                for (int z = -halfResolution; z < halfResolution; z++){
                    Vector3Int gridPos = new Vector3Int(x, y, z);
                    Sample sample = _grid[gridPos];

                    if (gridPos.y + 1 < halfResolution){
                        sample.Up = _grid[gridPos + Vector3Int.up];
                    }

                    if (gridPos.y - 1 > -halfResolution){
                        sample.Down = _grid[gridPos + Vector3Int.down];
                    }

                    if (gridPos.x + 1 < halfResolution){
                        sample.Right = _grid[gridPos + Vector3Int.right];
                    }

                    if (gridPos.x - 1 > -halfResolution){
                        sample.Left = _grid[gridPos + Vector3Int.left];
                    }

                    if (gridPos.z + 1 < halfResolution){
                        sample.Forward = _grid[gridPos + Vector3Int.forward];
                    }

                    if (gridPos.z - 1 > -halfResolution){
                        sample.Back = _grid[gridPos + Vector3Int.back];
                    }

                    _grid[gridPos] = sample;
                }
            }
        }
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        tick++;
        if (tick >= updateFrequency){
            tick = 0;
            RecalculatePath();
        }
    }

    [ContextMenu("Recalculate Path")]
    private void RecalculatePath()
    {
        if (!src || !dest) return;

        Stack<Vector3Int> path = new Stack<Vector3Int>();
        
        Sample closestToTarget;
        float closestDistance = Mathf.Infinity;

        foreach (KeyValuePair<Vector3Int, Sample> kvp in _grid){
            kvp.Value.DistanceToDest = Vector3.Distance(kvp.Value.WorldPosition, dest.position);
            if (kvp.Value.DistanceToDest < closestDistance){
                closestDistance = kvp.Value.DistanceToDest;
                closestToTarget = kvp.Value;
            }

            Debug.Log(closestDistance);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (KeyValuePair<Vector3Int, Sample> kvp in _grid){
            Sample sample = kvp.Value;
            Gizmos.color = Color.Lerp(Color.red, Color.green, sample.DistanceToDest / (scale * resolution / 2));
            Gizmos.DrawSphere(sample.WorldPosition, 0.4f);
        }
    }
}