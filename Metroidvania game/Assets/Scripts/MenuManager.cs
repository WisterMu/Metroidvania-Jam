using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // This script is responsible for managing the menu and loading scenes additively


    // [SerializeField]
    // private GameObject mainMenu; // Reference to the Main Menu GameObject
    // [SerializeField]
    // private GameObject optionsMenu; // Reference to the Options menu GameObject
    // [SerializeField]
    // private GameObject creditsMenu; // Reference to the currently active menu GameObject
    [SerializeField]

    private GameObject[] menus; // Array of menu GameObjects, assigned in the inspector
    public int currMenuIndex = 0; // Index of the menu in the build settings (0 for MainMenu, 1 for OptionsMenu, 2 for Credits, etc.)
    public bool isIngame = false; // Flag to track if the menu was opened in-game (main menu should not be loaded in this case)

    void Awake()
    {
        LoadMenu(currMenuIndex); // Load the initial menu based on the current menu index
    }

    void Update()
    {

    }

    public void LoadMenu(int menuIndex = 0)
    {
        // Set the active menu based on the index

        // Check if valid
        // Debug.Log("Loading menu with index: " + menuIndex);
        Debug.Log(menus);
        if (menuIndex < 0 || menuIndex >= menus.Length)
        {
            Debug.LogError("Invalid menu index: " + menuIndex);
            return;
        }

        currMenuIndex = menuIndex; // Update the current menu index
        for (int i = 0; i < menus.Length; i++)
        {
            if (i == currMenuIndex)
            {
                menus[i].SetActive(true); // Activate the selected menu
            }
            else
            {
                menus[i].SetActive(false); // Deactivate all other menus
            }
        }
        // UpdateMenuState(); // Update the menu state based on the current game state
        Debug.Log("Loaded menu: " + menus[currMenuIndex].name);
    }

    public void UnloadMenu()
    {
        // Unload the menu scene
        SceneManager.UnloadSceneAsync("MainMenu").completed += (op) =>
        {
            Debug.Log("Menu unloaded successfully.");
        };
    }

    public void QuitGame()
    {
        // This method is called when the Quit button is pressed
        Debug.Log("Quitting game...");
        Application.Quit(); // Quit the application
    }
    
    // void UpdateMenuState()
    // {
    //     // This method can be used to update the state of the menu based on the current game state
    //     if (isIngame)
    //     {
    //         switch (currMenuIndex)
    //         {
    //             case 0:
    //                 // Main Menu
    //                 Debug.Log("In-game: Main Menu loaded. // SHOULD NOT BE LOADED IN-GAME");
    //                 // Note: In-game main menu should not be loaded, this is just a placeholder
    //                 break;
    //             case 1:
    //                 // Options Menu
    //                 Debug.Log("In-game: Options Menu loaded.");
    //                 Button backButton = optionsMenu.transform.Find("BackButton").GetComponent<UnityEngine.UI.Button>();
    //                 backButton.onClick.RemoveAllListeners(); // Clear existing listeners
    //                 backButton.onClick.AddListener(() => SceneManager.UnloadSceneAsync("MainMenu")); // Add listener to unload menu
    //                 break;
    //             case 2:
    //                 // Credits Menu
    //                 Debug.Log("In-game: Credits Menu loaded.");
    //                 break;
    //             default:
    //                 Debug.LogError("Invalid menu index for in-game state: " + currMenuIndex);
    //                 break;
    //         }
    //     }
    //     else
    //     {
    //         // If not in-game, load the main menu
    //         switch (currMenuIndex)
    //         {
    //             case 0:
    //                 // Main Menu
    //                 Debug.Log("Main Menu loaded.");
    //                 break;
    //             case 1:
    //                 // Options Menu
    //                 Debug.Log("Options Menu loaded.");
    //                 break;
    //             case 2:
    //                 // Credits Menu
    //                 Debug.Log("Credits Menu loaded.");
    //                 break;
    //             default:
    //                 Debug.LogError("Invalid menu index: " + currMenuIndex);
    //                 break;
    //         }
    //     }
    // }
}
