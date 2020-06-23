using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BASA;

public class GlockAnim : MonoBehaviour
{
    Animator anim;
    bool estaAtirando;
    RaycastHit hit;

    public GameObject faisca, buraco, fumaca, efeitoTiro, posEfeitoTiro, particulaSangue;

    public ParticleSystem rastroBala;

    public AudioSource audioArma;
    public AudioClip[] sonsArma;

    public int carregador = 3;
    public int municao = 17;

    UIMangers Uiscript;
    MovimentoArma movArmaScript;
    public GameObject posUI;

    public bool automatico;
    public float numAleatorioMira;

    public float valorMira;

    void Start()
    {
        automatico = false;
        estaAtirando = false;
        anim = GetComponent<Animator>();
        audioArma = GetComponent<AudioSource>();
        Uiscript = GameObject.FindWithTag("uiManager").GetComponent<UIMangers>();
        movArmaScript = GetComponentInParent<MovimentoArma>();
        valorMira = 300;
    }

    
    void Update()
    {

        Uiscript.municao.transform.position = Camera.main.WorldToScreenPoint(posUI.transform.position);
        Uiscript.municao.text = municao.ToString() + "/" + carregador.ToString();

        ModificaMira();

        if (anim.GetBool("OcorreAcao"))
        {
            return;
        }

        Automatico();
        Atirar();
        Recarregar();
        Mirar();
    }

    void ModificaMira()
    {
        if (estaAtirando)
        {
            valorMira = Mathf.Lerp(valorMira, 450, Time.deltaTime * 20);
            Uiscript.mira.sizeDelta = new Vector2(valorMira, valorMira);
        }
        else
        {
            valorMira = Mathf.Lerp(valorMira, 300, Time.deltaTime * 20);
            Uiscript.mira.sizeDelta = new Vector2(valorMira, valorMira);
        }
    }

    void Automatico()
    {
            if (Input.GetKeyDown(KeyCode.Q))
        {
            audioArma.clip = sonsArma[4];
            audioArma.Play();
            automatico = !automatico;

            if (automatico)
            {
                Uiscript.imgModoTiro.sprite = Uiscript.spriteModoTiro[1];
            }
            else
            {
                Uiscript.imgModoTiro.sprite = Uiscript.spriteModoTiro[0];
            }
        }
    }

    void Atirar()
    {
        if (Input.GetButtonDown("Fire1") || automatico ? Input.GetButton("Fire1") : false)
        {
            if (!estaAtirando && municao > 0)
            {
                municao--;
                audioArma.clip = sonsArma[0];
                audioArma.Play();
                rastroBala.Play();
                estaAtirando = true;
                StartCoroutine(Atirando());
            }
            else if (!estaAtirando && municao == 0 && carregador > 0)
            {
                anim.Play("Recarregar");
                carregador--;
                municao = 17;
            }
            else if (municao == 0 && carregador == 0)
            {
                audioArma.clip = sonsArma[3];
                audioArma.Play();
            }
        }
    }

    void Recarregar()
    {
        if (Input.GetKeyDown(KeyCode.R) && carregador > 0 && municao < 17)
        {
            anim.Play("Recarregar");
            carregador--;
            municao = 17;
        }
    }

    void Mirar()
    {
        if (Input.GetButton("Fire2"))
        {
            anim.SetBool("Mira", true);
            posUI.transform.localPosition = new Vector3(-0.15f, 0.0418f, 0.1342f);
            //Camera.main.fieldOfView = 45;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 45, Time.deltaTime * 10);
            Uiscript.mira.gameObject.SetActive(false);
            movArmaScript.valor = 0.01f;
            numAleatorioMira = 0f;
        }
        else
        {
            anim.SetBool("Mira", false);
            posUI.transform.localPosition = new Vector3(-0.2271f, 0.0418f, 0.1342f);
            //Camera.main.fieldOfView = 60;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, Time.deltaTime * 10);
            Uiscript.mira.gameObject.SetActive(true);
            movArmaScript.valor = 0.1f;
            numAleatorioMira = 0.05f;
        }
    }

    IEnumerator Atirando()
    {
        float screenX = Screen.width / 2;
        float screenY = Screen.height / 2;

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY));
        anim.Play("Atirar");

        GameObject efeitoTiroObj = Instantiate(efeitoTiro, posEfeitoTiro.transform.position, posEfeitoTiro.transform.rotation);
        efeitoTiroObj.transform.parent = posEfeitoTiro.transform;

        if (Physics.Raycast(new Vector3(ray.origin.x + Random.Range(-numAleatorioMira, numAleatorioMira), ray.origin.y + Random.Range(-numAleatorioMira, numAleatorioMira), ray.origin.z), Camera.main.transform.forward, out hit))
        {
            if (hit.transform.tag == "Inimigo1")
            {
                if (hit.transform.GetComponent<Inimigo1>() || hit.transform.GetComponent<Inimigo2>())
                {
                    InimigoVerificadorDano();
                }
                else if (hit.rigidbody != null && hit.transform.GetComponentInParent<Inimigo1>())
                {
                    AdicionaForca(ray, 900);
                }

                GameObject particulaCriada = Instantiate(particulaSangue, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                particulaCriada.transform.parent = hit.transform;
            }
            else
            {
                InstanciaEfeitos();

                if(hit.rigidbody != null)
                {
                    AdicionaForca(ray, 400);
                }
            }

        }

        yield return new WaitForSeconds(0.3f);
        estaAtirando = false;
    }

    void InimigoVerificadorDano()
    {
        if (hit.transform.GetComponent<Inimigo1>())
        {
            hit.transform.GetComponent<Inimigo1>().LevouDano(15);
        }
        else if (hit.transform.GetComponent<Inimigo2>())
        {
            hit.transform.GetComponent<Inimigo2>().LevouDano(15);
        }
    }

    void InstanciaEfeitos()
    {
        Instantiate(faisca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Instantiate(fumaca, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

        GameObject buracoObj = Instantiate(buraco, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        buracoObj.transform.parent = hit.transform;
    }

    void SomMagazine()
    {
        audioArma.clip = sonsArma[1];
        audioArma.Play();
    }

    void SomUp()
    {
        audioArma.clip = sonsArma[2];
        audioArma.Play();
    }

    void AdicionaForca(Ray ray, float forca)
    {
        Vector3 direcaoBala = ray.direction;
        hit.rigidbody.AddForceAtPosition(direcaoBala * forca, hit.point);
    }
}
