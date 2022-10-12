using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderDisplay : MonoBehaviour
{
    public Slider slider;
      [SerializeField]
    public TextMeshProUGUI TMPtext;

    public bool Whole_Number = true;

    // Update is called once per frame
    void Update()
    {
        if(Whole_Number){
          TMPtext.text = slider.value.ToString("#");
        }else{
          TMPtext.text = slider.value.ToString("#.###");
        }
        
    }
}
