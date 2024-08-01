using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class enemyCounter : MonoBehaviour
{
    private int m_Counter;
    [SerializeField] private TMP_Text counterDisplay;
    // Start is called before the first frame update
    void Start()
    {
        m_Counter = GameObject.FindGameObjectsWithTag("enemy").Length;
    }

    // Update is called once per frame
    void Update()
    {
        m_Counter = GameObject.FindGameObjectsWithTag("enemy").Length;
        if (GameObject.FindGameObjectsWithTag("enemy").Length > 0)
        {
            counterDisplay.text = "Enemies Remaining: " + m_Counter.ToString();
        }
        else
        {
            counterDisplay.text = "All Enemies Defeated!";
        }
    }
}
