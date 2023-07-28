using System.Threading.Tasks;
using UnityEngine;

public class HostSingleTon : MonoBehaviour
{
    private static HostSingleTon instance;

    public HostGameManager GameManager { get; private set; }

    public static HostSingleTon Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<HostSingleTon>();

            if (instance == null) return null;

            return instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        GameManager = new HostGameManager();
    }
}
