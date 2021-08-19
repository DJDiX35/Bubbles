using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderTester : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Enter! (" + name + ")");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Enter! (" + name + ")");
    }
}
