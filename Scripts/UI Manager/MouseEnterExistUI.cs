using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MouseEnterExistUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	// ====================== SUBSCRIBE ======================== //
	public event EventHandler subscribeChannel_MouseEnter;
	public event EventHandler subscribeChannel_MouseExit;

	public void OnPointerEnter(PointerEventData eventData)
	{
		//Debug.Log("Entered: "+ this.gameObject.name);
		subscribeChannel_MouseEnter? // if there are any subscribers
			.Invoke(this, EventArgs.Empty);
		//throw new System.NotImplementedException();
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		//Debug.Log("Exited: " + this.gameObject.name);
		subscribeChannel_MouseExit? // if there are any subscribers
			.Invoke(this, EventArgs.Empty);
		//throw new System.NotImplementedException();
	}
}
