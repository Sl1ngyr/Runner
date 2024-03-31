using System.Collections;
using System.Collections.Generic;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obstacles
{
    public class SpawnManager : MonoBehaviour
    {
        [Header("Prefabs obstacles")] 
        [SerializeField] private List<Obstacle> _prefabs;
        
        [Space] 
        [Header("Pool fields")] 
        [SerializeField] private int _poolCout;
        [SerializeField] private float _speedObstacle;
        [SerializeField] private int _minStartPositionX;
        [SerializeField] private int _maxStartPositionX;
        [SerializeField] private float _startPositionZ;
        [SerializeField] private float _increaseSpeed;
        [SerializeField] private int _timeToStartSpawn;
        [SerializeField] private int _timeToSpawnNextObstacle;
        
        private List<ObstacleObjectPool<Obstacle>> _pools;
        private Coroutine _spawnCoroutine;

        private void Awake()
        {
            InitPool();
        }

        private void StartSpawnObstacle()
        {
            _spawnCoroutine = StartCoroutine(CoroutineSpawn());
        }
        
        private IEnumerator CoroutineSpawn()
        {
            Obstacle.StopMoveObstacles = false;
            yield return new WaitForSeconds(_timeToStartSpawn);
            while (true)
            {
                TakeRandomObstacle();
                yield return new WaitForSeconds(_timeToSpawnNextObstacle);
                IncreaseSpeed();
            }
        }
        
        private void InitPool()
        {
            _pools = new List<ObstacleObjectPool<Obstacle>>();
            
            for (int i = 0; i < _prefabs.Count; i++)
            {
                _pools.Add(new ObstacleObjectPool<Obstacle>(_prefabs[i],_poolCout));
            }
        }

        private void TakeRandomObstacle()
        {
            var random = Random.Range(0, _prefabs.Count);
            _pools[random].Get(SetPosition()).Speed = _speedObstacle;
            
        }

        private Vector3 SetPosition()
        {
            var random = Random.Range(_minStartPositionX, 2);
            var position = new Vector3(random, 0, _startPositionZ);

            return position;
        }

        private void StopAllObject()
        {
            Obstacle.StopMoveObstacles = true;
            StopCoroutine(_spawnCoroutine);
        }

        private void DeactivateAllObjects()
        {
            foreach (var poolObj in _pools)
            {
                poolObj.DeactivateObjects();
            }
        }
        
        private void IncreaseSpeed()
        {
            _speedObstacle += _increaseSpeed;
        }
        
        private void OnEnable()
        {
            EventBus.Instance.onGameStarted += StartSpawnObstacle;
            EventBus.Instance.onPlayerCollideWithObstacle += StopAllObject;
            EventBus.Instance.onPlayerDead += DeactivateAllObjects;
            EventBus.Instance.onAdToReviveCompleted += StartSpawnObstacle;
        }

        private void OnDisable()
        {
            EventBus.Instance.onGameStarted -= StartSpawnObstacle;
            EventBus.Instance.onPlayerCollideWithObstacle -= StopAllObject;
            EventBus.Instance.onPlayerDead -= DeactivateAllObjects;
            EventBus.Instance.onAdToReviveCompleted -= StartSpawnObstacle;
        }
        
    }
}