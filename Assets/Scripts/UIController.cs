using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
	public Slider HappySlider;
	public TMP_Text TimerText;
	public GameObject StartPanel;
	public GameObject EndPanel;
	public Sprite ParticleNeutral;
	public Sprite ParticleAngry;
	public Sprite ParticleSad;
	public Sprite ParticleHappy;
	public Sprite ParticleClap;
	public Sprite ParticleCry;
	public Sprite ParticleIll;
	public Sprite ParticlePlay;
	public Sprite ParticlePoop;

	public Sprite FullStar;

	public Image Emoji;
	public Image FirstStar;
	public Image SecondStar;
	public Image ThirdStar;

	public float SliderSpeed;

	private ParticleSystem _lastUsedParticle;
	private float RequestedSliderNum;
	private float Happyiness;
	private int MinutesLeft;
	private int _collectedStars;
	private bool _needToEncreaseSlider;
	private bool _needToDecreaseSlider;
	public void SetSlider(float amount)
	{
		RequestedSliderNum = amount;
		if (amount > HappySlider.value)
			_needToEncreaseSlider = true;
		else
			_needToDecreaseSlider = true;
		Happyiness = amount;
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
		if (Happyiness > 75)
		{
			Invoke(nameof(AddStar), 0.5f);
			Invoke(nameof(AddStar), 1f);
			Invoke(nameof(AddStar), 1.5f);
		}
		if (Happyiness > 50 && Happyiness < 75)
		{
			Invoke(nameof(AddStar), 0.5f);
			Invoke(nameof(AddStar), 1f);
		}
		if (Happyiness > 25 && Happyiness < 50)
		{
			Invoke(nameof(AddStar), 0.5f);
		}
		if (Happyiness < 25)
		{

		}
	}
	private void EncreaseSlider()
	{
		HappySlider.value += SliderSpeed;
		if (HappySlider.value >= RequestedSliderNum)
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
	public void SetEmoji(BabyState emotion, BabyState State)
	{
		if(State == BabyState.Neutral)
		{
			if (emotion == BabyState.Neutral)
			{
				Emoji.sprite = ParticleNeutral;
			}
			if (emotion == BabyState.Happy)
			{
				Emoji.sprite = ParticleIll;
			}
			if (State == BabyState.Angry)
			{
				Emoji.sprite = ParticleAngry;
			}
			if (State == BabyState.Upset)
			{
				Emoji.sprite = ParticleSad;
			}
		}
		else
		{
			if(State == BabyState.Pooped)
			{
				Emoji.sprite = ParticlePoop;
			}
			if(State == BabyState.Ill)
			{
				Emoji.sprite = ParticleHappy;
			}
		}/*
		switch (type)
		{
			case EmojiType.neutral:
				Emoji.sprite = ParticleNeutral;
				break;
			case EmojiType.happy:
				Emoji.sprite = ParticleHappy;
				break;
			case EmojiType.angry:
				Emoji.sprite = ParticleAngry;
				break;
			case EmojiType.sad:
				Emoji.sprite = ParticleSad;
				break;
			case EmojiType.cry:
				Emoji.sprite = ParticleCry;
				break;
			case EmojiType.ill:
				Emoji.sprite = ParticleIll;
				break;
			case EmojiType.play:
				Emoji.sprite = ParticlePlay;
				break;
			case EmojiType.clap:
				Emoji.sprite = ParticleClap;
				break;
			case EmojiType.poop:
				Emoji.sprite = ParticlePoop;
				break;
		}*/
	}
	private void AddStar()
	{
		switch (_collectedStars)
		{
			case 0:
				FirstStar.sprite = FullStar;
				break;
			case 1:

				SecondStar.sprite = FullStar;
				break;
			case 2:

				ThirdStar.sprite = FullStar;
				break;
		}
		_collectedStars++;
	}
}
public enum EmojiType
{
	neutral = 0,
	happy = 1,
	angry = 2,
	sad = 3,
	cry = 4,
	ill = 5,
	play = 6,
	clap = 7,
	poop = 8,
	heal = 9
}
