using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider HappySlider;
    public float SliderSpeed;

	private float RequestedSliderNum;
	private bool _needToEncreaseSlider;
	private bool _needToDecreaseSlider;

    public void SetSlider(float amount)
	{
		RequestedSliderNum = amount;
		if (amount > HappySlider.value)
			_needToEncreaseSlider = true;
		else
			_needToDecreaseSlider = true;
	}
	private void FixedUpdate()
	{
		if (_needToEncreaseSlider)
			EncreaseSlider();
		if (_needToDecreaseSlider)
			DecreaseSlider();
	}

	private void EncreaseSlider()
	{
		HappySlider.value += SliderSpeed;
		if(HappySlider.value >= RequestedSliderNum)
		{
			_needToEncreaseSlider = false;
			HappySlider.value = RequestedSliderNum;
		}
	}
	private void DecreaseSlider()
	{
		HappySlider.value -= SliderSpeed;
		if (HappySlider.value <= RequestedSliderNum)
		{
			_needToDecreaseSlider = false;
			HappySlider.value = RequestedSliderNum;
		}
	}
}
