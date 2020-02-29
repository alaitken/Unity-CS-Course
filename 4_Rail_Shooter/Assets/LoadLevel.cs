using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] float waitTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("loadGame", waitTime);
    }

    private void loadGame()
    {
        SceneManager.LoadScene(1);
    }
}
