using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BASA
{
    public class UIMangers : MonoBehaviour
    {
        public Slider sliderHP, sliderStamina;
        public Movimento scriptMov;
        public Text municao;
        public Image imgModoTiro;
        public Sprite[] spriteModoTiro;
        public RectTransform mira;

        void Start()
        {
            scriptMov = GameObject.FindWithTag("Player").GetComponent<Movimento>();
            municao.enabled = true;
            imgModoTiro.enabled = true;
        }


        void Update()
        {
            sliderHP.value = scriptMov.hp;
            sliderStamina.value = scriptMov.stamina;
        }
    }

}
