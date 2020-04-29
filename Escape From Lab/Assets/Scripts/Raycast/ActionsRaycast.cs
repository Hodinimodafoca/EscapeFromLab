using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BASA
{
    [RequireComponent(typeof(RayCastScript))]
    public class ActionsRaycast : MonoBehaviour
    {
        RayCastScript raycastScript;
        public bool pegou;
        float distancia;
        public GameObject salvaObj;


        void Start()
        {
            raycastScript = GetComponent<RayCastScript>();
            pegou = false;
        }

        
        void Update()
        {
            distancia = raycastScript.distanciaAlvo;

            if(distancia <= 3)
            {
                if(Input.GetKeyDown(KeyCode.E) && raycastScript.objPega != null)
                {
                    Pegar();
                }

                if(Input.GetKeyDown(KeyCode.E) && raycastScript.objArrasta != null)
                {
                    if (!pegou)
                    {
                        pegou = true;
                        Arrastar();
                    }
                    else
                    {
                        pegou = false;
                        Soltar();
                    }
                }
            }
        }

        void Arrastar()
        {
            raycastScript.objArrasta.GetComponent<Rigidbody>().isKinematic = true;
            raycastScript.objArrasta.GetComponent<Rigidbody>().useGravity = false;
            raycastScript.objArrasta.transform.SetParent(transform);
            raycastScript.objArrasta.transform.localPosition = new Vector3(0, 0, 3);
            raycastScript.objArrasta.transform.localRotation = Quaternion.Euler(0, 0, 0);

        }

        void Soltar()
        {
            raycastScript.objArrasta.GetComponent<Rigidbody>().isKinematic = false;
            raycastScript.objArrasta.GetComponent<Rigidbody>().useGravity = true;
            raycastScript.objArrasta.transform.SetParent(null);
            raycastScript.objArrasta.transform.localPosition = new Vector3(0, 0, 3);
        }

        void Pegar()
        {
            Destroy(raycastScript.objPega);
        }
    }
}

