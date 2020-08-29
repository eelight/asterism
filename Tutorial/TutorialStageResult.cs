using Asterism.Domain.UseCases;
using UnityEngine;

namespace Asterism.Tutorial {

	public class TutorialStageResult : StageResult {

		public bool IsTutorial { get; set; } = true;

		protected override void Transition() {
			if (IsTutorial) return;
			base.Transition();
		}

		public void TutorialEndTransition() {
			base.Transition();
		}

	}

}