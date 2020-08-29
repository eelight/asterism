using System.Collections;
using System.Collections.Generic;
using Asterism;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class RayTest : MonoBehaviour {

    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
        cachePos = transform.position;
    }

    private Vector3 cachePos;
    
    // Update is called once per frame
    void Update() {
        
        Ray ray = new Ray(transform.position, transform.up);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.white);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100, DEFINE.LASER_MASK)) {

            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            cachePos = Vector3.Lerp(cachePos, hit.point, 2f * Time.deltaTime);
            lineRenderer.SetPosition(1, hit.point);

            if (hit.collider.CompareTag($"Refraction")) {
                var refraction = hit.collider.GetComponent<RefractionRay>();
                if (refraction != null) refraction.UpdateView();
            }

        }
        
    }
}
