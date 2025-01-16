using System;
using System.Collections.Generic;
using SOs.Variables;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlanePopulator : MonoBehaviour 
{
    [Header("Algorithm Settings")]
    [SerializeField] private float cellRadius = 1;
    [SerializeField] private int rejectionSamples = 30;
    [SerializeField] private Vector2Variable regionSize;
    
    [Header("Spawned Object Settings")]
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] private float cylinderPercentage;
    [SerializeField] private GameObject cylinderPrefab;
    [SerializeField] private GameObject prismPrefab;
    
    [Header("Plane Mesh Settings")]
    [SerializeField] private GameObject planeMesh;
    
    private List<Vector2> _points;
    private GameObject _spawnedObject;

    // Make plane release itself when needed
    private IObjectPool<PlanePopulator> objectPool;
    public IObjectPool<PlanePopulator> ObjectPool {set => objectPool = value; }

    private void Start()
    {
        _points = PoissonDiscSampling.GeneratePoints(cellRadius, regionSize.Value, rejectionSamples);
        
        planeMesh.transform.localScale = new Vector3(regionSize.Value.x, 1, regionSize.Value.y);
        planeMesh.transform.localPosition = new Vector3(regionSize.Value.x * 5, 0, regionSize.Value.y * 5);

        if (_points != null)
        {
            Vector3 pointConversion = default;
            
            for (int i = 0; i < _points.Count; i++)
            {
                pointConversion.x = _points[i].x;
                pointConversion.z = _points[i].y;
                
                int cylinderAmount = Mathf.CeilToInt(_points.Count * cylinderPercentage / 100f);
                if (i < cylinderAmount)
                {
                    _spawnedObject = Instantiate(cylinderPrefab, transform.position, Quaternion.identity, transform);
                    _spawnedObject.transform.localPosition = new Vector3(_points[i].x * 10,0, _points[i].y * 10);
                }
                else
                {
                    _spawnedObject = Instantiate(prismPrefab, transform.position, Quaternion.identity,transform);
                    _spawnedObject.transform.localPosition = new Vector3(_points[i].x * 10,0, _points[i].y * 10);
                }
                
                _spawnedObject.transform.localScale = new Vector3(_spawnedObject.transform.localScale.x, 
                    Random.Range(minHeight,maxHeight), _spawnedObject.transform.localScale.z);
                    
            }
        }
    }

    public void Deactivate()
    {
        objectPool.Release(this);
    }
}