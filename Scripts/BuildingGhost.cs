using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
	private void Awake()
	{
	}

	[SerializeField] Color TrueTint;
	[SerializeField] Color FalseTint;

	private void Start()
	{
		ModifyGhost(); // based on begining BuildingType
		// always subscribe after Awake
		// ====================== SUBSCRIBE ======================== //
		BuildingManager.subscribeChannel_ActiveBuildingChanged += (o, e) => ModifyGhost();
	}

	private void Update()
	{
		this.transform.position = INPUT.M.getPos3D;
		// change color
		SO_BuildingType ActiveBuilding = BuildingManager.get_ActiveBuildingType();
		bool canPlaceBuilding = U.CanPlaceObject(this.transform.position, ActiveBuilding._prefab);
		bool canAffordBuilding = ResourceManager.CanAfford(ActiveBuilding._resource_cost_list);

		this.gameObject.NameStartsWith("sprite").GetComponent<SpriteRenderer>().color = (canPlaceBuilding && canAffordBuilding) ? this.TrueTint : FalseTint;
	}

	void ModifyGhost()
	{
		SO_BuildingType _SO_BuildingType = BuildingManager.get_ActiveBuildingType();
		if(_SO_BuildingType != null)
		{
			this.gameObject.NameStartsWith("sprite").GetComponent<SpriteRenderer>().sprite = _SO_BuildingType._sprite;
			this.gameObject.SetActive(true);
		}
		else
			this.gameObject.SetActive(false);
	}
}
