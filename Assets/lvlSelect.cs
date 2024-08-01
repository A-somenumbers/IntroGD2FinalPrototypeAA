using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lvlSelect : MonoBehaviour
{
    public int lvl;
    public int lvl2;
    public int lvl3;

    public void Level1()
    {
        SceneManager.LoadScene(lvl);
    }
    public void Level2()
    {
        SceneManager.LoadScene(lvl2);
    }
    public void Level3()
    {
        SceneManager.LoadScene(lvl3);
    }

}
