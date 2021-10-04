using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
	public Slider HappySlider;
	public TMP_Text TimerText;
	public GameObject StartPanel;
	public GameObject EndPanel;
	public ParticleSystem ParticleNeutral;
	public ParticleSystem ParticleAngry;
	public ParticleSystem ParticleSad;
	public ParticleSystem ParticleHappy;
	public ParticleSystem ParticleClap;
	public ParticleSystem ParticleCry;
	public ParticleSystem ParticleIll;
	public ParticleSystem ParticlePlay;
	public ParticleSystem ParticlePoop;

	public Sprite FullStar;

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
	private void Start()
	{
		StopEmojis();
	}
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
	private void StopEmojis()
	{
		ParticleNeutral.Stop();
		ParticleHappy.Stop();
		ParticleAngry.Stop();
		ParticleSad.Stop();
		ParticleCry.Stop();
		ParticleIll.Stop();
		ParticlePlay.Stop();
		ParticlePoop.Stop();
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
	public void SetEmojiParticles(EmojiType type)
	{
		try
		{
			_lastUsedParticle.Stop();
		}
		catch { }
		switch (type)
		{
			case EmojiType.neutral:
				ParticleNeutral.Play();
				_lastUsedParticle = ParticleNeutral;
				break;
			case EmojiType.happy:
				ParticleHappy.Play();
				_lastUsedParticle = ParticleHappy;
				break;
			case EmojiType.angry:
				ParticleAngry.Play();
				_lastUsedParticle = ParticleAngry;
				break;
			case EmojiType.sad:
				ParticleSad.Play();
				_lastUsedParticle = ParticleSad;
				break;
			case EmojiType.cry:
				ParticleCry.Play();
				_lastUsedParticle = ParticleCry;
				break;
			case EmojiType.ill:
				ParticleIll.Play();
				_lastUsedParticle = ParticleIll;
				break;
			case EmojiType.play:
				ParticlePlay.Play();
				_lastUsedParticle = ParticlePlay;
				break;
			case EmojiType.clap:
				ParticleClap.Play();
				_lastUsedParticle = ParticleClap;
				break;
			case EmojiType.poop:
				ParticlePoop.Play();
				_lastUsedParticle = ParticlePoop;
				break;
		}
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
