using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BASA
{
    public class Movimento : MonoBehaviour
    {
        [Header("Config Personagem")]
        public CharacterController controle;
        public float velocidade = 6f;
        public float alturaPulo = 3f;
        public float gravidade = -20f;
        public bool estaCorrendo;
        public AudioClip[] audiosJump;
        AudioSource audioPulo;
        bool noAr;

        [Header("Verifica Chao")]
        public Transform checkChao;
        public float raioEsfera = 0.4f;
        public LayerMask chaoMask;
        public bool estaNoChao;
        Vector3 velocidadeCai;

        [Header("Verifica Abaixado")]
        public Transform cameraTransform;
        public bool estaAbaixado;
        public bool levantarBloqueado;
        public float alturaLevantado, alturaAbaixado, posCameraEmPe, posCameraAbaixado;
        float velCorrente = 1f;
        RaycastHit hit;

        [Header("Status Personagem")]
        public float hp = 100;
        public float stamina = 100;
        public bool cansado;
        public Respiracao scriptResp;


        void Start()
        {
            controle = GetComponent<CharacterController>();
            cameraTransform = Camera.main.transform;
            estaAbaixado = false;
            estaCorrendo = false;
            audioPulo = GetComponent<AudioSource>();
            noAr = false;
        }

        void Update()
        {
            Verificacoes();
            MovimentoAbaixa();
            Inputs();
            CondicaoPlayer();
            SomPulos();
        }

        void SomPulos()
        {
            if (!estaNoChao)
            {
                noAr = true;
            }

            if(estaNoChao && noAr)
            {
                noAr = false;
                audioPulo.clip = audiosJump[1];
                audioPulo.Play();
            }
        }

        void Verificacoes()
        {
            estaNoChao = Physics.CheckSphere(checkChao.position, raioEsfera, chaoMask);

            if (estaNoChao && velocidadeCai.y < 0)
            {
                velocidadeCai.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = (transform.right * x + transform.forward * z).normalized;

            controle.Move(move * velocidade * Time.deltaTime);

            velocidadeCai.y += gravidade * Time.deltaTime;

            controle.Move(velocidadeCai * Time.deltaTime);
            
        }

        void MovimentoAbaixa()
        {
            controle.center = Vector3.down * (alturaLevantado - controle.height) / 2f;

            if (estaAbaixado)
            {
                controle.height = Mathf.Lerp(controle.height, alturaAbaixado, Time.deltaTime * 3);
                float novoY = Mathf.SmoothDamp(cameraTransform.localPosition.y, posCameraAbaixado, ref velCorrente, Time.deltaTime * 3);
                cameraTransform.localPosition = new Vector3(0, novoY);
                velocidade = 3f;
                CheckBloqueioAbaaixado();
            }
            else
            {
                controle.height = Mathf.Lerp(controle.height, alturaLevantado, Time.deltaTime * 3);
                float novoY = Mathf.SmoothDamp(cameraTransform.localPosition.y, posCameraEmPe, ref velCorrente, Time.deltaTime * 3);
                cameraTransform.localPosition = new Vector3(0, novoY);
                velocidade = 6f;
            }
        }


        void Inputs()
        {
            if(Input.GetKey(KeyCode.LeftShift) && estaNoChao && !estaAbaixado && !cansado)
            {
                velocidade = 9;
                estaCorrendo = true;
                stamina -= 0.3f;
                stamina = Mathf.Clamp(stamina, 0, 100);
            }
            else
            {
                estaCorrendo = false;
                stamina += 0.1f;
                stamina = Mathf.Clamp(stamina, 0, 100);
            }

            if (Input.GetButtonDown("Jump") && estaNoChao)
            {
                velocidadeCai.y = Mathf.Sqrt(alturaPulo * -2f * gravidade);
                audioPulo.clip = audiosJump[0];
                audioPulo.Play();
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Abaixa();
            }
        }


        void Abaixa()
        {

            if(levantarBloqueado || estaNoChao == false)
            {
                return;
            }

            estaAbaixado = !estaAbaixado;
            
        }

        void CheckBloqueioAbaaixado()
        {
            Debug.DrawRay(cameraTransform.position, Vector3.up * 1.1f, Color.red);
            if (Physics.Raycast(cameraTransform.position, Vector3.up, out hit, 1.1f))
            {
                levantarBloqueado = true;
            }
            else
            {
                levantarBloqueado = false;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(checkChao.position, raioEsfera); 
        }

        void CondicaoPlayer()
        {
            if(stamina == 0)
            {
                cansado = true;
                scriptResp.forcaResp = 5;
            }

            if(stamina > 20)
            {
                cansado = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("cabecaDesliza"))
            {
                controle.SimpleMove(transform.forward * 1000 * Time.deltaTime);
            }
        }
    }
}

