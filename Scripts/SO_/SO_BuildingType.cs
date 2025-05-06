using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SO_BuildingType")]
public class SO_BuildingType : ScriptableObject
{
	public string _name;
	public Sprite _sprite;
	public GameObject _prefab;
	public ResourceGenerator_Data _resource_generator_data;
	public ResourceCost_Data[] _resource_cost_list;

	#region get_resource_cost_list_text for ToolTip
	public string get_resource_cost_list_text
	{
		get
		{
			string str = "";
			str = $"{_name}\n";
			foreach (ResourceCost_Data _data in this._resource_cost_list)
				str += $"{_data._SO_ResourceType._short_name}: {_data._amount}\n";
			return str;
		}
	} 
	#endregion
}

[System.Serializable]
public class ResourceGenerator_Data
{
	public float _time_interval = 1f;
	public SO_ResourceType _SO_ResourceType;
	public float _radius_reach = 5f;
}


[System.Serializable]
public class ResourceCost_Data
{
	public SO_ResourceType _SO_ResourceType;
	public int _amount;
}