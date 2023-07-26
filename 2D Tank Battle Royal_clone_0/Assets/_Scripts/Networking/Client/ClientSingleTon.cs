using System.Threading.Tasks;
using UnityEngine;

public class ClientSingleTon : MonoBehaviour
{
    private static ClientSingleTon instance;

    public ClientGameManager GameManager { get; private set; }

    public static ClientSingleTon Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<ClientSingleTon>();

            if (instance == null) return null;

            return instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public async Task<bool> CreateClientAsync()
    {
        GameManager = new ClientGameManager();

        return await GameManager.InitAsync();
    }
}
