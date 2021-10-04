using UnityEngine;

public class Water : MonoBehaviour
{
	private DragAbleObject _item;
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(TagManager.GetTag(TagType.DragAble))) 
		{
			_item = other.GetComponent<DragAbleObject>();
			if (_item.ItemType == ItemType.Diper) 
			{
				_item.CleanDiaper();
			}
		}
	}
}
