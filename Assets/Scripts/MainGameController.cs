using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameController : MonoBehaviour
{
	public float TimeForOneGame;

	private UIController _uiController;
	private InputController _inputController;
	[SerializeField] private float _timeleft;
	private bool _needToCountTime;
	private void Start()
	{
		_uiController = GetComponent<UIController>();
		_inputController = GetComponent<InputController>();
	}
	public void StartGame()
	{
		_inputController.StartInput();
		_uiController.OpenGameWindow();
		StartTimer();
	}
	public void EndGame()
	{
		_inputController.StopInput();
		_uiController.OpenEndGameWindow();
	}
	private void StartTimer()
	{
		_timeleft = TimeForOneGame;
		_needToCountTime = true;
	}
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
		if (_timeleft <= 0)
		{
			_needToCountTime = false;
			EndGame();
		}
	}
}
