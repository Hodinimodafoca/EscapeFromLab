using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BASA;

public class MovimentoHead : MonoBehaviour
{
    private float tempo = 0.0f;
    public float velocidade = 0.1f;
    public float forca = 0.2f;
    public float pontoDeOrigem = 0.0f;

    float cortaOnda;
    float horizontal;
    float vertical;
    Vector3 salvaPos;

    AudioSource audiosource;
    public AudioClip[] audioClip;
    public int indexPassos;

    Movimento scriptMov;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        indexPassos = 0;
    }

    void Update()
    {
        scriptMov = GetComponentInParent<Movimento>();
        cortaOnda = 0.0f;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        salvaPos = transform.localPosition;

        if(Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
        {
            tempo = 0.0f;
        }
        else
        {
            cortaOnda = Mathf.Sin(tempo);
            tempo = tempo + velocidade;

            if(tempo > Mathf.PI * 2)
            {
                tempo = tempo - (Mathf.PI * 2);
            }
        }

        if(cortaOnda != 0)
        {
            float mudaMovimentacao = cortaOnda * forca;
            float eixosTotais = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            eixosTotais = Mathf.Clamp(eixosTotais, 0.0f, 1.0f);
            mudaMovimentacao = eixosTotais * mudaMovimentacao;
            salvaPos.y = pontoDeOrigem + mudaMovimentacao;
        }
        else
        {
            salvaPos.y = pontoDeOrigem;

        }
        transform.localPosition = salvaPos;

        SomPassos();
        AtualizaHead();
    }

    void SomPassos()
    {
        if(cortaOnda <= -0.95f && !audiosource.isPlaying && scriptMov.estaNoChao)
        {
            audiosource.clip = audioClip[indexPassos];
            audiosource.Play();
            indexPassos++;
            if(indexPassos >= 4)
            {
                indexPassos = 0;
            }
        }
    }

    void AtualizaHead()
    {
        if (scriptMov.estaCorrendo)
        {
            velocidade = 0.25f;
            forca = 0.25f;
        }
        else if (scriptMov.estaAbaixado)
        {
            velocidade = 0.15f;
            forca = 0.11f;
        }
        else
        {
            velocidade = 0.18f;
            forca = 0.15f;
        }
    }
}
