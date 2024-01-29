using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit_Behaviour : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("TE ORDENO RETURNER A MONKE");
        
        IA.instance.ReturnToIdle();
      //  IA.instance.Guess();
    }
}
