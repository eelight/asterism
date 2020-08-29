using Zenject;

namespace Asterism.Domain.UseCases {

	public interface IStageInfo {

		int StageId { get; }
	}

	public class StageInfo : IStageInfo {

		[Inject]
		private IReferee referee;

		public int StageId => referee.StageId;

	}

}