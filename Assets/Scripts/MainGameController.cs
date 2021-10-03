using UnityEngine;

public class MainGameController : MonoBehaviour
{
	public float TimeForOneGame;

	private UIController _uiController;
	[SerializeField] private float _timeleft;
	private bool _needToCountTime;
	private void Start()
	{
		_uiController = GetComponent<UIController>();
		StartGame();
	}
	private void StartGame()
	{
		StartTimer();
	}
	private void StartTimer()
	{
		_timeleft = TimeForOneGame;
		_needToCountTime = true;
	}
	private void Update()
	{
		if (_needToCountTime)
		{
			CountTime();
		}
	}
	private void CountTime()
	{
		_timeleft -= Time.deltaTime;
		_uiController.SetTime((int)_timeleft);
		if(_timeleft <= 0)
		{
			_needToCountTime = false;
		}
	}
}
