using Asterism;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller {

	public override void InstallBindings() {

		Container.BindInterfacesTo<SaveData>().AsSingle();

	}

}