using UnityEngine;

namespace DSFramework {

    public class DSingleMono<T> : MonoBehaviour where T: MonoBehaviour {
        private static T _instance;

        public static T DSInstance {
            get{
                if (_instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    DontDestroyOnLoad(obj);
                    _instance = obj.AddComponent<T>();
                    
                }
                return _instance;
            }
        }

    }

}
