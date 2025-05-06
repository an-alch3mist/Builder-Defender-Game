using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
	Dictionary<SO_ResourceType, TMPro.TextMeshProUGUI> MAP_TM_ResCount;
	SO_ResourceTypeList _SO_ResourceTypeList;
	void INITIALIZE_ResourceUI()
	{
		_SO_ResourceTypeList = Resources.Load<SO_ResourceTypeList>(typeof(SO_ResourceTypeList).Name);

		//GameObject template = transform.Find("Template_ResourceType").gameObject;
		Transform template = transform.Find("Template_ResourceType");
		template.gameObject.SetActive(false);
		MAP_TM_ResCount = new Dictionary<SO_ResourceType, TMPro.TextMeshProUGUI>();
		foreach (SO_ResourceType resourceType in _SO_ResourceTypeList._LIST)
		{
			Transform template_clone = Instantiate(template, transform);
			template_clone.gameObject.SetActive(true);

			template_clone.Find("img").GetComponent<Image>().sprite = resourceType._sprite;
			//Debug.Log(template_clone.Find("text").GetComponent<TMPro.TextMeshProUGUI>());
			MAP_TM_ResCount[resourceType] = template_clone.Find("text").GetComponent<TMPro.TextMeshProUGUI>();
		}
	}

	private void Awake()
	{
		INITIALIZE_ResourceUI();
	}

	private void Start()
	{
		// dependent on ResourceManager MAP_ResourceCount
		// ====================== SUBSCRIBE ======================== //
		ResourceManager._subscribeChannel_WhenResourceCountAltered += (o, e) => UpdateResourceCount();
	}

	void UpdateResourceCount()
	{
		foreach (SO_ResourceType resourceType in _SO_ResourceTypeList._LIST)
		{
			int count = ResourceManager.GetResourceCount(resourceType);
			MAP_TM_ResCount[resourceType].text = U.AbrrevatedNumber(count);
		}
	}
}
