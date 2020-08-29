using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Asterism.Domain.UseCases {
    
    public interface IStageLoader {

        int GetStageId();

    }

    public class StageLoader : MonoBehaviour, IStageLoader {

        [SerializeField]
        private GameObject[] loadOnCompletes;

        [Inject]
        private ISaveData saveData;
        
        private static int stageId;

        private void Awake() {
            foreach (var v in loadOnCompletes) v.SetActive(false);

            stageId = saveData.ClearStageId + 1;
            stageId = Mathf.Min(stageId, DEFINE.FINAL_STAGE_ID);
            
            #if UNITY_EDITOR
            if (SceneManager.sceneCount >= 2) {
                return;
            }
            #endif
            
            LoadAsync().Forget();
        }

        private async UniTaskVoid LoadAsync() {
            await SceneManager.LoadSceneAsync("Stage_" + stageId, LoadSceneMode.Additive);
            foreach (var v in loadOnCompletes) v.SetActive(true);
        }

        public int GetStageId() {
            return stageId;
        }

    }

}
