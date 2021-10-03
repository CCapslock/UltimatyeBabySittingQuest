using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    [HideInInspector] public static ParticlesManager Current;


    public ParticleSystem Vomit;
    public ParticleSystem StinkyCloud;
    private void Awake()
    {
        Current = this;
        StopVomiting();
        StopStinkyCloud();
    }
	public void StartVomiting()
	{
        Vomit.Play();
    }
	public void StopVomiting()
	{
        Vomit.Stop();
    }
    public void StartStinkyCloud()
	{
        StinkyCloud.Play();
    }
	public void StopStinkyCloud()
	{
        StinkyCloud.Stop();
    }
}
