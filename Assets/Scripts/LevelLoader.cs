using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int iLvToLoad;
    public string sLvToLoad;
    public string currentScene;

    public static int enemyNum = 4;

    public bool useIntLoad = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collisionGameObject = collision.gameObject;

        if (collisionGameObject.name == "Player")
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        if (enemyNum == 0)
        {
            if (useIntLoad)
            {
                SceneManager.LoadScene(iLvToLoad);
            }
            else
            {
                SceneManager.LoadScene(sLvToLoad);
            }
        }
        else
        {
            SceneManager.LoadScene(currentScene);
        }
    }
    public static void DestroyEnemy()
    {
        enemyNum--;
    }
}
