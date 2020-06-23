using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BASA;

public class JogaPedra : MonoBehaviour
{
    Rigidbody rgb;
    public float hVelocidade = 15;
    public float vVelocidade = 4;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Destroy(this.gameObject, 8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Joga()
    {
        rgb = GetComponent<Rigidbody>();
        Vector3 targetForca = transform.forward * hVelocidade;
        targetForca += transform.up * vVelocidade;
        rgb.AddForce(targetForca, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            player.GetComponent<Movimento>().hp -= 30;
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject);
    }
}
