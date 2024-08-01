using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handles : MonoBehaviour
{
    public movement m;
    Animator anim;

    
    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        m = gameObject.GetComponent<movement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        float magnitute = Mathf.Clamp(m.getMoveSpeed(), 0.0f, m.runS) / m.runS;
        anim.SetFloat("Magnitute of speed", magnitute);
    }
}
