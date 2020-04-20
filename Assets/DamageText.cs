using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float animationSpeed = 0.8f;
    public float lifeTime = 1f;


    void Update()
    {
        transform.position = new Vector3 (transform.position.x, transform.position.y + animationSpeed, transform.position.z);
    }
    void Start()
    {
        StartCoroutine(Die(lifeTime));
    }

    IEnumerator Die(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(this.gameObject);
    }
}
