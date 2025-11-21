using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] private float hpValue = 0;
    [SerializeField] private Slider slider;

    public void UpdateHP(float hp)
    {
        hpValue = hp;
        slider.value = hpValue;
    }
}
