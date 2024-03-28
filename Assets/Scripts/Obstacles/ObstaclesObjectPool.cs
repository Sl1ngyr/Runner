using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obstacles
{
    public class ObstaclesObjectPool<T> where T : MonoBehaviour
    {
        private T _prefab;
        private List<T> _objects;

        public ObstaclesObjectPool(T prefab, int poolCout)
        {
            _prefab = prefab;
            _objects = new List<T>();

            for (int i = 0; i < poolCout; i++)
            {
                var obj = GameObject.Instantiate(_prefab);
                obj.gameObject.SetActive(false);
                _objects.Add(obj);
            }
        }

        public T Get(Vector3 position)
        {
            var obj = _objects.FirstOrDefault(x => !x.isActiveAndEnabled);

            if (obj == null)
            {
                obj = Create();
            }

            obj.transform.position = position;
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Release(T obj)
        {
            obj.gameObject.SetActive(false);
        }

        public void DeactivateAllObject()
        {
            foreach (var obj in _objects)
            {
                Release(obj);
            }
        }
        
        private T Create()
        {
            var obj = GameObject.Instantiate(_prefab);
            _objects.Add(obj);
            return obj;
        }
    }
}