using UnityEngine;
using NaughtyAttributes;

public class BabyController : MonoBehaviour
{
	public GameObject Baby;
	public Transform Diaper;
	public Transform Pelvis;
	public Renderer BabyBody;
	public Renderer BabyHead;
	public Material BabyHealthMaterial;
	public Material BabyIllMaterial;
	public float MinTimeBeforeAction;
	public float MaxTimeBeforeAction;
	public float CryUppsetNumber;
	public float PoopUppsetNumber;
	public float IllnessNegativePoints;
	public Vector3 ThrowAwayVector;

	[Foldout("Animation Parameters")]
	public float EatingAnimationLenght;
	[Foldout("Animation Parameters")]
	public float TimeBeforeFoodDisapear;
	[Foldout("Animation Parameters")]
	public float TimeBeforeGoodToyDisapear;
	[Foldout("Animation Parameters")]
	public float TimeBeforeBadToyThrow;
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
	[SerializeField] private BabyState _currentState;
	[SerializeField] private BabyState _lastEmotionState;

	//eatingStuff
	private Animator _babyAnimator;
	private Transform _itemTransform;
	private Rigidbody _itemRigidbody;
	private DragAbleObject _lastUsedItem;

	//diaperStuff
	private Vector3 _diaperOnPelvisPosition;
	private Quaternion _diaperOnPelvisRotation;

	[SerializeField] private bool _isBusy; private int _randomNum;
	private bool _isDiaperOn = true;

