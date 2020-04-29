using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEfeitos : MonoBehaviour
{
    public float tempo = 0;

    void Start()
    {
        Destroy(gameObject, tempo);
    }

}
