using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Inimigo2 : MonoBehaviour
{

    public NavMeshAgent navMesh;
    public GameObject player;
    public float distanciaAtaque;
    public float distDoPlayer;
    public float velocidade = 5;

    Animator anim;

    public int hp = 100;
    public bool estaMorto;
    public Rigidbody rgb;

    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        estaMorto = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!estaMorto)
        {
            distDoPlayer = Vector3.Distance(transform.position, player.transform.position);

            VaiAtrasJogador();

            OlhaParaPlayer();

            if(hp <= 0)
            {
                estaMorto = true;
                navMesh.isStopped = true;
                navMesh.enabled = false;

                CorrigiRigEntra();
            }
        }
    }

    void OlhaParaPlayer()
    {
        Vector3 direcaoOlha = player.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlha);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
    }

    void VaiAtrasJogador()
    {
        navMesh.speed = velocidade;

        if (distDoPlayer < distanciaAtaque)
        {
            navMesh.isStopped = true;
            anim.SetBool("Joga", true);
            CorrigiRigEntra();
        }       
        else 
        {
            anim.SetBool("Joga", false);
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
            CorrigiRigSai();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            CorrigiRigEntra();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CorrigiRigSai();
        }
    }

    void CorrigiRigEntra()
    {
        rgb.isKinematic = true;
        rgb.velocity = Vector3.zero;
    }

    void CorrigiRigSai()
    {
        rgb.isKinematic = false;
    }
}
