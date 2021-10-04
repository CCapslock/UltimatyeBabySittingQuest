using UnityEngine;

public class AudioManger : MonoBehaviour
{
	[HideInInspector] public static AudioManger Current;
	public AudioSource music;
	public AudioSource Baby;

	public AudioClip Take;
	public AudioClip Throw;
	public AudioClip Use;

	private void Start()
	{
		Current = this;
	}
	public void MakeEffectSound(EffectSound effect)
	{
		switch (effect)
		{
			case EffectSound.Take:
				break;
			case EffectSound.Throw:
				music.PlayOneShot(Throw);
				break;
			case EffectSound.Use:
				break;
		}
	}
	public void MakeBabySound(BabySound effect)
	{
		switch (effect)
		{
			case BabySound.Cry:
				break;
			case BabySound.Laugh:
				break;
			case BabySound.Puke:
				break;
		}
		Baby.Play();
	}
}

public enum EffectSound
{
	Take = 0,
	Throw = 1,
	Use = 2
}
public enum BabySound
{
	Cry = 0,
	Laugh = 1,
	Puke = 2
}