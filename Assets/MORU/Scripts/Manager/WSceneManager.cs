using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace Moru.SceneManager
{
    public class WSceneManager : SingleToneMono<WSceneManager>
    {

        [SerializeField, ShowInInspector] AsyncOperation async;
        public float loading_gauge;

        public List<Button> lobbyMove;


        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }

        public void TestSceneLoader(int index)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        }

        public void OnSceneLoad_WhileLoading(int index)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);

            StartCoroutine(Co_SceneLoad(index));
        }

        public void SceneLoad()
        {
            if (!async.allowSceneActivation)
            { async.allowSceneActivation = true; }
        }

        private IEnumerator Co_SceneLoad(int _index)
        {
            yield return new WaitForSeconds(0.3f);
            async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_index);

            async.allowSceneActivation = false;
            loading_gauge = 0;
            while (!async.isDone)
            {
                
                if (async.isDone || loading_gauge>=0.9f)
                {
                    loading_gauge += Time.deltaTime;
                }
                else
                {
                    loading_gauge = async.progress;
                }
                yield return null;
            }
        }

        public void GoLobby()
        {
            TestSceneLoader(0);
            for (int i=0; i<lobbyMove.Count; i++)
            {

            }
        }

        public void OnAppQuit()
        {
            Application.Quit();
        }

    }
}