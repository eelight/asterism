using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public interface IStar {

	BoolReactiveProperty Connecting { get; }
	
}

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class Star : MonoBehaviour, IStar {

	private LineRenderer lineRenderer;

	// Start is called before the first frame update
	void Awake() {
		lineRenderer = GetComponent<LineRenderer>();
		hitPointCache = transform.position;
	}

	private bool callUpdate;
	private Vector3 cachePos;

	[SerializeField]
	private float delayTime = 1f;

	[SerializeField]
	private float speed = 100f;

	public float time;

	private Vector3 hitPointCache;

	// Update is called once per frame
	public void UpdateView(Vector3 inDir, Vector3 normal, Vector3 hitPoint) {

		if (callUpdate) return;
		callUpdate = true;
		hitPointCache = hitPoint;
		var dir = Vector3.Reflect(inDir, normal);
		Ray ray = new Ray(hitPoint, dir);

		Debug.DrawRay(ray.origin, ray.direction * 100, Color.white);
        
		if (Physics.Raycast(ray, out RaycastHit hit, 100)) {

			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, hitPoint);
			cachePos = Vector3.Lerp(cachePos, hit.point, speed * Time.deltaTime);
			lineRenderer.SetPosition(1, cachePos);

			if (Vector3.Distance(cachePos, hit.point) < 0.1f) {
				var star = hit.collider.GetComponent<Star>();
				if (star != null) star.UpdateView(ray.direction, hit.normal, hit.point);	
			}
		}
        
	}

	private void LateUpdate() {
		if (callUpdate) {
			Connecting.Value = true;
			callUpdate = false;
			return;
		}

		time += Time.deltaTime;
		if (time >= delayTime) {
			Connecting.Value = false;
			time = 0f;
			cachePos = hitPointCache;
			lineRenderer.positionCount = 0;
			
		}
	}

	public BoolReactiveProperty Connecting { get; } = new BoolReactiveProperty();

}