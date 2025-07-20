using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ScenesController : MonoBehaviour
{
    public static ScenesController Instance { get; private set; }

    private int _currentIndex;
    private int _nextIndex;

    private AsyncOperation _nextSceneLoadOp;
    private bool _isNextSceneReady = false;
    private bool _waitingForSceneLoading = false;

    private List<GameObject> _nextSceneRootObjects;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Garantir só 1 AudioListener ativo na cena atual
        AudioListener[] listeners = GameObject.FindObjectsOfType<AudioListener>();
        for (int i = 1; i < listeners.Length; i++)
            listeners[i].enabled = false;

        // Garantir só 1 EventSystem ativo na cena atual
        EventSystem[] eventSystems = GameObject.FindObjectsOfType<EventSystem>();
        for (int i = 1; i < eventSystems.Length; i++)
            eventSystems[i].gameObject.SetActive(false);

        PreloadNextScene();
    }

    public void PreloadNextScene()
    {
        _currentIndex = SceneManager.GetActiveScene().buildIndex;
        _nextIndex = _currentIndex + 1;

        if (_nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Não há mais cenas depois desta.");
            return;
        }

        StartCoroutine(LoadSceneAsync(_nextIndex));
    }

    private IEnumerator LoadSceneAsync(int index)
    {
        _nextSceneLoadOp = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        _nextSceneLoadOp.allowSceneActivation = false;

        while (_nextSceneLoadOp.progress < 0.9f)
        {
            Debug.Log($"[ScenesController] Carregando cena {index}... {(_nextSceneLoadOp.progress * 100f):F0}%");
            yield return null;
        }

        // Aguarda cena estar carregada na hierarquia
        Scene nextScene = SceneManager.GetSceneByBuildIndex(index);
        while (!nextScene.isLoaded)
            yield return null;

        // Pega todos os root objects e desativa eles para impedir execução antecipada
        _nextSceneRootObjects = new List<GameObject>(nextScene.GetRootGameObjects());
        foreach (var obj in _nextSceneRootObjects)
        {
            obj.SetActive(false);

            // Desativa EventSystem se houver
            var eventSystem = obj.GetComponentInChildren<EventSystem>(true);
            if (eventSystem != null)
                eventSystem.gameObject.SetActive(false);

            // Desabilita AudioListener se houver
            var audioListener = obj.GetComponentInChildren<AudioListener>(true);
            if (audioListener != null)
                audioListener.enabled = false;
        }

        _isNextSceneReady = true;
        Debug.Log($"[ScenesController] Cena {index} pré-carregada e desativada.");

        if (_waitingForSceneLoading)
        {
            StartCoroutine(SwitchScenesRoutine());
        }
    }

    public void AdvanceScene()
    {
        if (_isNextSceneReady)
        {
            StartCoroutine(SwitchScenesRoutine());
        }
        else
        {
            _waitingForSceneLoading = true;
            Debug.Log("[ScenesController] Esperando a próxima cena carregar para avançar...");
        }
    }

    private IEnumerator SwitchScenesRoutine()
    {
        _waitingForSceneLoading = false;

        // Reativa os root objects da próxima cena
        foreach (var obj in _nextSceneRootObjects)
        {
            obj.SetActive(true);

            var eventSystem = obj.GetComponentInChildren<EventSystem>(true);
            if (eventSystem != null)
                eventSystem.gameObject.SetActive(true);

            var audioListener = obj.GetComponentInChildren<AudioListener>(true);
            if (audioListener != null)
                audioListener.enabled = true;
        }

        _nextSceneLoadOp.allowSceneActivation = true;

        while (!_nextSceneLoadOp.isDone)
            yield return null;

        // Define a nova cena como ativa
        Scene newScene = SceneManager.GetSceneByBuildIndex(_nextIndex);
        SceneManager.SetActiveScene(newScene);

        // Descarrega a cena anterior
        yield return SceneManager.UnloadSceneAsync(_currentIndex);

        // Atualiza índices e flags
        _currentIndex = _nextIndex;
        _nextSceneLoadOp = null;
        _isNextSceneReady = false;
        _nextSceneRootObjects = null;

        // Começa o preload da próxima cena automaticamente
        PreloadNextScene();
    }
}
