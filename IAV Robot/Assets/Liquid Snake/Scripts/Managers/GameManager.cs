using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private int timesDetected = 0;

    public int TimesDetected
    {
        get { return timesDetected; }
        set { timesDetected = value; if (timesDetected < 0) timesDetected = 0; }
    }
}
