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

    // Update is called once per frame
    void Update()
    {
        TMPtext.text = slider.value.ToString("#");
    }
}
