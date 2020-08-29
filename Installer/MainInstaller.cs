using Asterism.Common;
using Asterism.Domain.Entities;
using Asterism.Domain.UseCases;
using Zenject;

public class MainInstaller : MonoInstaller {

	public override void InstallBindings() {

		Container.BindInterfacesTo<LoggerSettings>().AsCached();
		Container.BindInterfacesTo<StageClear>().AsCached();

		var referee = FindObjectOfType<Referee>();
		Container.BindInstance<IReferee>(referee).AsCached();
		
		var stageLoader = FindObjectOfType<StageLoader>();
		Container.BindInstance<IStageLoader>(stageLoader).AsCached();
		
		Container.BindInterfacesTo<StageInfo>().AsCached();

	}

}