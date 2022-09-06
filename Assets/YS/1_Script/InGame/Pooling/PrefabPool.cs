using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace YS
{
    public abstract class PoolingObject
    {
        private static ulong nextID = 0;
        private ulong id;

        public ulong ID => id;

        protected PoolingObject()
        {
            id = nextID++;
        }

        public bool Release()
        {
            return ObjectPool.ReleaseObject(GetType(), this);
        }
    }

    /// <summary>
    /// PoolingObject를 상속받아야 사용 가능
    /// </summary>
    public class ObjectPool : SingleTone<ObjectPool>
    {
        private Dictionary<Type, Dictionary<ulong, PoolingObject>> availableObj = new Dictionary<Type, Dictionary<ulong, PoolingObject>>();
        private Dictionary<Type, Dictionary<ulong, PoolingObject>> unavailableObj = new Dictionary<Type, Dictionary<ulong, PoolingObject>>();

        public static void PoolObject(Type type, int count)
        {
#if UNITY_EDITOR
            Assert.IsTrue(type.IsClass && Utility.IsDerived(type, typeof(PoolingObject)));
#endif
            ObjectPool pool = Instance;

            for (int i = 0; i < count; ++i)
            {
                PoolingObject t = Activator.CreateInstance(type) as PoolingObject;
                pool.availableObj[type].Add(t.ID, t);
            }

            Debug.LogWarning("Pool " + type + " : " + count);
        }
        public static void PoolObject<T>(int count) where T : PoolingObject, new()
        {
            ObjectPool pool = Instance;

            for (int i = 0; i < count; ++i)
            {
                PoolingObject t = new T();
                pool.availableObj[typeof(T)].Add(t.ID, t);
            }

            Debug.LogWarning("Pool " + typeof(T) + " : " + count);
        }
        public static object GetObject(Type type)
        {
#if UNITY_EDITOR
            Assert.IsTrue(type.IsClass && Utility.IsDerived(type, typeof(PoolingObject)));
#endif
            var pool = Instance;
            if (!pool.availableObj.ContainsKey(type))
            {
                pool.availableObj.Add(type, new Dictionary<ulong, PoolingObject>());
                pool.unavailableObj.Add(type, new Dictionary<ulong, PoolingObject>());
            }

            // 없다면 생성?
            if (pool.availableObj[type].Count == 0)
                PoolObject(type, pool.unavailableObj[type].Count == 0 ? 1 : pool.unavailableObj[type].Count);

            var e = pool.availableObj[type].GetEnumerator();
            e.MoveNext();
            var t = e.Current.Value;

            pool.availableObj[type].Remove(t.ID);
            pool.unavailableObj[type].Add(t.ID, t);

            return t;
        }
        public static T GetObject<T>() where T : PoolingObject, new()
        {
            Type type = typeof(T);
            var pool = Instance;

            if (!pool.availableObj.ContainsKey(type))
            {
                pool.availableObj.Add(type, new Dictionary<ulong, PoolingObject>());
                pool.unavailableObj.Add(type, new Dictionary<ulong, PoolingObject>());
            }

            // 없다면 생성?
            if (pool.availableObj[type].Count == 0)
                PoolObject(type, pool.unavailableObj[type].Count == 0 ? 1 : pool.unavailableObj[type].Count);

            var e = pool.availableObj[type].GetEnumerator();
            e.MoveNext();
            var t = e.Current.Value;

            pool.availableObj[type].Remove(t.ID);
            pool.unavailableObj[type].Add(t.ID, t);

            return t as T;
        }
        public static bool ReleaseObject(Type type, PoolingObject obj)
        {
            ObjectPool pool = Instance;
#if UNITY_EDITOR
            Assert.IsTrue(pool.unavailableObj.ContainsKey(type));
#endif
            if (!Instance.unavailableObj[type].Remove(obj.ID))
                return false;

            Instance.availableObj[type].Add(obj.ID, obj);

            return true;
        }
        public static bool ReleaseObject<T>(T obj) where T : PoolingObject, new()
        {
            Type type = typeof(T);
            ObjectPool pool = Instance;
#if UNITY_EDITOR
            Assert.IsTrue(pool.unavailableObj.ContainsKey(type));
#endif
            if (!Instance.unavailableObj[type].Remove(obj.ID))
                return false;

            Instance.availableObj[type].Add(obj.ID, obj);

            return true;
        }
    }

    public class PrefabPool : SingleTone<PrefabPool>
    {
        static private Dictionary<int, Dictionary<int, GameObject>> availableObj = new Dictionary<int, Dictionary<int, GameObject>>();
        static private Dictionary<int, Dictionary<int, GameObject>> unavailableObj = new Dictionary<int, Dictionary<int, GameObject>>();

#if UNITY_EDITOR
        static private Dictionary<int, Transform> roots = new Dictionary<int, Transform>();
#else
        static private Transform root = new GameObject().transform;
#endif
        public static void PoolObject(int count, PoolingComponent prefabPC)
        {
            GameObject prefab = prefabPC.gameObject;

            prefab.SetActive(false);

            int id = prefab.GetInstanceID();

            if (!availableObj.ContainsKey(id))
            {
                availableObj.Add(id, new Dictionary<int, GameObject>());
                unavailableObj.Add(id, new Dictionary<int, GameObject>());
#if UNITY_EDITOR
                roots.Add(id, new GameObject(prefab.name).transform);
#endif
            }

#if UNITY_EDITOR
            Transform root = roots[id];
#endif

            for (int i = 0; i < count; ++i)
            {
                GameObject obj = GameObject.Instantiate(prefab, root);
                obj.SetActive(false);

                var pc = obj.GetComponent<PoolingComponent>();
                pc.prefabID = id;
                pc.OnInstantiate();
                availableObj[id].Add(obj.GetInstanceID(), obj);
            }

            Debug.LogWarning(prefab.name + ".PoolObject()");
        }

        public static GameObject GetObject(PoolingComponent prefabPC, bool active = true)
        {
            GameObject prefab = prefabPC.gameObject;

            int key = prefab.GetInstanceID();
            bool hasPooler = availableObj.ContainsKey(key);

            // 없다면 생성?
            if (hasPooler)
            {
                if (availableObj[key].Count == 0)
                    PoolObject(unavailableObj[key].Count == 0 ? 1 : unavailableObj[key].Count, prefabPC);
            }
            else PoolObject(1, prefabPC);

            var e = availableObj[key].GetEnumerator();
            e.MoveNext();

            GameObject obj = e.Current.Value;
            availableObj[key].Remove(obj.GetInstanceID());
            unavailableObj[key].Add(obj.GetInstanceID(), obj);

            obj.SetActive(active);

            return obj;
        }
        public static bool ReleaseObject(int prefabID, GameObject obj)
        {
            if (!unavailableObj[prefabID].Remove(obj.GetInstanceID()))
                return false;

#if UNITY_EDITOR
            Transform root = roots[prefabID];
#endif

            obj.SetActive(false);

            availableObj[prefabID].Add(obj.GetInstanceID(), obj);
            obj.transform.SetParent(root);

            return true;
        }
        public static void Clear()
        {
            availableObj.Clear();
            unavailableObj.Clear();
#if UNITY_EDITOR
            roots.Clear();
#endif
        }
    }
}