using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    #region Fields

    #region Singleton
    private static GameManager instance;
    public static GameManager Instance 
    {
        get 
        {
            if ( instance == null )
            {
                instance = GameObject.FindObjectOfType<GameManager>();

                DontDestroyOnLoad(instance);
            }
            return instance; 
        }
    }
    #endregion Singleton

    public GameObject currentCheckpoint;


    public static string lastEnemyHitted;
    public static string enemyBeingHitted;

    #region Health Pickup
    [SerializeField] private GameObject healthPrefab;
    public GameObject HealthPrefab
    {
        get
        {
            return healthPrefab;
        }
    }
    private float healhDropProbability = 50;
    public float HealhDropProbability 
    { 
        get
        {
            return healhDropProbability;
        }
    }
    #endregion Health Pickup

    #endregion Fields

    void Awake()
    {
        instance = this;
    }

    void Start () 
    {
        enemyBeingHitted = " ";
	}
	
    public void setEnemyBeingHitted(string name)
    {
        lastEnemyHitted = enemyBeingHitted;
        enemyBeingHitted = name;

        PlayerController.Instance.StartComboTimer();
    }

}
