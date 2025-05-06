using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceNode : MonoBehaviour
{
	[SerializeField] int ResourceCount;

	SO_ResourceNodeType _SO_ResourceNodeType;
	private void Awake()
	{
		_SO_ResourceNodeType =  (SO_ResourceNodeType)(this.gameObject.GetComponent<SO_ref>().SO);
		this.ResourceCount = _SO_ResourceNodeType._start_count;
	}

	public SO_ResourceType get_ResourceType { get { return _SO_ResourceNodeType._so_resource_type; } }
	public void deduct()
	{
		this.ResourceCount -= 1;
		if(this.ResourceCount == 0)
		{
			GameObject.Destroy(this.gameObject); // destroy before calling to subscribers since they do, Collision Check
			// the instance of ResourceNode atached to this.gameObject shall be gone after this frame complete

			//_subscribeChannel_ResourceZero?. // if there are any subscribers
			//	Invoke(this, EventArgs.Empty);

			/* previously Incorrect, subscribe called before destroy, require async function to suceed, 
			// >> else each Building shall have null ResourceNode
			_subscribeChannel_ResourceZero?. // if there are any subscribers
					Invoke(this, EventArgs.Empty);
			GameObject.Destroy(this.gameObject);
			*/
		}
		int _subscribersCount = this._subscribeChannel_ResourceZero?.GetInvocationList().Length ?? 0;
		Debug.Log(this.transform.position.ToString() + ": " + _subscribersCount);
	}

	private void OnDisable()
	{
		// gameObject Might Be still preset completely Removed before its called
		//_subscribeChannel_ResourceZero?. // if there are any subscribers
		//		Invoke(this, EventArgs.Empty);
		Debug.Log("GameObject OnDisable");
	}

	/*
		OnDestroy runs after the final frame update of an object’s 
		life—guaranteed to fire once per destruction sequence 

		Because OnDisable is called before the object is fully torn down—and because
		Unity does not guarantee the relative order of OnDisable across different 
		scripts or GameObjects—you can’t reliably assume your subscribers still exist 
		or are still subscribed at that moment 
	*/
	private void OnDestroy()
	{
		// ensure this gameOnject completely Removed before its called, SO that no more detection from Collider
		_subscribeChannel_ResourceZero?. // if there are any subscribers
				Invoke(this, EventArgs.Empty);
		Debug.Log("GameObject OnDestroy");
	}

	// ====================== SUBSCRIBE ======================== //
	public event EventHandler _subscribeChannel_ResourceZero;

}
