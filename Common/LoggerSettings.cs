using Zenject;

namespace Asterism.Common {

	public class LoggerSettings : IInitializable {

		public void Initialize() {
			#if RELEASE && !UNITY_EDITOR
			UnityEngine.Debug.unityLogger.logEnabled = false;
			#endif
		}

	}

}