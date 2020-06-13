using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BASA;

public class Inimigo1 : MonoBehaviour
{

    public NavMeshAgent navMesh;
    public GameObject player;
    public GameObject objDesliza;
    public float distanciaAtaque;
    public float distanciaPlayer;
    public float velocidade = 4f;

    Animator anim;

    public int HP = 100;

    Ragdoll ragScript;

    public bool estaMorto;
    public bool furia;
    public bool invencivel;

    public Renderer render;
    private CapsuleCollider capsule;

    public AudioClip[] sonsMonstro;
    public AudioSource audioMonstro;

    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        ragScript = GetComponent<Ragdoll>();
        render = GetComponentInChildren<Renderer>();

        audioMonstro = GetComponent<AudioSource>();

        estaMorto = false;
        invencivel = false;
        ragScript.DesativaRagdoll();
        
        capsule = GetComponent<CapsuleCollider>();
    }


    void Update()
    {
        if (!estaMorto)
        {
            distanciaPlayer = Vector3.Distance(transform.position, player.transform.position);

            VaiAtrasJogador();

            OlhaParaPlayer();

            capsule.enabled = true;

            if(HP <= 50 && !furia)
            {
                furia = true;
                anim.ResetTrigger("LevouTiro");
                ParaDeAndar();
                anim.CrossFade("Zombie Scream", 0.2f);
                render.material.color = Color.red;
                velocidade = 8;
            }

            if (HP <= 0 && !estaMorto)
            {
                render.material.color = Color.white;
                objDesliza.SetActive(false);
                estaMorto = true;
                ParaDeAndar();
                navMesh.enabled = false;
                ragScript.AtivaRagdoll();
                MorreSom();
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

        if(distanciaPlayer < distanciaAtaque)
        {
            navMesh.isStopped = true;
            Debug.Log("Atacando");
            anim.SetTrigger("Ataca");
            anim.SetBool("PodeAndar", false);
            anim.SetBool("ParaAtaque", false);
            CorrigiRigEntra();
        }
        if(distanciaPlayer >= 3)
        {
            anim.SetBool("ParaAtaque", true);
        }
        if(anim.GetBool("PodeAndar"))
        {
            navMesh.isStopped = false;
            navMesh.SetDestination(player.transform.position);
            anim.ResetTrigger("Ataca");
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
        ragScript.rgb.isKinematic = true;
        ragScript.rgb.velocity = Vector3.zero;
    }

    void CorrigiRigSai()
    {
        ragScript.rgb.isKinematic = false;
    }

    public void LevouDano(int dano)
    {
        int n;

        n = Random.Range(0, 10);

        if(n % 2 == 0 && !furia)
        {
            anim.SetTrigger("LevouTiro");
            ParaDeAndar();
        }

        if (!invencivel)
        {
            HP -= dano;
        }
    }

    void ParaDeAndar()
    {
        navMesh.isStopped = true;
        //anim.SetTrigger("LevouTiro");
        anim.SetBool("PodeAndar", false);
        CorrigiRigEntra();
    }

    public void DanoPlayer()
    {
        player.GetComponent<Movimento>().hp -= 10;
    }

    public void FicaInvencivel()
    {
        invencivel = true;
    }

    public void SaiInvencivel()
    {
        invencivel = false;
        anim.speed = 2;
    }

    public void PassoMonstro()
    {
        audioMonstro.volume = 0.05f;
        audioMonstro.PlayOneShot(sonsMonstro[0]);
    }

    public void SenteDor()
    {
        audioMonstro.volume = 1f;
        audioMonstro.clip = sonsMonstro[1];
        audioMonstro.Play();
    }

    public void GritaSom()
    {
        audioMonstro.volume = 1f;
        audioMonstro.clip = sonsMonstro[2];
        audioMonstro.Play();
    }

    public void MorreSom()
    {
        audioMonstro.volume = 1f;
        audioMonstro.clip = sonsMonstro[3];
        audioMonstro.Play();
    }
}
