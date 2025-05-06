using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPACE_LOG;
using System.Linq;
using System.Threading.Tasks;

public class ResourceGenerator : MonoBehaviour
{
	float time;
	SO_BuildingType _SO;
	private void Awake() 
	{
		// SO_ref >>
		this._SO = (SO_BuildingType)(gameObject.GetComponent<SO_ref>().SO);
		// << SO_ref
		this.time = 0f;

	}

	private void Start()
	{
		// call Update_ResourceNodes for this Building at the Start
		// for HQ and other building that is placed in Non_Game Mode, hence its called after Awake
		// since it depend on _SO_ResourceNodeType recived by getResourceType() in ResourceNode which is initialized in its awake
		this.Update_ResourceNodes();
	}

	List<ResourceNode> ResourceNode_1D;
	//async void Update_ResourceNodes() // if gameObject.Destroy() is called after subscribe
	async void Update_ResourceNodes()
	{
		this.ResourceNode_1D = new List<ResourceNode>();
		//Debug.Log("initialize: " + ResourceNode_1D.Count);

		// wait for a while since ResourceNode gameobject is destroyed just now
		//await Task.Delay(20);

		Collider2D[] COLLIDER = Physics2D.OverlapCircleAll(point: this.transform.position, radius: _SO._resource_generator_data._radius_reach);
		foreach(Collider2D collider in COLLIDER)
		{
			ResourceNode _ResourceNode = collider.gameObject.GetComponent<ResourceNode>();
			if(_ResourceNode != null)
				if(_ResourceNode.get_ResourceType == _SO._resource_generator_data._SO_ResourceType)
				{
					ResourceNode_1D.Add(_ResourceNode);
					// ====================== SUBSCRIBE ======================== //
					if (SubscriberTrack.Add(_ResourceNode) == true) // able to Add? or if ResourceNode doesn't exist before
						_ResourceNode._subscribeChannel_ResourceZero += this._subscribeRef;
				}
		}
		//
		//console.log_txt(ResourceNode_1D.Select(node => node.transform.position).toTable("L<> node pos3D"));
		//Debug.Log("found: " + this.ResourceNode_1D.Count);
	}

	// ====================== SUBSCRIBE ======================== //
	HashSet<ResourceNode> SubscriberTrack = new HashSet<ResourceNode>();
	void _subscribeRef(object o, EventArgs e) => this.Update_ResourceNodes();
	private void OnDestroy()
	{
		// could be disable or destrpyed since the Unsubscription from ResourceNode Does'nt Depend on this.collider
		foreach (ResourceNode _node in this.SubscriberTrack)
			_node._subscribeChannel_ResourceZero -= this._subscribeRef;
	}

	private void Update()
	{
		time += Time.deltaTime;
		if(time > _SO._resource_generator_data._time_interval)
		{
			if(ResourceNode_1D.Count > 0)
			{
				this.ResourceNode_1D[0].deduct();

				ResourceManager.AddResource(
					_SO_ResourceType: _SO._resource_generator_data._SO_ResourceType, 
					count: 1);
			}
			time = 0f;
			// Debug.Log(ResourceManager.log_MAP());
		}
	}

	public string get_CountPerSecond() { return U.RoundDecimal(1f / _SO._resource_generator_data._time_interval, 2) + $" {this._SO._resource_generator_data._SO_ResourceType._name.ToLower()}/s"; }
	public float get_Progress() { return this.time / _SO._resource_generator_data._time_interval; }
	public Sprite get_ResourceTypeSprite() { return this._SO._resource_generator_data._SO_ResourceType._sprite; }

	public bool NeedGizmos = false;
	private void OnDrawGizmos()
	{
		if(NeedGizmos == true)
		{
			Gizmos.color = Color.red;
			// for Gizmos in Editor without Awake
			this._SO = (SO_BuildingType)(gameObject.GetComponent<SO_ref>().SO);
			Gizmos.DrawWireSphere(this.transform.position, this._SO._resource_generator_data._radius_reach);
		}
	}
}