	private void Start()
	{
		_babyAnimator = Baby.GetComponent<Animator>();
		_uiController = GetComponent<UIController>();
		_diaperOnPelvisPosition = Diaper.localPosition;
		_diaperOnPelvisRotation = Diaper.rotation;
		MakecorrectEmoji();
		_lastEmotionState = BabyState.Neutral;
	}
	public void StartTheBaby()
	{
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
	public void TakeAwayDiaper()
	{
		_isDiaperOn = false;
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
					MakeUpsetAction(UpsetActions.Poop);
					break;
				case 2:
					MakeUpsetAction(UpsetActions.Ill);
					break;
			}
		}
		else
		{
			WaitBeforeUpsetAction();
		}
	}
	private void TurnOffCry()
	{
		if (_currentState == BabyState.Cry)
			_currentState = BabyState.Neutral;
		MakecorrectEmoji();
	}
	private void MakeUpsetAction(UpsetActions action)
	{
		switch (action)
		{
			case UpsetActions.Cry:
				_babyAnimator.SetTrigger("Cry");
				HappyMeter -= CryUppsetNumber;
				_currentState = BabyState.Cry;
				TurnOfBusy();
				Invoke(nameof(TurnOffCry), 2f);
				break;
			case UpsetActions.Poop:
				_babyAnimator.SetTrigger("Poop");
				Diaper.GetComponent<DragAbleObject>().MakeDiaperDirty();
				ParticlesManager.Current.StartStinkyCloud();
				_currentState = BabyState.Pooped;
				break;
			case UpsetActions.Ill:
				_babyAnimator.SetTrigger("Ill");
				_currentState = BabyState.Ill;
				BecomeIll();
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
			_randomNum = Random.Range(1, 100);
			_isBusy = true;
			switch (item.ItemType)
			{
				case ItemType.Food:
					if (_randomNum <= item.ChanceOfRandomAction)
					{
						PlayWithItem(item);
					}
					else
					{
						EatItem(item);
					}
					break;
				case ItemType.Toy:
					if (_randomNum <= item.ChanceOfRandomAction)
					{
						EatItem(item);
					}
					else
					{
						PlayWithItem(item);
					}
					break;
				case ItemType.Diper:
					if (_currentState == BabyState.Pooped && !_isDiaperOn)
					{
						WearDiaper(item);
						item.WearDiaper();
					}
					else
					{
						if ((_randomNum <= item.ChanceOfRandomAction))
						{
							EatItem(item);
						}
						else
						{
							Debug.Log("Throw");
							FindObjectOfType<DragController>().ReleaseItem();
						}
					}
					_isBusy = false;
					break;
				case ItemType.Medicine:
					_lastUsedItem = item;
					_itemTransform = item.transform;
					MakeMedicineFeedBack();
					DestroyObject();
					break;
			}
			MakecorrectEmoji();
			return true;
		}
		else
		{
			switch (item.ItemType)
			{
				case ItemType.Diper:
					if (_currentState == BabyState.Pooped && !_isDiaperOn)
					{
						MakecorrectEmoji();
						WearDiaper(item);
						item.WearDiaper();
					}
					return true;
				case ItemType.Medicine:
					_lastUsedItem = item;
					_itemTransform = item.transform;
					MakecorrectEmoji();
					MakeMedicineFeedBack();
					DestroyObject();
					return true;
			}
			return false;
		}
	}
	private void WearDiaper(DragAbleObject item)
	{
		_isDiaperOn = true;
		Diaper = item.transform;
		_itemRigidbody = Diaper.GetComponent<Rigidbody>();
		_itemRigidbody.isKinematic = true;
		_itemRigidbody.useGravity = false;
		Diaper.parent = Pelvis;
		Diaper.localPosition = _diaperOnPelvisPosition;
		Diaper.rotation = _diaperOnPelvisRotation;
		Diaper.gameObject.layer = 0;
		item.IsDiaperOn = true;
		if (_currentState == BabyState.Pooped && !item.IsDiaperDirty)
		{
			_currentState = BabyState.Neutral;
			ParticlesManager.Current.StopStinkyCloud();
			_babyAnimator.SetTrigger("Clap");
			Invoke(nameof(TurnOfBusy), ClapAnimationLenght);
		}
		MakecorrectEmoji();
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
		Invoke(nameof(MakeMedicineFeedBack), EatingAnimationLenght);
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
	private void BecomeIll()
	{
		Material[] mats = BabyBody.sharedMaterials;
		mats[0] = BabyIllMaterial;
		BabyBody.sharedMaterials = mats;
		mats = BabyHead.sharedMaterials;
		mats[0] = BabyIllMaterial;
		BabyHead.sharedMaterials = mats;
	}
	private void BecomeHealthy()
	{
		Material[] mats = BabyBody.sharedMaterials;
		mats[0] = BabyHealthMaterial;
		BabyBody.sharedMaterials = mats;
		mats = BabyHead.sharedMaterials;
		mats[0] = BabyHealthMaterial;
		BabyHead.sharedMaterials = mats;
	}
	private void PlayWithItem(DragAbleObject item)
	{
		int num = Random.Range(1, 100);
		if (num <= item.ChanceOfSuccesToy)
		{
			//goodresult
			HappyMeter += item.PositivePointsIfPlay;
			_lastUsedItem = item;
			_babyAnimator.SetTrigger("GoodPlay");
			_itemTransform = item.transform;
			_itemRigidbody = _itemTransform.GetComponent<Rigidbody>();
			_itemRigidbody.isKinematic = true;
			_itemRigidbody.useGravity = false;
			_itemTransform.parent = TransformForItem;
			_itemTransform.localPosition = Vector3.zero;
			_itemTransform.gameObject.layer = 0;
			Invoke(nameof(DestroyObject), TimeBeforeGoodToyDisapear);
		}
		else
		{
			//badresult
			HappyMeter -= item.NegativePointsIfPlay;
			_lastUsedItem = item;
			_babyAnimator.SetTrigger("BadPlay");
			_itemTransform = item.transform;
			_itemRigidbody = _itemTransform.GetComponent<Rigidbody>();
			_itemRigidbody.isKinematic = true;
			_itemRigidbody.useGravity = false;
			_itemTransform.parent = TransformForItem;
			_itemTransform.localPosition = Vector3.zero;
			_itemTransform.gameObject.layer = 0;
			Invoke(nameof(ThrowAwayItem), TimeBeforeBadToyThrow);
		}

		_uiController.SetSlider(HappyMeter);
		TurnOfBusy();
		Invoke(nameof(MakecorrectEmoji), 1f);
	}
	private void ThrowAwayItem()
	{
		_itemRigidbody.isKinematic = false;
		_itemRigidbody.useGravity = true;
		_itemTransform.parent = null;
		_itemRigidbody.AddForce(ThrowAwayVector * 1000f);
	}
	private void MakeFoodFeedBack()
	{
		CalculateFoodFeedback(_lastUsedItem.ChanceOfSuccesFood, _lastUsedItem.PositivePointsIfEat, _lastUsedItem.NegativePointsIfEat);
	}
	private void MakeMedicineFeedBack()
	{
		if (_currentState == BabyState.Ill)
		{
			_currentState = BabyState.Neutral;
			HappyMeter += _lastUsedItem.NegativePointsIfEat;
			_babyAnimator.SetTrigger("GoodFood");
			BecomeHealthy();
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
		Invoke(nameof(MakecorrectEmoji), 1f);
		TurnOfBusy();
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
		TurnOfBusy();
		DecideImpression();
		Invoke(nameof(MakecorrectEmoji), 1f);
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
			_lastEmotionState = BabyState.Happy;
			_babyAnimator.SetBool("Happy", true);
			MakecorrectEmoji();
		}
		else if (HappyMeter >= 50 && HappyMeter < 75)
		{
			_lastEmotionState = BabyState.Neutral;
			_babyAnimator.SetBool("Neutral", true);
			MakecorrectEmoji();
		}
		else if (HappyMeter >= 25 && HappyMeter < 50)
		{
			_lastEmotionState = BabyState.Angry;
			_babyAnimator.SetBool("Angry", true);
			MakecorrectEmoji();
		}
		else if (HappyMeter >= 0 && HappyMeter < 25)
		{
			_lastEmotionState = BabyState.Upset;
			_babyAnimator.SetBool("Sad", true);
			MakecorrectEmoji();
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
	private void MakecorrectEmoji()
	{
		_uiController.SetEmoji(_lastEmotionState, _currentState);
	}
}
public enum BabyState
{
	Neutral = 0,
	Happy = 1,
	Upset = 2,
	Angry = 3,
	Ill = 4,
	Pooped = 5,
	Cry = 6
}
public enum UpsetActions
{
	Cry = 0,
	Poop = 1,
	Ill = 2
}
