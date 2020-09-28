using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateNewWorld : MonoBehaviour
{ 
    public TMPro.TMP_InputField worldWidth;
    public TMPro.TMP_InputField worldHeight;

    public void createWorld()
    {
        PlayerPrefs.SetInt("WorldWidth", int.Parse(worldWidth.text));
        PlayerPrefs.SetInt("WorldHeight", int.Parse(worldHeight.text));

        SceneManager.LoadScene("World");
    }
}
