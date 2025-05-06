using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Z
{
	#region dot
	public static float dot(Vector3 a, Vector3 b)
	{
		return a.x * b.x + a.y * b.y + a.z * b.z;
	} 
	#endregion
}

public class INPUT
{
	#region MOUSE
	public static class M
	{
		public static Camera cam;
		// up = new vec3(0, 0, +1)
		public static Vector3 getPos3D
		{
			get
			{
				// plane: (r - o).up = 0
				Vector3 up = Vector3.forward;
				Vector3 o = Vector3.zero;

				Ray ray = cam.ScreenPointToRay(Input.mousePosition);

				// line: a + n * L
				Vector3 a = ray.origin;
				Vector3 n = ray.direction;

				float L = -Z.dot(a - o, up) / Z.dot(n, up);
				return a + n * L;
			}
		}
		public static bool InstantDown(int mouse_btn_type = 0)
		{
			return Input.GetMouseButtonDown(mouse_btn_type);
		}
		public static bool Down(int mouse_btn_type = 0)
		{
			return Input.GetMouseButtonDown(mouse_btn_type);
		}
		public static bool InstantUp(int mouse_btn_type = 0)
		{
			return Input.GetMouseButtonUp(mouse_btn_type);
		}
	}
	#endregion

	#region K
	public static class K
	{
		public static bool InstantDown(KeyCode keyCode)
		{
			return Input.GetKeyDown(keyCode);
		}
		public static bool Down(KeyCode keyCode)
		{
			return Input.GetKey(keyCode);
		}
		public static bool InstantUp(KeyCode keyCode)
		{
			return Input.GetKeyUp(keyCode);
		}
	}
	#endregion

	#region UI
	public static class UI
	{
		// is move pointer over (any UI gameobject) / (UI EventSystem) ?
		public static bool Hover
		{
			get { return EventSystem.current.IsPointerOverGameObject(); }
		}

		public static RectTransform CanvasRectTransform;
		public static Vector2 pos
		{
			// return 1280, 720 regardless of canvas scale provided same ratio
			get { return Input.mousePosition / CanvasRectTransform.localScale.x; }
		}
	}
	#endregion
}

#region U
public static class U
{
	// converted to lowercase before check
	public static Transform NameStartsWith(this Transform transform, string name)
	{
		for (int i0 = 0; i0 < transform.childCount; i0 += 1)
			if (transform.GetChild(i0).name.ToLower().StartsWith(name.ToLower()))
				return transform.GetChild(i0);
		Debug.LogError($"found no leaf starting with that name: {name.ToLower()}, under transform: {transform.name}");
		return null;
	}
	public static GameObject NameStartsWith(this GameObject gameObject, string name)
	{
		Transform transform = gameObject.transform;
		for (int i0 = 0; i0 < transform.childCount; i0 += 1)
			if (transform.GetChild(i0).name.ToLower().StartsWith(name.ToLower()))
				return transform.GetChild(i0).gameObject;
		Debug.LogError($"found no leaf starting with that name: {name.ToLower()}, under transform: {transform.name}");
		return null;
	}

	// CanPlaceBuilding.... pos2D, gameObject with boxCollider2D  
	public static bool CanPlaceObject(Vector2 pos2D, GameObject gameObject)
	{
		Collider2D collider = gameObject.GetComponent<Collider2D>();

		if(collider is BoxCollider2D)
		{
			BoxCollider2D boxCollider2D = (BoxCollider2D)collider;
			Collider2D[] COLLIDER = Physics2D.OverlapBoxAll(pos2D + boxCollider2D.offset, boxCollider2D.size, angle: 0f);
			return COLLIDER.Length == 0;
		}
		else if (collider is CircleCollider2D)
		{
			CircleCollider2D circleCollider2D = (CircleCollider2D)collider;
			Collider2D[] COLLIDER = Physics2D.OverlapCircleAll(pos2D + circleCollider2D.offset, circleCollider2D.radius);
			return COLLIDER.Length == 0;
		}
		else if(collider is CapsuleCollider2D)
		{
			CapsuleCollider2D capsuleCollider2D = (CapsuleCollider2D)collider;
			Collider2D[] COLLIDER = Physics2D.OverlapCapsuleAll(pos2D + capsuleCollider2D.offset, capsuleCollider2D.size, capsuleCollider2D.direction, angle: 0f);
			return COLLIDER.Length == 0;
		}
		//
		Debug.LogError($"no collider attached to {gameObject.name} at {gameObject.transform.position}");
		return true;
	}

	// ad
	#region ad
	public static string AbrrevatedNumber(int value)
	{
		// Define scales
		Dictionary<long, string> scales = new Dictionary<long, string>
		{
			{1_000_000_000_000, "T"},
			{1_000_000_000,     "B"},
			{1_000_000,         "M"},
			{1_000,             "K"}
		};

		// Numbers below the smallest scale are unchanged
		if (value < 1_000)
			return value.ToString();

		// Find the largest applicable scale
		foreach (long threshold in scales.Keys.OrderByDescending(k => k))
		{
			if (value >= threshold)
			{
				double scaled = (double)value / threshold;
				double truncated = Math.Floor(scaled * 10) / 10;  // one decimal, always down :contentReference[oaicite:7]{index=7}

				// If the number part is 20 or more, drop decimals
				if (truncated >= 10)
					return $"{(int)truncated}{scales[threshold]}";

				// Otherwise, show one decimal (e.g. 1.1k, 19.9k)
				return $"{truncated:0.#}{scales[threshold]}";
			}
		}
		// default
		return value.ToString();
	}

	public static string RoundDecimal(float val, int digits = 2)
	{
		float new_val = (int)(val * Mathf.Pow(10, digits)) / (Mathf.Pow(10, digits));
		return new_val.ToString();
	}
	#endregion

	public static float Clamp(this float x, float min, float max)
	{
		if (x > max) return max;
		if (x < min) return min;
		return x;
	}
}
#endregion