using Prime31.TransitionKit;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Asterism.Common {

	public class ToTitleButton : MonoBehaviour {

		private bool done;
		
		public void OnClick() {
			if (done) return;
			done = true;
			
			var wind = new WindTransition()
			{
				nextScene = DEFINE.TITLE,
				duration = 1.0f,
				size = 0.3f
			};
			TransitionKit.instance.transitionWithDelegate( wind );
		}

	}
	

}