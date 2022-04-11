using UnityEngine;

namespace WRStudio
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    GameObject obj = GameObject.Find(typeof(T).Name);

                    if (obj)
                    {
                        m_instance = obj.GetComponent<T>();
                    }
                }

                if (m_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    m_instance = obj.AddComponent<T>();
                    //(m_instance as Singleton<T>).OnCreated();
                }

                return m_instance;
            }
        }

        private static T m_instance;

        public static void Init()
        {
            Instance.OnCreated();
        }

        protected virtual void OnCreated()
        {
            DontDestroyOnLoad(this);
            Debug.Log($"{typeof(T).Name} Singleton Instance Created ! ");
        }
    }
}