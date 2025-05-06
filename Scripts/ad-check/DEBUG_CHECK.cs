using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPACE_LOG;

public class DEBUG_CHECK : MonoBehaviour
{
	public ScriptableObject SO;
	event EventHandler _subscribeChannel;

	private void Start()
	{
		//_subscribe += _subscribe_audience;
		if (SubscriberTrack.Add(this.transform)) // returns false if already present
			_subscribeChannel += (o, e) => log_somthng();
		else
			Debug.Log("Already subscribed 0");

		if (SubscriberTrack.Add(this.transform)) // returns false if already present
			_subscribeChannel += (o, e) => log_somthng();
		else
			Debug.Log("Already subscribed 1");

		//this._subscriber_log_another = (o, e) => log_another();
		_subscribeChannel += this._subscriber_log_another;
	}
	HashSet<Transform> SubscriberTrack = new HashSet<Transform>();


	#region previously
	/*
	void _subscribe_audience(object sender, EventArgs e)
	{
		//throw new NotImplementedException();
		log_somthng();
	}
	*/
	#endregion
	void log_somthng()
	{
		Debug.Log("somthng--subscribed to when mouse Instant Down(0) occur");
	}
	// store lamda in variable, if do need to unSubsribe in Future
	void _subscriber_log_another(object o,EventArgs e) => log_another();


	void log_another()
	{
		Debug.Log("another--subscribed to when mouse Instant Down(0) occur");
	}



	public float radius = 5f;

	private void Update()
	{
		if(INPUT.M.Down(0))
		{
			// Reserve null only for truly static events, where no instance context exists.
			// check if _subsribe != nulls
			if(_subscribeChannel != null) // there are subsribers
			{
				// study >>
				_subscribeChannel.Invoke(this, EventArgs.Empty);
				// << study
			}
		}

		if(INPUT.K.InstantDown(KeyCode.R))
		{
			// study >>
			//_subscribeChannel -= (o, e) => log_another();
			_subscribeChannel -= _subscriber_log_another;
			Debug.Log("log_another() has been unSubscribed()");
			// << study
		}
	}

	private void OnDisable()
	{
		Debug.Log("check--OnDisable");
	}

	public bool NeedGizmos = false;
	private void OnDrawGizmos()
	{
		if (NeedGizmos == true)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(this.transform.position, this.radius);
		}
	}
}
