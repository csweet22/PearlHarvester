using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarterScript : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("MainScene");
    }
    
}
