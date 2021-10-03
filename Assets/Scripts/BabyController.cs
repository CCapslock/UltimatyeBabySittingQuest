using UnityEngine;
using NaughtyAttributes;

public class BabyController : MonoBehaviour
{
	public GameObject Baby;
	public Transform Diaper;
	public Transform Pelvis;
	public float MinTimeBeforeAction;
	public float MaxTimeBeforeAction;
	public float CryUppsetNumber;
	public float PoopUppsetNumber;
	public float IllnessNegativePoints;

	[Foldout("Animation Parameters")]
	public float EatingAnimationLenght;
	[Foldout("Animation Parameters")]
	public float TimeBeforeFoodDisapear;
	[Foldout("Animation Parameters")]
	public float VomitingAnimationLenght;
	[Foldout("Animation Parameters")]
	public float CryAnimationLenght;
	[Foldout("Animation Parameters")]
	public float ClapAnimationLenght;
	[Foldout("Animation Parameters")]
	public float PoopAnimationLenght;
	[Foldout("Animation Parameters")]
	public float TimeBeforeStartVomiting;
	[Foldout("Animation Parameters")]
	public Transform TransformForItem;

	public float HappyMeter = 60f;

	private UIController _uiController;
	private BabyState _currentState;

	//eatingStuff
	private Animator _babyAnimator;
	private Transform _itemTransform;
	private Rigidbody _itemRigidbody;
	private DragAbleObject _lastUsedItem;

	//diaperStuff
	private Vector3 _diaperOnPelvisPosition;
	private Quaternion _diaperOnPelvisRotation;

	[SerializeField] private bool _isBusy;

