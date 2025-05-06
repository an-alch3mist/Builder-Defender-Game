using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGeneratorOverlay : MonoBehaviour
{
	[SerializeField]ResourceGenerator _ResourceGenerator;

	private void Awake()
	{
		
	}

	private void Start()
	{
		// depend on _SO in ResourceGenerator
		this.gameObject.NameStartsWith("icon").GetComponent<SpriteRenderer>().sprite = this._ResourceGenerator.get_ResourceTypeSprite();
		this.gameObject.NameStartsWith("stats").GetComponent<TMPro.TextMeshPro>().text = this._ResourceGenerator.get_CountPerSecond();
	}

	private void Update()
	{
		this.gameObject.NameStartsWith("progress").transform.localScale = new Vector3(this._ResourceGenerator.get_Progress(), 1f, 1f);
	}
}
