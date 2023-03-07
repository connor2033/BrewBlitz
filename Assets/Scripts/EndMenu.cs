using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    
    public void Quit()
    {
        Debug.Log("Quitting game. Goodbye!");
        Application.Quit();
    }

}
