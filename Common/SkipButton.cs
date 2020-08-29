using Asterism;
using Asterism.Domain.UseCases;
using Asterism.Tutorial;
using Prime31.TransitionKit;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SkipButton : MonoBehaviour {

	[Inject]
	private IStageInfo stageInfo;

	[Inject]
	private ISaveData saveData;

	// Start is called before the first frame update
	private void Start() {

		GetComponent<Button>().OnClickAsObservable()
							.Subscribe(_ => OnClick())
							.AddTo(this);

	}

	private bool done;

	private void OnClick() {
		if (done) return;
		done = true;

		if (stageInfo.StageId != 1) return;

		var tutorial = FindObjectOfType<TutorialController>();
		if (tutorial != null && tutorial.IsTutorial) {
			tutorial.TutorialEnd();
			done = false;
			return;
		}
		
		saveData.ClearStageId = 1;
		saveData.SetScore(1, 2);

		var wind = new WindTransition() {
			nextScene = DEFINE.MAIN,
			duration = 1.0f,
			size = 0.3f
		};
		TransitionKit.instance.transitionWithDelegate(wind);
	}

}