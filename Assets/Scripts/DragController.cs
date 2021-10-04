using UnityEngine;

public class DragController : MonoBehaviour
{
	public Transform HoldPositionTransform;
	public Transform PlayerTransform;
	public float ThrowForce;

	private BabyController _babyController;
	private Rigidbody _itemRigidbody;
	private Transform _itemTransform;
	private DragAbleObject _item;
	[SerializeField]private bool _isHolding;

	private void Start()
	{
		_babyController = GetComponent<BabyController>();
	}

	public void TakeItem(Transform itemTransform)
	{
		if (!_isHolding)
		{
			_isHolding = true;
			_itemTransform = itemTransform;
			_item = _itemTransform.GetComponent<DragAbleObject>();
			_itemRigidbody = _itemTransform.GetComponent<Rigidbody>();
			_itemRigidbody.isKinematic = true;
			_itemRigidbody.useGravity = false;
			_itemTransform.parent = HoldPositionTransform;
			_itemTransform.localPosition = Vector3.zero;
			_itemTransform.gameObject.layer = 7;
			if(_item.ItemType == ItemType.Diper && _item.IsDiaperOn)
			{
				_babyController.TakeAwayDiaper();
			}
		}
	}
	public void ReleaseItem()
	{
		if (_isHolding)
		{
			_isHolding = false;
			_itemTransform.parent = null;
			_itemRigidbody.isKinematic = false;
			_itemRigidbody.useGravity = true;
			_itemTransform.gameObject.layer = 0;
		}
	}
	public void ThrowItem()
	{
		if (_isHolding)
		{
			_isHolding = false;
			_itemTransform.parent = null;
			_itemRigidbody.isKinematic = false;
			_itemRigidbody.useGravity = true;
			_itemTransform.gameObject.layer = 0;
			_itemRigidbody.AddForce(PlayerTransform.forward * ThrowForce);
			AudioManger.Current.MakeEffectSound(EffectSound.Throw);
		}
	}
	public bool TryUseOnBaby()
	{
		if (_isHolding)
		{
			if (_item.ItemType == ItemType.NotUsable)
				return false;
			else
			{
				if (_babyController.TryUseItem(_item))
				{
					_isHolding = false;
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		else
		{
			return false;
		}
	}
}
