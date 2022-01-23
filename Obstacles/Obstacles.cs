using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JumpHere.Obstacles
{
    public class Obstacles : MonoBehaviour
    {
        public virtual void OnCollisionEnter(Collision collision)
        {
            StartCoroutine(DestroyDelay());
        }
        IEnumerator DestroyDelay()
        {
            Collider[] colliders = GetComponents<Collider>();
            if(colliders.Length > 0)
                foreach(Collider collider in colliders)
                    collider.enabled = false;
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }
    }
}