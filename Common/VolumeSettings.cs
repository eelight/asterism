using Extensions;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Asterism.Common {

	public class VolumeSettings : MonoBehaviour {

		[SerializeField]
		private AudioMixer audioMixer;

		[SerializeField]
		private Slider slider;

		private void Awake() {
			slider.onValueChanged.AddListener(OnChangeVolume);

			audioMixer.GetFloat("MasterVolume", out var decibel);
			slider.value = decibel.FromDecibel();
		}

		private void OnChangeVolume(float value) {

			audioMixer.SetFloat("MasterVolume", value.ToDecibel());
		}

	}

}