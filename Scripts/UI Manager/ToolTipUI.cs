using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ToolTipUI : MonoBehaviour
{
	static GameObject _gameObject;
	static RectTransform ToolTipRectTransform;
	static TextMeshProUGUI text_TM;
	static RectTransform back_RectTransform;

	/*
	ToolTipUI
		back
		text
	*/

	/*
		Make sure Following: 
		- Anchor, pivot of all ToolTipUI, back, text set to: bottom-left
		- text alignMent Bottom Aligned, Left/Center/Right Justify
		 RayCast Disabled in both back, text to avoid Enter Exit UI EventSystem Glitch
	*/

	static int pad = 16;
	static float border;
	private void Awake()
	{
		// make sure both tm, back is anchored bottom-left, woth -8 offset x, and tm alllignment vertical set to middle
		ToolTipUI._gameObject = this.gameObject;
		ToolTipUI.ToolTipRectTransform = this.gameObject.GetComponent<RectTransform>();
		ToolTipUI.text_TM = this.gameObject.NameStartsWith("text").GetComponent<TextMeshProUGUI>();
		ToolTipUI.back_RectTransform = this.gameObject.NameStartsWith("back").GetComponent<RectTransform>();

		Outline OutlineComponent = this.gameObject.NameStartsWith("back").GetComponent<Outline>();
		ToolTipUI.border = (OutlineComponent != null) ? OutlineComponent.effectDistance.x : 0f;
		// set padding offset transform to text, not the background
		ToolTipUI.text_TM.rectTransform.localPosition += new Vector3(+pad / 2, +pad / 2); // push toward left and up by pad/2
	}

	private void Start()
	{
		// depend on UIManager to Initialize INPUT.UI.CanvasRectTransform
		ToolTipUI.Show("New Text Has Been Set\nline: 1\nline: 2");
		ToolTipUI.Hide(); // hide at begining
	}

	private void Update()
	{
		ToolTipUI.SetPos(INPUT.UI.pos);

		// Hide Tool Tip after _time sec
		ToolTipUI._time -= Time.deltaTime;
		if (ToolTipUI._time < 0f)
			ToolTipUI.Hide();
	}

	static void SetText(string str)
	{
		text_TM.text = str;

		text_TM.ForceMeshUpdate();
		Vector2 textSize = text_TM.GetRenderedValues(onlyVisibleCharacters: false); // size with entire text
		back_RectTransform.sizeDelta = textSize + new Vector2(pad, pad); // size with respect to anchor
	}

	static void SetPos(Vector2 pos)
	{
		Vector2 AnchoredPos = INPUT.UI.pos;
		// clamp >>
		AnchoredPos.x = AnchoredPos.x.Clamp( 0f + border, INPUT.UI.CanvasRectTransform.rect.width - back_RectTransform.rect.width - border);
		AnchoredPos.y = AnchoredPos.y.Clamp( 0f + border, INPUT.UI.CanvasRectTransform.rect.height - back_RectTransform.rect.height - border);
		// << clamp
		// make sure pivot of ToolTipREctTransform is bottom left
		ToolTipRectTransform.anchoredPosition = AnchoredPos;
	}

	static float _time = 0f;
	public static void Show(string str, float time = 30f)
	{
		_time = time;
		Debug.Log("ToolTip.Show()");
		_gameObject.SetActive(true);
		SetText(str);
		SetPos(INPUT.UI.pos);
	}

	public static void Hide()
	{
		_gameObject.SetActive(false);
	}
}
