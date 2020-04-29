using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    List<Rigidbody> ragdollRigids = new List<Rigidbody>();
    public Rigidbody rgb;
    List<Collider> ragdollColliders = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
    }

    public void DesativaRagdoll()
    {
        Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();

        for(int i = 0; i < rigs.Length; i++)
        {
            if(rigs[i] == rgb)
            {
                continue;
            }

            ragdollRigids.Add(rigs[i]);
            rigs[i].isKinematic = true;

            Collider col = rigs[i].gameObject.GetComponent<Collider>();
            col.enabled = false;
            ragdollColliders.Add(col);
        }
    }

    public void AtivaRagdoll()
    {
        for (int i = 0; i < ragdollRigids.Count; i++)
        {
            ragdollRigids[i].isKinematic = false;
            ragdollColliders[i].enabled = true; 
        }

        rgb.isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine("FinalizaAnim");
    }

    IEnumerator FinalizaAnim()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<Animator>().enabled = false;
        this.enabled = false;
    }
}
