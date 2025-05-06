using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortingOrder : MonoBehaviour
{
	private void Awake()
	{
		SortSprite();
		if (this.OnlyRunWhenCreated)
			Destroy(this);
		// componenet is being removed instantly, 
		// MonoBehaviour is Undiscoverable instantly after this Destroy()
		// however all the method inside are still executed until everything done
	}


	private void Update()
	{
		SortSprite();
	}

	[SerializeField] bool OnlyRunWhenCreated = true;
	static int precision = 5;

	[SerializeField] bool useOffsetY = false;
	[SerializeField] float offsetY = 0f;
	public void SortSprite()
	{
		if(useOffsetY == false)
		 this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(this.transform.position.y * precision);
		else
		{
			SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
			sr.sortingOrder = -(int)((this.transform.position.y + offsetY) * precision);
		}
	}

	[SerializeField] bool DrawInEditMode = false;
	private void OnDrawGizmos()
	{
		if (DrawInEditMode == true)
			SortSprite();
	}
}
