using UnityEngine;
using System.Collections;

public class ObjectDestroyed : MonoBehaviour {
    public OnObjectDestroyed onObjectDestroyed = null;
    public delegate void OnObjectDestroyed(Transform t);

    public void OnDestroy() {
        if(onObjectDestroyed != null) {
            onObjectDestroyed(transform);
        }
    }
}
