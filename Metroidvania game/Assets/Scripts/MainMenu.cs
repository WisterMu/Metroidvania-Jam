using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void StartGame()
    {

     // Load the game scene
      SceneManager.LoadScene(1); //using the build index of the game scene which can be found in build profiles^^
   }

}
