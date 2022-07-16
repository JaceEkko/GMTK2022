using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePower : MonoBehaviour
{
    public virtual IEnumerator ActivatePower() {
        yield return null;
    }
}
