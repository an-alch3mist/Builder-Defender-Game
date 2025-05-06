using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/SO_ResourceType")]
public class SO_ResourceType : ScriptableObject
{
	public string _name;
	public string _short_name;
	public int _count;
	public Sprite _sprite;
}
