using UnityEngine;
using UnityEngine.SceneManagement;
// This script is responsible for managing the menu button (ingame) and its interactions

public class MenuButton : MonoBehaviour
{
    private bool isMenuActive = false; // Flag to track if the menu is active
    MenuManager menuManager; // Reference to the MenuManager script
    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("Scene unloaded: " + scene.name);

        if (scene.name == "MainMenu")
        {
            // Do something after MainMenu is unloaded
            Debug.Log("MainMenu has been successfully unloaded.");
            isMenuActive = false; // Set the flag to false to close the menu
        }
    }

    void OnGUI()
    {
        if (isMenuActive)
        {
            // No button is easier tbh
            // // Display a button to close the menu
            // if (GUI.Button(new Rect(20, 20, 150, 30), "Close Menu"))
            // {
            //     isMenuActive = false; // Set the flag to false to close menu
            //     menuManager.UnloadMenu(); // Call the UnloadMenu method from MenuManager to close the menu
            // }
        }
        else
        {
            // Display a button to open the menu
            if (GUI.Button(new Rect(20, 20, 150, 30), "Open Menu"))
            {
                isMenuActive = true; // Set the flag to true to show the menu

                // Actual button stuff
                // Pressing button loads the MainMenu scene additively (doesn't unload the current scene)
                var loadOp = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);

                // Once loading is complete
                loadOp.completed += (op) =>
                {
                    menuManager = GameObject.Find("MenuManager").GetComponent<MenuManager>();
                    menuManager.isIngame = true; // Set the flag to true to indicate that the menu was opened in-game
                    menuManager.LoadMenu(3); // Call the LoadMenu method from MenuManager (opens the options menu)
                };
            }
        }
    }
}
