using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScenesController : MonoBehaviour
{
    public static ScenesController Instance { get; private set; }

    public int _currentIndex;
    public int _nextIndex;

    private AsyncOperation _nextSceneLoadOp;
    private bool _isNextSceneReady = false;
    private bool _waitingForSceneLoading = false;

    private List<GameObject> _nextSceneRootObjects;

    private bool runningTransition = false;

    public TransitionSettings transition;
    public float startDelay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        //// Garante que só tenha 1 AudioListener ativo
        //AudioListener[] listeners = GameObject.FindObjectsOfType<AudioListener>();
        //for (int i = 1; i < listeners.Length; i++)
        //    listeners[i].enabled = false;

        //// Garante que só tenha 1 EventSystem ativo
        //EventSystem[] eventSystems = GameObject.FindObjectsOfType<EventSystem>();
        //for (int i = 1; i < eventSystems.Length; i++)
        //    eventSystems[i].gameObject.SetActive(false);

        //PreloadNextScene();
    }

    public void NextScene()
    {
        SceneManager.LoadScene(_nextIndex);
        _nextIndex += 1;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(_currentIndex);
    }

    public void SetSceneIndex()
    {
        _currentIndex += 1;
        _nextIndex += 1;
    }


    //    public void PreloadNextScene()
    //    {
    //        _currentIndex = SceneManager.GetActiveScene().buildIndex;
    //        _nextIndex = _currentIndex + 1;

    //        if (_nextIndex >= SceneManager.sceneCountInBuildSettings)
    //        {
    //            Debug.LogWarning("Não há mais cenas depois desta.");
    //            return;
    //        }

    //        StartCoroutine(LoadSceneAsync(_nextIndex));
    //    }

    //    private IEnumerator LoadSceneAsync(int index)
    //    {
    //        _nextSceneLoadOp = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
    //        _nextSceneLoadOp.allowSceneActivation = false;

    //        while (_nextSceneLoadOp.progress < 0.9f)
    //        {
    //            Debug.Log($"[ScenesController] Carregando cena {index}... {(_nextSceneLoadOp.progress * 100f):F0}%");
    //            yield return null;
    //        }

    //        // Aguarda cena carregada na hierarquia
    //        Scene nextScene = SceneManager.GetSceneByBuildIndex(index);
    //        while (!nextScene.isLoaded)
    //            yield return null;

    //        // Desativa todos os objetos raiz para evitar execução antecipada
    //        _nextSceneRootObjects = new List<GameObject>(nextScene.GetRootGameObjects());
    //        foreach (var obj in _nextSceneRootObjects)
    //        {
    //            obj.SetActive(false);

    //            // Desativa EventSystem se existir
    //            var eventSystem = obj.GetComponentInChildren<EventSystem>(true);
    //            if (eventSystem != null)
    //                eventSystem.gameObject.SetActive(false);

    //            // Desabilita AudioListener se existir
    //            var audioListener = obj.GetComponentInChildren<AudioListener>(true);
    //            if (audioListener != null)
    //                audioListener.enabled = false;
    //        }

    //        _isNextSceneReady = true;
    //        Debug.Log($"[ScenesController] Cena {index} pré-carregada e desativada.");

    //        if (_waitingForSceneLoading)
    //        {
    //            StartCoroutine(SwitchScenesRoutine());
    //        }
    //    }

    //    // Chama essa para avançar a cena SEM transição (troca direta)
    //    public void AdvanceScene()
    //    {
    //        if (_isNextSceneReady)
    //        {
    //            StartCoroutine(SwitchScenesRoutine());
    //        }
    //        else
    //        {
    //            _waitingForSceneLoading = true;
    //            Debug.Log("[ScenesController] Esperando a próxima cena carregar para avançar...");
    //        }
    //    }

    //    // Chama essa para avançar a cena COM transição visual (usando TransitionManager)
    //    public void AdvanceSceneWithTransition(TransitionSettings transition, float startDelay)
    //    {
    //        if (!_isNextSceneReady)
    //        {
    //            _waitingForSceneLoading = true;
    //            Debug.Log("[ScenesController] Esperando a próxima cena carregar para avançar...");
    //            return;
    //        }

    //        if (TransitionManager.Instance() == null)
    //        {
    //            Debug.LogError("TransitionManager não encontrado!");
    //            return;
    //        }

    //        if (runningTransition)
    //        {
    //            Debug.LogWarning("Transição já está rodando.");
    //            return;
    //        }

    //        runningTransition = true;

    //        // Assina os eventos do TransitionManager
    //        TransitionManager.Instance().onTransitionCutPointReached += OnTransitionCutPointReached;
    //        TransitionManager.Instance().onTransitionEnd += OnTransitionEnd;

    //        // Começa só a animação da transição (sem carregar cena)
    //        TransitionManager.Instance().Transition(transition, startDelay);
    //    }

    //    private void OnTransitionCutPointReached()
    //    {
    //        // Quando o efeito visual chega no ponto de corte, troca de cena
    //        StartCoroutine(SwitchScenesRoutine());
    //    }

    //    private void OnTransitionEnd()
    //    {
    //        // Limpa flags e desassocia eventos quando a transição termina
    //        runningTransition = false;

    //        if (TransitionManager.Instance() != null)
    //        {
    //            TransitionManager.Instance().onTransitionCutPointReached -= OnTransitionCutPointReached;
    //            TransitionManager.Instance().onTransitionEnd -= OnTransitionEnd;
    //        }
    //    }

    //    private IEnumerator SwitchScenesRoutine()
    //    {
    //        _waitingForSceneLoading = false;

    //        // Reativa os objetos raiz da próxima cena
    //        foreach (var obj in _nextSceneRootObjects)
    //        {
    //            obj.SetActive(true);

    //            var eventSystem = obj.GetComponentInChildren<EventSystem>(true);
    //            if (eventSystem != null)
    //                eventSystem.gameObject.SetActive(true);

    //            var audioListener = obj.GetComponentInChildren<AudioListener>(true);
    //            if (audioListener != null)
    //                audioListener.enabled = true;
    //        }

    //        _nextSceneLoadOp.allowSceneActivation = true;

    //        while (!_nextSceneLoadOp.isDone)
    //            yield return null;

    //        // Define a nova cena como ativa
    //        Scene newScene = SceneManager.GetSceneByBuildIndex(_nextIndex);
    //        SceneManager.SetActiveScene(newScene);

    //        // Descarrega a cena antiga
    //        yield return SceneManager.UnloadSceneAsync(_currentIndex);

    //        _currentIndex = _nextIndex;
    //        _nextSceneLoadOp = null;
    //        _isNextSceneReady = false;
    //        _nextSceneRootObjects = null;

    //        // Começa a pré-carregar a próxima cena
    //        PreloadNextScene();
    //    }
}
