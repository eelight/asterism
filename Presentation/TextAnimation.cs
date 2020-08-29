using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextAnimation : MonoBehaviour {
	
	[SerializeField]
	private bool auto;
	
	private void Start() {
		if(auto) StartEffectAsync().Forget();
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
	
	public async UniTask StartEffectAsync2(CancellationToken token = default) {
		if (token == default) token = gameObject.GetCancellationTokenOnDestroy();
		var tex = GetComponent<TextMeshProUGUI>();
		var group = GetComponent<CanvasGroup>();

		await group.DOFade(0f, 0f);
		
		tex.enabled = true;

		group.DOFade(1f, 1f);
		DOTween.To(() => tex.characterSpacing, x => tex.characterSpacing = x, 40f, 2.4f);
		await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: token);
		await group.DOFade(0f, 1f).WithCancellation(token);
		tex.enabled = false;
	}

}