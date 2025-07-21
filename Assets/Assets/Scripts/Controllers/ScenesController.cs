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

        //// Garante que s� tenha 1 AudioListener ativo
        //AudioListener[] listeners = GameObject.FindObjectsOfType<AudioListener>();
        //for (int i = 1; i < listeners.Length; i++)
        //    listeners[i].enabled = false;

        //// Garante que s� tenha 1 EventSystem ativo
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
    //            Debug.LogWarning("N�o h� mais cenas depois desta.");
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

    //        // Desativa todos os objetos raiz para evitar execu��o antecipada
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
    //        Debug.Log($"[ScenesController] Cena {index} pr�-carregada e desativada.");

    //        if (_waitingForSceneLoading)
    //        {
    //            StartCoroutine(SwitchScenesRoutine());
    //        }
    //    }

    //    // Chama essa para avan�ar a cena SEM transi��o (troca direta)
    //    public void AdvanceScene()
    //    {
    //        if (_isNextSceneReady)
    //        {
    //            StartCoroutine(SwitchScenesRoutine());
    //        }
    //        else
    //        {
    //            _waitingForSceneLoading = true;
    //            Debug.Log("[ScenesController] Esperando a pr�xima cena carregar para avan�ar...");
    //        }
    //    }

    //    // Chama essa para avan�ar a cena COM transi��o visual (usando TransitionManager)
    //    public void AdvanceSceneWithTransition(TransitionSettings transition, float startDelay)
    //    {
    //        if (!_isNextSceneReady)
    //        {
    //            _waitingForSceneLoading = true;
    //            Debug.Log("[ScenesController] Esperando a pr�xima cena carregar para avan�ar...");
    //            return;
    //        }

    //        if (TransitionManager.Instance() == null)
    //        {
    //            Debug.LogError("TransitionManager n�o encontrado!");
    //            return;
    //        }

    //        if (runningTransition)
    //        {
    //            Debug.LogWarning("Transi��o j� est� rodando.");
    //            return;
    //        }

    //        runningTransition = true;

    //        // Assina os eventos do TransitionManager
    //        TransitionManager.Instance().onTransitionCutPointReached += OnTransitionCutPointReached;
    //        TransitionManager.Instance().onTransitionEnd += OnTransitionEnd;

    //        // Come�a s� a anima��o da transi��o (sem carregar cena)
    //        TransitionManager.Instance().Transition(transition, startDelay);
    //    }

    //    private void OnTransitionCutPointReached()
    //    {
    //        // Quando o efeito visual chega no ponto de corte, troca de cena
    //        StartCoroutine(SwitchScenesRoutine());
    //    }

    //    private void OnTransitionEnd()
    //    {
    //        // Limpa flags e desassocia eventos quando a transi��o termina
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

    //        // Reativa os objetos raiz da pr�xima cena
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

    //        // Come�a a pr�-carregar a pr�xima cena
    //        PreloadNextScene();
    //    }
}
