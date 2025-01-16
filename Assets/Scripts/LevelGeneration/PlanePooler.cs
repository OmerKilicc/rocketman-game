using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using SOs.Variables;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

namespace LevelGeneration
{
    public class PlanePooler : MonoBehaviour
    {
        private IObjectPool<PlanePopulator> _objectPool;

        [Header("Pool Settings")]
        [SerializeField] private int initialPlaneCount = 2;
        [SerializeField] private int defaultSize = 20;
        [SerializeField] private int maxSize = 100;
        [SerializeField] private bool collectionCheck = true;
        [SerializeField] private PlanePopulator planePrefab;

        [Header("Position References")]
        [SerializeField] private FloatVariable playerZPosition;
        [SerializeField] private Vector2Variable firstPositionToPlace;
        [SerializeField] private Vector2Variable regionSize;

        private List<PlanePopulator> _activePlanes = new List<PlanePopulator>();
        private float _positionZOffset;

        private void Awake()
        {
            _objectPool = new ObjectPool<PlanePopulator>(CreatePlane, 
                OnGetFromPool, OnReleaseToPool, OnDestroyObject,
                collectionCheck, defaultSize, maxSize);
        }

        private void Start()
        {
            _positionZOffset = regionSize.Value.y * 10;
            firstPositionToPlace.Value.x = -regionSize.Value.x * 5;
            
            for (int i = 0; i < initialPlaneCount; i++)
            {
                PlanePopulator plane = _objectPool.Get();
                _activePlanes.Add(plane);
            }
            
            _activePlanes[0].transform.position = firstPositionToPlace.Value;

            for (int i = 0; i < _activePlanes.Count; i++)
            {
                _activePlanes[i].transform.position = new Vector3(firstPositionToPlace.Value.x,
                    firstPositionToPlace.Value.y,
                    _positionZOffset * i);
            }
        }

        public void PlaceNewPlane()
        {
            PlanePopulator newPlane = _objectPool.Get();
            
            newPlane.transform.position = new Vector3(firstPositionToPlace.Value.x,
                firstPositionToPlace.Value.y,
                _positionZOffset + _activePlanes[_activePlanes.Count - 1].transform.position.z);

            _activePlanes.Add(newPlane);
            _activePlanes[0].Deactivate();
            _activePlanes.RemoveAt(0);
        }
        
        // invoked when creating an item to populate the object pool
        private PlanePopulator CreatePlane()
        {
            PlanePopulator planeInstance = Instantiate(planePrefab);
            planeInstance.ObjectPool = _objectPool;
            return planeInstance;
        }
        private void OnReleaseToPool(PlanePopulator obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void OnGetFromPool(PlanePopulator obj)
        {
            obj.gameObject.SetActive(true);
        }
        private void OnDestroyObject(PlanePopulator obj)
        {
            Destroy(obj.gameObject);
        }
    }
}