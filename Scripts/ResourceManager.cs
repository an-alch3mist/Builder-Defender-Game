using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPACE_LOG;

public class ResourceManager : MonoBehaviour
{
	static Dictionary<SO_ResourceType, int> MAP_ResourceCount;
	public static void INITIALIZE_ResourceManager()
	{
		SO_ResourceTypeList _SO_ResourceTypeList = Resources.Load<SO_ResourceTypeList>(typeof(SO_ResourceTypeList).Name);

		ResourceManager.MAP_ResourceCount = new Dictionary<SO_ResourceType, int>();
		foreach (SO_ResourceType resourceType in _SO_ResourceTypeList._LIST)
			ResourceManager.MAP_ResourceCount[resourceType] = 0;
	}

	private void Awake()
	{
		// independent
		INITIALIZE_ResourceManager();
	}

	private void Update()
	{

	}

	// called externally when required
	#region event subsribe approach
	public static event EventHandler _subscribeChannel_WhenResourceCountAltered;
	#endregion
	public static void AddResource(SO_ResourceType _SO_ResourceType, int count)
	{
		ResourceManager.MAP_ResourceCount[_SO_ResourceType] += count;


		_subscribeChannel_WhenResourceCountAltered? // check there are subsribers, otherwise error
			.Invoke(null, EventArgs.Empty);
	}
	public static int GetResourceCount(SO_ResourceType _SO_ResourceType)
	{
		return ResourceManager.MAP_ResourceCount[_SO_ResourceType];
	}

	// ad
	#region ad Log__MAP_ResourceCount<>
	public static string log_MAP()
	{
		return MAP_ResourceCount.toTable();
	} 
	#endregion


	public static bool CanAfford(ResourceCost_Data[] CostList)
	{
		foreach(ResourceCost_Data _data in CostList)
		{
			// cannot afford
			if (MAP_ResourceCount[_data._SO_ResourceType] < _data._amount)
				return false;
		}
		// can afford
		return true;
	}

	public static void SpendResources(ResourceCost_Data[] CostList)
	{
		foreach(ResourceCost_Data _data in CostList)
			MAP_ResourceCount[_data._SO_ResourceType] -= _data._amount;

		// ====================== SUBSCRIBE ======================== //
		// resource count has been altered, notify the subscribers
		_subscribeChannel_WhenResourceCountAltered? // check there are subsribers, otherwise error
			.Invoke(null, EventArgs.Empty);
	}
}