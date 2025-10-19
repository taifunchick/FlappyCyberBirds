using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeIncrease : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Score._instance.UpdateScore();
        }
    }
}
