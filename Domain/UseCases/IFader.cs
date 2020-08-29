using Cysharp.Threading.Tasks;

namespace Asterism.Domain.UseCases {

	public interface IFader {

		/// <summary>
		/// 表示
		/// </summary>
		UniTask FadeIn(float duration);
		
		/// <summary>
		/// 非表示
		/// </summary>
		UniTask FadeOut(float duration);

	}

}