	private void Start()
	{
		_babyAnimator = Baby.GetComponent<Animator>();
		_uiController = GetComponent<UIController>();
		_diaperOnPelvisPosition = Diaper.localPosition;
		_diaperOnPelvisRotation = Diaper.rotation;
		WaitBeforeUpsetAction();
		DecideImpression();
		_uiController.SetSlider(HappyMeter);
	}
	private void FixedUpdate()
	{
		if (_currentState == BabyState.Ill || _currentState == BabyState.Pooped)
		{
			LoseHappines();
		}
	}
	private void LoseHappines()
	{
		HappyMeter -= IllnessNegativePoints;
		_uiController.SetSlider(HappyMeter);
		DecideImpression();
	}
	private void WaitBeforeUpsetAction()
	{
		Invoke(nameof(SelectAction), Random.Range(MinTimeBeforeAction, MaxTimeBeforeAction));
	}
	private void SelectAction()
	{
		if (!_isBusy)
		{
			_isBusy = true;
			int num = Random.Range(0, ((int)UpsetActions.Ill) + 1);
			switch (num)
			{
				case 0:
					MakeUpsetAction(UpsetActions.Cry);
					break;
				case 1:
					if (_currentState != BabyState.Pooped)
					{
						MakeUpsetAction(UpsetActions.Poop);
					}
					else
					{
						MakeUpsetAction(UpsetActions.Cry);
					}
					break;
				case 2:
					if (_currentState != BabyState.Ill)
					{
						MakeUpsetAction(UpsetActions.Ill);
					}
					else
					{
						MakeUpsetAction(UpsetActions.Cry);
					}
					break;
			}
		}
		else
		{
			WaitBeforeUpsetAction();
		}
	}
	private void MakeUpsetAction(UpsetActions action)
	{
		switch (action)
		{
			case UpsetActions.Cry:
				_babyAnimator.SetTrigger("Cry");
				HappyMeter -= CryUppsetNumber;
				TurnOfBusy();
				break;
			case UpsetActions.Poop:
				_babyAnimator.SetTrigger("Poop");
				Diaper.GetComponent<DragAbleObject>().MakeDiaperDirty();
				ParticlesManager.Current.StartStinkyCloud();
				_currentState = BabyState.Pooped;
				break;
			case UpsetActions.Ill:
				_isBusy = false;
				//_babyAnimator.SetTrigger("Ill");
				//_currentState = BabyState.Ill;
				break;
		}
		DecideImpression();
		_uiController.SetSlider(HappyMeter);
		WaitBeforeUpsetAction();
	}
	private void TurnOfBusy()
	{
		_isBusy = false;
	}
	public bool TryUseItem(DragAbleObject item)
	{
		if (!_isBusy)
		{
			switch (item.ItemType)
			{
				case ItemType.Food:
					EatItem(item);
					break;
				case ItemType.Toy:
					PlayWithItem(item);
					break;
				case ItemType.Diper:
					WearDiaper(item);
					break;
				case ItemType.Medicine:
					_currentState = BabyState.Neutral;
					break;
			}
			return true;
		}
		else
		{
			switch (item.ItemType)
			{
				case ItemType.Diper:
					if (_currentState == BabyState.Pooped)
						WearDiaper(item);
					break;
				case ItemType.Medicine:
					if (_currentState == BabyState.Ill)
						_currentState = BabyState.Neutral;
					break;
			}
			return false;
		}
	}
	private void WearDiaper(DragAbleObject item)
	{
		Debug.Log("Wear");
		Diaper = item.transform;
		_itemRigidbody = Diaper.GetComponent<Rigidbody>();
		_itemRigidbody.isKinematic = true;
		_itemRigidbody.useGravity = false;
		Diaper.parent = Pelvis;
		Diaper.localPosition = _diaperOnPelvisPosition;
		Diaper.rotation = _diaperOnPelvisRotation;
		Diaper.gameObject.layer = 0;
		if (_currentState == BabyState.Pooped && !item.IsDiaperDirty)
		{
			_currentState = BabyState.Neutral;
			ParticlesManager.Current.StopStinkyCloud();
			_babyAnimator.SetTrigger("Clap");
			Invoke(nameof(TurnOfBusy), ClapAnimationLenght);
		}
	}
	private void EatMedicine(DragAbleObject item)
	{
		_lastUsedItem = item;
		_babyAnimator.SetTrigger("Eat");
		_itemTransform = item.transform;
		_itemRigidbody = _itemTransform.GetComponent<Rigidbody>();
		_itemRigidbody.isKinematic = true;
		_itemRigidbody.useGravity = false;
		_itemTransform.parent = TransformForItem;
		_itemTransform.localPosition = Vector3.zero;
		_itemTransform.gameObject.layer = 0;
		Invoke(nameof(DestroyObject), TimeBeforeFoodDisapear);
		Invoke(nameof(MakeFoodFeedBack), EatingAnimationLenght);
	}
	private void EatItem(DragAbleObject item)
	{
		_lastUsedItem = item;
		_babyAnimator.SetTrigger("Eat");
		_itemTransform = item.transform;
		_itemRigidbody = _itemTransform.GetComponent<Rigidbody>();
		_itemRigidbody.isKinematic = true;
		_itemRigidbody.useGravity = false;
		_itemTransform.parent = TransformForItem;
		_itemTransform.localPosition = Vector3.zero;
		_itemTransform.gameObject.layer = 0;
		Invoke(nameof(DestroyObject), TimeBeforeFoodDisapear);
		Invoke(nameof(MakeFoodFeedBack), EatingAnimationLenght);
	}
	private void PlayWithItem(DragAbleObject item)
	{
		_lastUsedItem = item;
		_babyAnimator.SetTrigger("Play");
		_itemTransform = item.transform;
		_itemRigidbody = _itemTransform.GetComponent<Rigidbody>();
		_itemRigidbody.isKinematic = true;
		_itemRigidbody.useGravity = false;
		_itemTransform.parent = TransformForItem;
		_itemTransform.localPosition = Vector3.zero;
		_itemTransform.gameObject.layer = 0;
	}
	private void MakeFoodFeedBack()
	{
		CalculateFoodFeedback(_lastUsedItem.ChanceOfSucces, _lastUsedItem.PositivePointsIfEat, _lastUsedItem.NegativePointsIfEat);
	}
	private void MakeMedicineFeedBack()
	{
		if (_currentState == BabyState.Ill)
		{
			_currentState = BabyState.Neutral;
			HappyMeter -= _lastUsedItem.NegativePointsIfEat;
			_babyAnimator.SetTrigger("GoodFood");
			Invoke(nameof(TurnOfBusy), ClapAnimationLenght);
		}
		else
		{
			HappyMeter -= _lastUsedItem.NegativePointsIfEat;
			_babyAnimator.SetTrigger("BadFood");
			Invoke(nameof(StartVomiting), TimeBeforeStartVomiting);
			Invoke(nameof(StopVomiting), VomitingAnimationLenght);
		}
		DecideImpression();
		_uiController.SetSlider(HappyMeter);
	}
	private void DestroyObject()
	{
		Destroy(_itemTransform.gameObject);
	}
	private void StartVomiting()
	{
		ParticlesManager.Current.StartVomiting();
	}
	private void StopVomiting()
	{
		ParticlesManager.Current.StopVomiting();
	}
	private void CalculateFoodFeedback(int succesChance, float positivePoints, float negativePoints)
	{
		int num = Random.Range(1, 100);
		if (num <= succesChance)
		{
			//goodresult
			HappyMeter += positivePoints;
			_babyAnimator.SetTrigger("GoodFood");
		}
		else
		{
			//badresult
			HappyMeter -= positivePoints;
			_babyAnimator.SetTrigger("BadFood");
			Invoke(nameof(StartVomiting), TimeBeforeStartVomiting);
			Invoke(nameof(StopVomiting), VomitingAnimationLenght);
		}
		DecideImpression();
		_uiController.SetSlider(HappyMeter);
	}
	private void DecideImpression()
	{
		_babyAnimator.SetBool("Happy", false);
		_babyAnimator.SetBool("Sad", false);
		_babyAnimator.SetBool("Angry", false);
		_babyAnimator.SetBool("Neutral", false);
		if (HappyMeter >= 75)
		{
			_babyAnimator.SetBool("Happy", true);
		}
		else if (HappyMeter >= 50 && HappyMeter < 75)
		{
			_babyAnimator.SetBool("Neutral", true);
		}
		else if (HappyMeter >= 25 && HappyMeter < 50)
		{
			_babyAnimator.SetBool("Angry", true);
		}
		else if (HappyMeter >= 0 && HappyMeter < 25)
		{
			_babyAnimator.SetBool("Sad", true);
		}
		else if (HappyMeter <= 0)
		{
			//проиграл
		}
		else if (HappyMeter >= 100)
		{
			//выиграл
		}
	}
}
public enum BabyState
{
	Neutral = 0,
	Happy = 1,
	Upset = 2,
	Angry = 3,
	Ill = 4,
	Pooped = 5
}
public enum UpsetActions
{
	Cry = 0,
	Poop = 1,
	Ill = 2
}
