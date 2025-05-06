using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SO_ResourceNodeType", order = 10)]
public class SO_ResourceNodeType : ScriptableObject
{
	public string _name;
	public SO_ResourceType _so_resource_type;
	[Range(1, 100000)] public int _start_count;
}
