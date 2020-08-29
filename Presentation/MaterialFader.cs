using System.Linq;
using Asterism.Domain.UseCases;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Asterism.Presentation {

    public class MaterialFader : MonoBehaviour, IFader {

        private Material[] materials;
        
        [SerializeField]
        private MeshRenderer[] meshRenderers;
        
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        [SerializeField]
        private bool manualSet;

        [ContextMenu("Set")]
        private void Set() {
            meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
        }

        // Start is called before the first frame update
        private void Awake() {
            if (!manualSet) materials = GetComponentsInChildren<MeshRenderer>().Select(mesh => mesh.material).ToArray();
            else materials = meshRenderers.Select(mesh => mesh.material).ToArray();
        }
        
        public async UniTask FadeIn(float duration) {

            Tweener tween = null;
            
            foreach (var material in materials) {
                var c = material.GetColor(BaseColor);
                c.a = 0f;
                material.SetColor(BaseColor, c);
            }

            foreach (var material in materials) {
                var c = material.GetColor(BaseColor);
                c.a = 1f;
                tween = DOTween.To(() => material.GetColor(BaseColor), x => material.SetColor(BaseColor, x), c, duration);
            }

            if (tween == null) await UniTask.DelayFrame(1);
            else await tween;
        }

        public async UniTask FadeOut(float duration) {
            Tweener tween = null;
            
            foreach (var material in materials) {
                
                var c = material.GetColor(BaseColor);
                c.a = 0f;
                tween = DOTween.To(() => material.GetColor(BaseColor), x => material.SetColor(BaseColor, x), c, duration);
            }

            if (tween == null) await UniTask.DelayFrame(1);
            else await tween;
        }

    }

}
