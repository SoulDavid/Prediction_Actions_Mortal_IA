using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (Input.anyKeyDown && SceneManager.GetActiveScene().name == "MenuScene")
        {
            if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1))
            {
                return;
            }
            SceneManager.LoadScene("GameScene");
        }

    }
}
