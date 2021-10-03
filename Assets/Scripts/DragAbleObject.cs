using UnityEngine;

public class DragAbleObject : MonoBehaviour
{
	public ParticleSystem StinkyCloud;
	public Material BasicMaterial;
	public Material OutlinedMaterial;
	public ItemType ItemType;
	public Renderer ItemRenderer;
	public float PositivePointsIfEat;
	public float NegativePointsIfEat;
	public float PositivePointsIfPlay;
	public float NegativePointsIfPlay;
	public int ChanceOfSucces;
	public int ChanceOfRandomAction = 5;
	public bool IsDiaperDirty;

	private Collider _itemCollider;
	private Rigidbody _itemRigidbody;
	private void Start()
	{
		ItemRenderer = GetComponent<Renderer>();
		if (StinkyCloud != null)
		{
			StinkyCloud.gameObject.SetActive(false);
		}
		if (ItemType == ItemType.Diper)
		{
			_itemCollider = GetComponent<Collider>();
			_itemRigidbody = GetComponent<Rigidbody>();
			if (_itemRigidbody.useGravity == false)
			{
				_itemCollider.enabled = false;
			}
		}
	}
	public void WearDiaper()
	{
		_itemCollider.enabled = false;
	}
	public void MakeDiaperDirty()
	{
		_itemCollider.enabled = true;
		IsDiaperDirty = true;
		StinkyCloud.gameObject.SetActive(true);
		StinkyCloud.Play();
	}
}
