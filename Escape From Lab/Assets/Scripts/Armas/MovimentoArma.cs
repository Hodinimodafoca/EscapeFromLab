using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BASA
{
    public class MovimentoArma : MonoBehaviour
    {
        public float valor;
        public float suavizaValor;
        public float valorMaximo;
        Vector3 posInicial;

        void Start()
        {
            posInicial = transform.localPosition;
        }

        
        void Update()
        {
            float movX = -Input.GetAxis("Mouse X") * valor;
            float movY = -Input.GetAxis("Mouse Y") * valor;

            movX = Mathf.Clamp(movX, -valorMaximo, valorMaximo);
            movY = Mathf.Clamp(movY, -valorMaximo, valorMaximo);

            Vector3 finalPos = new Vector3(movX, movY, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + posInicial, Time.deltaTime * suavizaValor);
        }
    }
}

