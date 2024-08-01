using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class healthBarScript : MonoBehaviour
{
    public Slider S;
    
   
    
    public void setMax(float maxhealth)
    {
        S.maxValue = maxhealth;
        S.value = maxhealth;
    }
    public void setHealth(float health) { S.value = health; }
    

    

}
