// using UnityEngine;

// public class GameManager : MonoBehaviour
// {
//     // Make sure there's only one instance of GameManager
//     public static GameManager instance;

//     private void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     // Start is called before the first frame update
//     void Start()
//     {
//         LoadPlayerAbilities();
//     }

//     // Update the player's abilities based on saved state
//     void LoadPlayerAbilities()
//     {
//         GameObject player = GameObject.FindGameObjectWithTag("Player");
//         if (player != null)
//         {
//             PlayerMovement pm = player.GetComponent<PlayerMovement>();
//             PlayerAttack pa = player.GetComponent<PlayerAttack>();
//             PlayerSummon ps = player.GetComponent<PlayerSummon>();
//             PlayerReveal pr = player.GetComponent<PlayerReveal>();

//             // Check PlayerPrefs for each ability
//             if (PlayerPrefs.GetInt("DashLearned", 0) == 1)
//             {
//                 pm.learnDash();
//             }
//             if (PlayerPrefs.GetInt("AttackLearned", 0) == 1)
//             {
//                 pa.learnAttack();
//             }
//             if (PlayerPrefs.GetInt("SummonLearned", 0) == 1)
//             {
//                 ps.learnSummon();
//             }
//             if (PlayerPrefs.GetInt("RevealLearned", 0) == 1)
//             {
//                 pr.learnReveal();
//             }
//         }
//     }

//     // Call this method when an ability is learned
//     public void LearnAbility(string abilityId)
//     {
//         switch (abilityId)
//         {
//             case "dash":
//                 PlayerPrefs.SetInt("DashLearned", 1);
//                 break;
//             case "attack":
//                 PlayerPrefs.SetInt("AttackLearned", 1);
//                 break;
//             case "summon":
//                 PlayerPrefs.SetInt("SummonLearned", 1);
//                 break;
//             case "reveal":
//                 PlayerPrefs.SetInt("RevealLearned", 1);
//                 break;
//         }
//         PlayerPrefs.Save();
//     }
// }

// using UnityEngine;

// public class GameManager : MonoBehaviour
// {
//     // Make sure there's only one instance of GameManager
//     public static GameManager instance;

//     // Declare boolean variables for abilities
//     private bool hasDash = false;
//     private bool hasAttack = false;
//     private bool hasSummon = false;
//     private bool hasReveal = false;

//     private void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//         }
//     }

//     // Start is called before the first frame update
//     void Start()
//     {
//         LoadPlayerAbilities();
//     }

//     // Update the player's abilities based on the current state
//     void LoadPlayerAbilities()
//     {
//         GameObject player = GameObject.FindGameObjectWithTag("Player");
//         Debug.Log("made it");
//         if (player != null)
//         {

//             Debug.Log("did it");
//             PlayerMovement pm = player.GetComponent<PlayerMovement>();
//             PlayerAttack pa = player.GetComponent<PlayerAttack>();
//             PlayerSummon ps = player.GetComponent<PlayerSummon>();
//             PlayerReveal pr = player.GetComponent<PlayerReveal>();

//             // Check the boolean flags for each ability
//             if (hasDash)
//             {
//                 pm.learnDash();
//             }
//             if (hasAttack)
//             {
//                 pa.learnAttack();
//             }
//             if (hasSummon)
//             {
//                 ps.learnSummon();
//             }
//             if (hasReveal)
//             {
//                 pr.learnReveal();
//             }
//         }
//     }

//     // Call this method when an ability is learned
//     public void LearnAbility(string abilityId)
//     {
//         switch (abilityId)
//         {
//             case "dash":
//                 hasDash = true;
//                 break;
//             case "attack":
//                 hasAttack = true;
//                 break;
//             case "summon":
//                 hasSummon = true;
//                 break;
//             case "reveal":
//                 hasReveal = true;
//                 break;
//         }
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement; // Import this namespace

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private bool hasDash = false;
    private bool hasAttack = false;
    private bool hasSummon = false;
    private bool hasReveal = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Subscribe to the sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // This method is called every time a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadPlayerAbilities(); // Call LoadPlayerAbilities when a new scene is loaded
    }

    void LoadPlayerAbilities()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            PlayerAttack pa = player.GetComponent<PlayerAttack>();
            PlayerSummon ps = player.GetComponent<PlayerSummon>();
            PlayerReveal pr = player.GetComponent<PlayerReveal>();

            if (hasDash) { pm.learnDash(); }
            if (hasAttack) { pa.learnAttack(); }
            if (hasSummon) { ps.learnSummon(); }
            if (hasReveal) { pr.learnReveal(); }
        }
    }

    public void LearnAbility(string abilityId)
    {
        switch (abilityId)
        {
            case "dash": hasDash = true; break;
            case "attack": hasAttack = true; break;
            case "summon": hasSummon = true; break;
            case "reveal": hasReveal = true; break;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
