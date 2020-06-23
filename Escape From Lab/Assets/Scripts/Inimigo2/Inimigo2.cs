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

    public GameObject pedraPermanete;
    public GameObject pedraInstancia;

    public bool usaCurvaAnim;
    public CapsuleCollider capcollider;

    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody>();
        navMesh = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        estaMorto = false;
        usaCurvaAnim = false;
        capcollider = GetComponent<CapsuleCollider>();
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
                anim.CrossFade("Zombie Death", 0.2f);
                transform.gameObject.layer = 10;
                anim.applyRootMotion = true;
                capcollider.direction = 2;
                usaCurvaAnim = false;
            }

            if(usaCurvaAnim && !anim.IsInTransition(0))
            {
                capcollider.height = anim.GetFloat("AlturaCollider");
                capcollider.center = new Vector3(0, anim.GetFloat("CentroColliderY"), 0);
            }
            else
            {
                capcollider.height = 2;
                capcollider.center = new Vector3(0, 1, 0);
            }
        }
    }

    public void InstanciaPedra()
    {
        pedraPermanete.SetActive(false);
        GameObject pedra = Instantiate(pedraInstancia, anim.GetBoneTransform(HumanBodyBones.RightHand).transform);
        pedra.transform.parent = null;
        pedra.transform.LookAt(player.transform.position);
        JogaPedra jogaScript = pedra.GetComponent<JogaPedra>();
        jogaScript.Joga();
    }

    public void AparecePedraPermanente()
    {
        pedraPermanete.SetActive(true);
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
            usaCurvaAnim = true;
        }
        else
        {
            pedraPermanete.SetActive(false);
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

    public void LevouDano(int dano)
    {
        hp -= dano;
    }
}
