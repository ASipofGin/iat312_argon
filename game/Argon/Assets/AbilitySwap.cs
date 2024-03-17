using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Include the SceneManagement namespace

public class AbilitySwap : MonoBehaviour
{
    public Ability[] abilities; // Populate this array with your abilities in the Inspector
    public Image mainAbilityIcon;
    public Image nextAbilityIcon;
    public Image previousAbilityIcon;
    public Sprite defaultIcon;
    private int currentAbilityIndex = 0; // Index of the currently selected ability
    public bool[] abilitiesUnlocked; // Track which abilities are unlocked

    [SerializeField] GameObject player;

    [SerializeField] private PlayerMovement pm;
    [SerializeField] private PlayerAttack pa;
    [SerializeField] private PlayerSummon ps;
    [SerializeField] private PlayerReveal pr;


    void Awake()
    {
        abilitiesUnlocked = new bool[abilities.Length]; // Initialize unlocked status array
        SceneManager.sceneLoaded += OnSceneLoaded; // Register to scene load event
        FindUIElements();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister from scene load event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshUnlockStatus(); // Check which abilities are unlocked
        StartUI();
        FindPlayerAbilities(); // Refresh all images when a new scene is loaded
    }

    void Start()
    {
        RefreshUnlockStatus(); // Initial check on start
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            pm = player.GetComponent<PlayerMovement>();
            pa = player.GetComponent<PlayerAttack>();
            ps = player.GetComponent<PlayerSummon>();
            pr = player.GetComponent<PlayerReveal>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CycleAbility(-1); // Cycle left on Q press
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            CycleAbility(1); // Cycle right on E press
        }
    }

    
    private void FindPlayerAbilities()
    {
        if (player != null)
        {
            Debug.Log("Player found.");
            pm = player.GetComponent<PlayerMovement>();
            pa = player.GetComponent<PlayerAttack>();
            ps = player.GetComponent<PlayerSummon>();
            pr = player.GetComponent<PlayerReveal>();
        }
        else
        {
        }
    }

    private void ActivateCurrentAbility()
    {
        DeactivateAllAbilities();

        switch (abilities[currentAbilityIndex].name.ToLower())
        {
            case "dash":
                if (pm != null){
                    pm.learnDash();
                }
                break;
            case "attack":
                if (pa != null){
                    pa.learnAttack();
                }
                break;
            case "summon":
                if (ps != null){
                ps.learnSummon();
                }
                break;
            case "reveal":
                if (pr != null){
                    pr.learnReveal();
                }
                break;
            default:
                Debug.LogError("Unrecognized ability.");
                break;
        }
    }

    private void DeactivateAllAbilities()
    {
        if (pm != null){
            pm.DeactivateDash();
        }
        if (pa != null){
            pa.DeactivateAttack();
        }
        if (ps != null){
            ps.DeactivateSummon();
        }
        if (pr != null){
            pr.DeactivateReveal();
        }

    }

    public void SetAbilityUnlocked(int index, bool unlocked)
    {
        if (index >= 0 && index < abilitiesUnlocked.Length)
        {
            abilitiesUnlocked[index] = unlocked;
            
            // Check if the newly unlocked ability should be set as the current ability
            // This can be based on your game's logic. For example, you might want to automatically
            // switch to the newly unlocked ability if it's the only one unlocked, or based on some other condition.
            // If so, update currentAbilityIndex here.
            // Example condition: if the unlocked ability is the first one being unlocked or if you want to auto-switch:
            if (unlocked && (IsFirstAbilityUnlocked() || ShouldAutoSwitchToNewAbility()))
            {
                currentAbilityIndex = index;
            }

            RefreshUI(); // Update UI to reflect the change
        }
    }

    // Helper method to check if the ability being unlocked is the first one in the game.
    private bool IsFirstAbilityUnlocked()
    {
        foreach (bool unlocked in abilitiesUnlocked)
        {
            if (unlocked)
            {
                return false; // Found another ability that's already unlocked
            }
        }
        return true; // No other abilities are unlocked
    }

    // You can define this method based on your game's logic
    // For example, it could check if the player is in a tutorial phase where you want to auto-switch to newly unlocked abilities
    private bool ShouldAutoSwitchToNewAbility()
    {
        // Implement your logic here
        // For now, let's assume we always want to switch to the new ability
        return true;
    }

    private void RefreshUnlockStatus()
    {
        // This method needs to be filled with the logic to check the unlocked status of abilities
        // For now, it's just a placeholder. You might fill this based on saved data or GameManager state.
    }

    void CycleAbility(int direction)
    {
        int originalIndex = currentAbilityIndex;
        do
        {
            currentAbilityIndex += direction;

            // Ensure the index wraps around correctly
            if (currentAbilityIndex >= abilities.Length) currentAbilityIndex = 0;
            else if (currentAbilityIndex < 0) currentAbilityIndex = abilities.Length - 1;

            // If we've looped all the way around, break to avoid infinite loop
            if (currentAbilityIndex == originalIndex) break;

        } while (!abilitiesUnlocked[currentAbilityIndex]); // Skip abilities that are not unlocked

        // Refresh UI Elements
        RefreshUI();
    }

    void RefreshUI()
    {
        // Update only if the ability is unlocked
        if (abilitiesUnlocked[currentAbilityIndex])
        {
            mainAbilityIcon.sprite = abilities[currentAbilityIndex].icon;
        }
        else
        {
            mainAbilityIcon.sprite = defaultIcon; // Consider a default or locked icon
        }

        // Calculate next and previous indices considering unlocked status
        int nextIndex = FindNextUnlockedAbility(currentAbilityIndex, 1);
        int prevIndex = FindNextUnlockedAbility(currentAbilityIndex, -1);

        nextAbilityIcon.sprite = abilitiesUnlocked[nextIndex] ? abilities[nextIndex].icon : defaultIcon;
        previousAbilityIcon.sprite = abilitiesUnlocked[prevIndex] ? abilities[prevIndex].icon : defaultIcon;

        ActivateCurrentAbility();
    }

    void StartUI()
    {
        // Update only if the ability is unlocked
        if (abilitiesUnlocked[currentAbilityIndex])
        {
            mainAbilityIcon.sprite = abilities[currentAbilityIndex].icon;
        }
        else
        {
            mainAbilityIcon.sprite = defaultIcon; // Consider a default or locked icon
        }

        // Calculate next and previous indices considering unlocked status
        // Initialize UI with default icons if the current ability is locked or not available

        int nextIndex = FindNextUnlockedAbility(currentAbilityIndex, 1);
        int prevIndex = FindNextUnlockedAbility(currentAbilityIndex, -1);

        nextAbilityIcon.sprite = abilitiesUnlocked[nextIndex] ? abilities[nextIndex].icon : defaultIcon;
        previousAbilityIcon.sprite = abilitiesUnlocked[prevIndex] ? abilities[prevIndex].icon : defaultIcon;

    }

    int FindNextUnlockedAbility(int startIndex, int step)
    {
        int index = startIndex;
        do
        {
            index += step;

            // Wrap index if needed
            if (index >= abilities.Length) index = 0;
            else if (index < 0) index = abilities.Length - 1;

            // If ability is unlocked, return its index
            if (abilitiesUnlocked[index]) return index;

        } while (index != startIndex); // Avoid infinite loops

        return startIndex; // Return the original index if no unlocked ability was found
    }

    void FindUIElements()
{
    // Finds the game objects with specific tags and gets their Image components
    GameObject mainAbilityIconGO = GameObject.FindGameObjectWithTag("MainAbilityIcon");
    if (mainAbilityIconGO != null)
        mainAbilityIcon = mainAbilityIconGO.GetComponent<Image>();
    else
        Debug.LogError("MainAbilityIcon not found.");

    GameObject nextAbilityIconGO = GameObject.FindGameObjectWithTag("NextAbilityIcon");
    if (nextAbilityIconGO != null)
        nextAbilityIcon = nextAbilityIconGO.GetComponent<Image>();
    else
        Debug.LogError("NextAbilityIcon not found.");

    GameObject previousAbilityIconGO = GameObject.FindGameObjectWithTag("PreviousAbilityIcon");
    if (previousAbilityIconGO != null)
        previousAbilityIcon = previousAbilityIconGO.GetComponent<Image>();
    else
        Debug.LogError("PreviousAbilityIcon not found.");
}
}
