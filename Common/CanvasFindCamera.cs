using UnityEngine;

namespace Asterism.Common {

    [RequireComponent(typeof(Canvas))]
    public class CanvasFindCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Awake() {
            
            GetComponent<Canvas>().worldCamera = Camera.main;
            
        }
        
    }

}
