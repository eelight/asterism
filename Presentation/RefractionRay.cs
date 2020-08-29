using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class RefractionRay : MonoBehaviour {

	private LineRenderer lineRenderer;

	// Start is called before the first frame update
	void Awake() {
		lineRenderer = GetComponent<LineRenderer>();
		cachePos = transform.position;
	}

	private bool callUpdate;
	private Vector3 cachePos;

	// Update is called once per frame
	public void UpdateView() {

		callUpdate = true;
		Ray ray = new Ray(transform.position, transform.up);

		Debug.DrawRay(ray.origin, ray.direction * 100, Color.white);
        
		if (Physics.Raycast(ray, out RaycastHit hit, 100)) {

			lineRenderer.positionCount = 2;
			lineRenderer.SetPosition(0, transform.position);  
			cachePos = Vector3.Lerp(cachePos, hit.point, 2f * Time.deltaTime);
			lineRenderer.SetPosition(1, hit.point);

			// if (Vector3.Distance(cachePos, hit.point) < 0.1f) {
				var star = hit.collider.GetComponent<Star>();
				if (star != null) star.UpdateView(ray.direction, hit.normal, hit.point);	
			// }
		}
        
	}

	private void LateUpdate() {
		if (callUpdate) {
			callUpdate = false;
			return;
		}
		
		cachePos = transform.position;
		lineRenderer.positionCount = 0;
	}

}

