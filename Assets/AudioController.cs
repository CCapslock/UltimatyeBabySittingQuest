using UnityEngine;

public class AudioController : MonoBehaviour
{
	public AudioSource Effects;
	public AudioSource Baby;

	public AudioClip Take;
	public AudioClip Throw;
	public AudioClip Use;
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