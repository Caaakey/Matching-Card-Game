using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool isQuit = false;
    private static readonly object _lock = new object();

    public void OnDestroy() { isQuit = true; }

    public static T Get
    {
        get
        {
            if (isQuit) return null;
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));
                    if (FindObjectsOfType(typeof(T)).Length > 1) return instance;
                    if (instance == null)
                    {
                        GameObject g = new GameObject();
                        instance = g.AddComponent<T>();
                        g.name = typeof(T).Name;

                        DontDestroyOnLoad(g);
                    }
                }

                return instance;
            }
        }
    }

}
