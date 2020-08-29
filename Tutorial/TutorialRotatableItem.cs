using System;
using Asterism.Presentation;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Asterism.Tutorial {

    public class TutorialRotatableItem : RotatableItem {

        [SerializeField]
        private Transform[] targetRotate;

        [SerializeField]
        private RotatableItem rotatableItem;

        [SerializeField]
        private DragableItem dragableItem;

        [SerializeField]
        private float threshold;

        private bool isEnd;
        private bool isTutorial = true;

        private void Awake() {
            rotatableItem.forceStop = true;
            forceStop = true;
            if (dragableItem != null) dragableItem.ForceStop = true;
        }

        public async UniTask StartTutorial() {
            forceStop = false;

            await UniTask.WaitUntil(() => isEnd);
        }

        public void EndTutorial() {
            rotatableItem.forceStop = false;
            forceStop = true;
            isTutorial = false;
        }
        
        protected override void MouseDrag() {
            
            base.MouseDrag();
            if (forceStop) return;
            if (!isTutorial) return;

            var euler2 = target.rotation.eulerAngles;
            
            foreach (var v in targetRotate) {
                var euler = v.rotation.eulerAngles;
                
                var dis = (euler - euler2).sqrMagnitude;
                if (!(dis <= threshold)) continue;
                isEnd = true;
                forceStop = true;

                target.position = targetRotate[0].position;
                target.rotation = targetRotate[0].rotation;
                break;
            }
            
            
        }

    }

}
