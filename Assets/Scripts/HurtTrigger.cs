using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtTrigger : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<LifeEntity>().BeHurt();
    }
}
