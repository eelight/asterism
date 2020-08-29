using UniRx;

namespace Asterism.Domain.Entities {
	
	public interface IStageClear {

		Subject<Unit> IsClear { get; }
		void Clear();

	}

	public class StageClear : IStageClear {

		public Subject<Unit> IsClear { get; }

		public void Clear() {
			IsClear.OnNext(Unit.Default);
		}

		public StageClear() {
			IsClear = new Subject<Unit>();
		}

	}

}