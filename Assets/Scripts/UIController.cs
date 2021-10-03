using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public Slider HappySlider;
    public TMP_Text TimerText;
    public GameObject StartPanel;
    public GameObject EndPanel;
    public float SliderSpeed;

	private float RequestedSliderNum;
	private int MinutesLeft;
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

	public void OpenGameWindow()
	{
		StartPanel.SetActive(false);
	}
	public void OpenEndGameWindow()
	{
		EndPanel.SetActive(true);
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
	public void SetTime(int seconds)
	{
		MinutesLeft = (seconds / 60);
		TimerText.text = (MinutesLeft + " : " + (seconds - MinutesLeft * 60));
	}
}
