using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardScript : MonoBehaviour
{
    public float growInc;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HazardLifeTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HazardLifeTime() {
        yield return StartCoroutine(GrowHazard());
        yield return StartCoroutine(RemoveHazard());
    }

    IEnumerator GrowHazard()
    {
        while (gameObject.transform.localScale.x < 1) {
            gameObject.transform.localScale += new Vector3(growInc, growInc, growInc) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //yield return StartCoroutine(RemoveHazard());
    }

    IEnumerator RemoveHazard() {
        while (gameObject.transform.localScale.x > 0.01f)
        {
            gameObject.transform.localScale -= new Vector3(growInc, growInc, growInc) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
