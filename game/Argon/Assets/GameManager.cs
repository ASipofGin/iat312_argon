using UnityEngine;
using UnityEngine.SceneManagement; // Import this namespace

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private bool hasDash = false;
    [SerializeField]
    private bool hasAttack = false;
    [SerializeField]
    private bool hasSummon = false;
    [SerializeField]
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

    private void Start(){

    }
    

    // This method is called every time a scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
 
        if (scene.name == "Aide Fight") 
        {
            Destroy(gameObject);
            return;
        }
        LoadPlayerAbilities();

    }
       

    void LoadPlayerAbilities()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        AbilitySwap abilitySwap = FindObjectOfType<AbilitySwap>(); // Find the AbilitySwap component in the scene

        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            PlayerAttack pa = player.GetComponent<PlayerAttack>();
            PlayerSummon ps = player.GetComponent<PlayerSummon>();
            PlayerReveal pr = player.GetComponent<PlayerReveal>();

            // Initialize the ability statuses in AbilitySwap based on the GameManager's flags
            abilitySwap.SetAbilityUnlocked(0, hasDash); // Assuming dash is at index 0
            abilitySwap.SetAbilityUnlocked(1, hasAttack); // Assuming attack is at index 1
            abilitySwap.SetAbilityUnlocked(2, hasSummon); // Assuming summon is at index 2
            abilitySwap.SetAbilityUnlocked(3, hasReveal); // Assuming reveal is at index 3

            if (hasDash) { pm.learnDash(); }
            if (hasAttack) { pa.learnAttack(); }
            if (hasSummon) { ps.learnSummon(); }
            if (hasReveal) { pr.learnReveal(); }
        }
    }

    public void LearnAbility(string abilityId)
    {
        AbilitySwap abilitySwap = FindObjectOfType<AbilitySwap>(); // Find the AbilitySwap component in the scene

        switch (abilityId)
        {
            case "dash":
                hasDash = true;
                abilitySwap.SetAbilityUnlocked(0, true); // Assuming dash is at index 0
                break;
            case "attack":
                hasAttack = true;
                abilitySwap.SetAbilityUnlocked(1, true); // Assuming attack is at index 1
                break;
            case "summon":
                hasSummon = true;
                abilitySwap.SetAbilityUnlocked(2, true); // Assuming summon is at index 2
                break;
            case "reveal":
                hasReveal = true;
                abilitySwap.SetAbilityUnlocked(3, true); // Assuming reveal is at index 3
                break;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public bool HasDash() { return hasDash; }
    public bool HasAttack() { return hasAttack; }
    public bool HasSummon() { return hasSummon; }
    public bool HasReveal() { return hasReveal; }
}


