using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public Transform PlayerTransform;
	public Transform CameraTransform;
	public Vector3 CameraCrawlPosition;
	public float Speed;
	public float CrawlSpeed;
	public float CameraSpeedToCrawl;

	private CharacterController _characterController;
	private Vector3 _playerMovementVector;
	private Vector3 _playerVelocityVector;
	private Vector3 _cameraStartPosition;
	private Vector3 _cameraDestanationPosition;
	private bool _needToMoveCamera;
	private bool _isCrawl;


	private void Start()
	{
		_characterController = FindObjectOfType<CharacterController>();
		_playerMovementVector = new Vector3();
		_playerVelocityVector = new Vector3();
		_cameraStartPosition = CameraTransform.localPosition;
	}
	private void FixedUpdate()
	{
		if (_needToMoveCamera)
		{
			MoveCamera();
		}
	}
	private void MoveCamera()
	{
		CameraTransform.localPosition = Vector3.MoveTowards(CameraTransform.localPosition, _cameraDestanationPosition, CameraSpeedToCrawl);
		if (CameraTransform.localPosition == _cameraDestanationPosition)
			_needToMoveCamera = false;
	}
	public void CameraLook(Vector3 cameraRotation, Vector3 playerRotation)
	{
		CameraTransform.localRotation = Quaternion.Euler(cameraRotation);
		PlayerTransform.Rotate(playerRotation);
	}
	public void MovePlayer(float xInput, float zInput)
	{
		_playerMovementVector = PlayerTransform.right * xInput + PlayerTransform.forward * zInput;
		_playerVelocityVector.y += -9.81f * Time.deltaTime;
		if (_isCrawl)
			_characterController.Move(_playerMovementVector * CrawlSpeed * Time.deltaTime);
		else
			_characterController.Move(_playerMovementVector * Speed * Time.deltaTime);

		_characterController.Move(_playerVelocityVector * Time.deltaTime);
	}
	public void StartCrawl()
	{
		_cameraDestanationPosition = CameraCrawlPosition;
		_needToMoveCamera = true;
		_isCrawl = true;
	}
	public void StopCrawl()
	{
		_cameraDestanationPosition = _cameraStartPosition;
		_needToMoveCamera = true;
		_isCrawl = false;
	}
}
