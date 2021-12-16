using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodedObjDestroyer : MonoBehaviour
{
    private IEnumerator destroySelf;
    // Start is called before the first frame update
    void Start()
    {
        destroySelf = DestroySelf();
        StartCoroutine(destroySelf);
    }

    private IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
