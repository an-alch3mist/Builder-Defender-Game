using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	//SO_BuildingTypeList _SO_BuildingTypeList;
	private void Awake()
	{
		// independent
		//_SO_BuildingTypeList = Resources.Load<SO_BuildingTypeList>(typeof(SO_BuildingTypeList).Name);
		ActiveBuildingType = null;
	}

	private void Start()
	{
		// dependent: might have ran Camera static class as initialization
		INPUT.M.cam = Camera.main;
	}

	private void Update()
	{
		//Debug.Log(INPUT.M.getPos3D);
		if (INPUT.M.InstantDown(0) && INPUT.UI.Hover == false)
		{
			// TODO get wood/store harvester from list based on .... fetch via (enum/string) instead of int
			/*
			if (INPUT.K.Down(KeyCode.Alpha0))
				Object.Instantiate(_SO_BuildingTypeList._LIST[0]._prefab, INPUT.M.getPos3D, Quaternion.identity);
			if(INPUT.K.Down(KeyCode.Alpha1))
				Object.Instantiate(_SO_BuildingTypeList._LIST[1]._prefab, INPUT.M.getPos3D, Quaternion.identity);
			*/
			if (ActiveBuildingType != null)
			{
				// place building if no intersection with 
				if (ResourceManager.CanAfford(ActiveBuildingType._resource_cost_list) == true) // can afford
				{
					if (U.CanPlaceObject(INPUT.M.getPos3D, ActiveBuildingType._prefab) == true) // has space
					{
						GameObject.Instantiate(ActiveBuildingType._prefab, INPUT.M.getPos3D, Quaternion.identity);
						ResourceManager.SpendResources(ActiveBuildingType._resource_cost_list);
					}
					else
						ToolTipUI.Show("Building Require A Free Space To Construct", 3f);
				}
				else
					ToolTipUI.Show("Fetch More Resource To Afford This Building", 3f);
			}
		}
	}

	static SO_BuildingType ActiveBuildingType;
	public static SO_BuildingType get_ActiveBuildingType() { return BuildingManager.ActiveBuildingType; }
	public static void set_ActiveBuildingType(SO_BuildingType _SO_BuildingType)
	{
		BuildingManager.ActiveBuildingType = _SO_BuildingType;
		// ====================== SUBSCRIBE ======================== //
		BuildingManager.subscribeChannel_ActiveBuildingChanged? // channel not empty
				.Invoke(null, EventArgs.Empty);
	}
	// ====================== SUBSCRIBE ======================== //
	// subscribe to avoid Class Coupling tightly
	// make sure subsribed audience to a Channel =order of call= does not affect outcome
	public static event EventHandler subscribeChannel_ActiveBuildingChanged;

}