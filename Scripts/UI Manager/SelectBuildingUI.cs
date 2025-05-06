using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SPACE_LOG;

public class SelectBuildingUI : MonoBehaviour
{
	[SerializeField] List<SO_BuildingType> SO_IgnoreBuildingTypes;

	Dictionary<SO_BuildingType, GameObject> MAP_UI;
	#region just for cursor
	[SerializeField] Sprite sprite_cursor;
	GameObject obj_clone; 
	#endregion

	private void Awake()
	{
		
	}

	void INITIALIZE_SelectBuildingUI()
	{
		List<SO_BuildingType> _LIST = Resources.Load<SO_BuildingTypeList>(typeof(SO_BuildingTypeList).Name)._LIST;

		this.MAP_UI = new Dictionary<SO_BuildingType, GameObject>();
		GameObject template = this.gameObject.NameStartsWith("Template");
		template.SetActive(false);

		#region just for cursor
		if(true)
		{
			// do not include this in dictionary
			obj_clone = GameObject.Instantiate(template, this.transform);
			obj_clone.SetActive(true);
			obj_clone.NameStartsWith("img").GetComponent<Image>().sprite = this.sprite_cursor;
			obj_clone.GetComponent<Button>().onClick.AddListener(() =>
			{
				//
				BuildingManager.set_ActiveBuildingType(null);
				showSelectedOutline();
				//
			});

			// ====================== SUBSCRIBE ======================== //
			obj_clone.GetComponent<MouseEnterExistUI>().subscribeChannel_MouseEnter += (o, e) => ToolTipUI.Show("Exit Building Building Placing Mode", time: 3f);
			obj_clone.GetComponent<MouseEnterExistUI>().subscribeChannel_MouseExit += (o, e) => ToolTipUI.Hide();
		}
		#endregion

		foreach (SO_BuildingType buildingType in _LIST)
		{
			if (SO_IgnoreBuildingTypes.Contains(buildingType) == true)
				continue;

			GameObject clone = GameObject.Instantiate(template, this.transform);
			clone.SetActive(true);
			this.MAP_UI[buildingType] = clone;

			clone.NameStartsWith("img").GetComponent<Image>().sprite = buildingType._sprite;
			clone.GetComponent<Button>().onClick.AddListener(() =>
			{
				//
				BuildingManager.set_ActiveBuildingType(buildingType);
				//showSelectedOutline();
				//
			});

			// ====================== SUBSCRIBE ======================== //
			clone.GetComponent<MouseEnterExistUI>().subscribeChannel_MouseEnter += (o, e) => ToolTipUI.Show(buildingType.get_resource_cost_list_text, 5f);
			clone.GetComponent<MouseEnterExistUI>().subscribeChannel_MouseExit += (o, e) => ToolTipUI.Hide();
		}
	}

	private void Start()
	{
		// always after Awake, if += subsribe included
		INITIALIZE_SelectBuildingUI();

		// at start based on BuildingType
		showSelectedOutline();

		// subscribe only after Awake
		// ====================== SUBSCRIBE ======================== //
		BuildingManager.subscribeChannel_ActiveBuildingChanged += (o, e) => showSelectedOutline();
	}

	void showSelectedOutline()
	{
		SO_BuildingType ActiveBuildingType = BuildingManager.get_ActiveBuildingType();

		#region just for cursor
		// true only when currently active BuildingType = null
		this.obj_clone.NameStartsWith("select").SetActive(ActiveBuildingType == null);
		#endregion
		// if currently active BuildingType Match with Any than, show the outline
		// else disable ot
		foreach (var kvp in this.MAP_UI) // key value pair approach
		{
			if (ActiveBuildingType == kvp.Key)
				kvp.Value.NameStartsWith("select").SetActive(true);
			else
				kvp.Value.NameStartsWith("select").SetActive(false);
		}
	}

}
