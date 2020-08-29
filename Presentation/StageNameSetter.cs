using System;
using Asterism.Domain.UseCases;
using TMPro;
using UnityEngine;
using Zenject;

namespace Asterism.Presentation {
	
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class StageNameSetter : MonoBehaviour {

		[Inject]
		private IStageInfo stageInfo;

		private void Start() {

			var textMeshPro = GetComponent<TextMeshProUGUI>();
			textMeshPro.text = "STAGE " + stageInfo.StageId.ToString("D2");
		}

	}

}