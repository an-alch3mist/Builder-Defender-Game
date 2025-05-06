using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] Canvas MainCanvas;
	private void Awake()
	{
		INPUT.UI.CanvasRectTransform = this.MainCanvas.GetComponent<RectTransform>();
	}
}
