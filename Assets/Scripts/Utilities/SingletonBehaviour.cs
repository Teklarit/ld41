using UnityEngine;

namespace TheHeartbeat.Utilities
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                return _instance;
            }
        }

        private void Awake()
        {
            _instance = (T) this;
            OnAwake();
        }

        protected virtual void OnAwake()
        {

        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}