using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Asterism.Presentation {

	[RequireComponent(typeof(Camera))]
	public class FindOverlayCamera : MonoBehaviour {

		private void Awake() {
			var cam = gameObject.GetComponent<Camera>();

			var kit = GameObject.Find("TransitionKit");
			if (kit == null) return;
		
			var myOverlayCamera = kit.GetComponent<Camera>();
		
			var cameraData = cam.GetUniversalAdditionalCameraData();
			cameraData.cameraStack.Add(myOverlayCamera);

		}

	}

}