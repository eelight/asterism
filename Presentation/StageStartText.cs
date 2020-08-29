using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class StageStartText : MonoBehaviour {
	
	[SerializeField]
	private bool autoRun;

	private void Start() {
		if(autoRun) StartEffectAsync().Forget();
	}

	public async UniTask StartEffectAsync(CancellationToken token = default) {
		if (token == default) token = gameObject.GetCancellationTokenOnDestroy();
		var tex = GetComponent<TextMeshProUGUI>();

		await tex.DOFade(0f, 0f);
		
		tex.enabled = true;

		tex.DOFade(1f, 1f);
		DOTween.To(() => tex.characterSpacing, x => tex.characterSpacing = x, 40f, 2.4f);
		await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
		await tex.DOFade(0f, 1f).WithCancellation(token);
		tex.enabled = false;
	}

}