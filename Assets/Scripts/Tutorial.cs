using UnityEngine;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Scene _gameScene;
    [SerializeField] private GameObject[] _tutorialObjects;
    [SerializeField] private TextMeshProUGUI _tutorialText;
    [SerializeField] private Button NextButton;
    [SerializeField] private Button PreviousButton;
    [SerializeField] private Button SkipButton;
    [SerializeField] private Button Play;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private ScriptableEvent _doubleTapEvent;

    [SerializeField] private int _currentTutorialIndex = 0;
    private Sequence sequence;
    private string[] _tutorialTexts = new string[]
    {
        "Beans to Machine \n Espresso shot to Combiner \n Cup to Combiner \n = \n Cup of Coffee",
        "Empty Tea Cup to TeaPot \n = \n Cup of Tea",
        "Any Dessert to Combiner \n Empty Plate to Combiner \n = \n Dessert Plate",
        "Double Click to Drop Items \n and Use Broom to Clean Up",
    };


    private void Start()
    {
        NextButton.onClick.AddListener(NextTutorial);
        PreviousButton.onClick.AddListener(PreviousTutorial);
        SkipButton.onClick.AddListener(SkipTutorial);
        Play.onClick.AddListener(PlayTutorial);
        PreviousButton.interactable = false;
        _tutorialObjects[_currentTutorialIndex].SetActive(true);
    }

    private void NextTutorial()
    {
        sequence.Kill();
        transform.position = Vector3.zero;
        _tutorialObjects[_currentTutorialIndex].SetActive(false);
        PreviousButton.interactable = true;
        _currentTutorialIndex++;
        _tutorialObjects[_currentTutorialIndex].SetActive(true);
        Debug.Log(_currentTutorialIndex);
        if (_currentTutorialIndex == _tutorialTexts.Length) NextButton.interactable = false;
    }
    
    private void PreviousTutorial()
    {
        sequence.Kill();
        transform.position = Vector3.zero;
        _tutorialObjects[_currentTutorialIndex].SetActive(false);
        NextButton.interactable = true;
        _currentTutorialIndex--;
        _tutorialObjects[_currentTutorialIndex].SetActive(true);
        Debug.Log(_currentTutorialIndex);
        if (_currentTutorialIndex == 0) PreviousButton.interactable = false;
    }


    private void PlayTutorial()
    {
        if (_playerController._holdingItem != null) Destroy(_playerController._holdingItem.gameObject);
        transform.position = Vector3.zero;
        var index = _currentTutorialIndex;
        switch (index)
        {
            case 0:
                sequence.Kill();
                // Beans => Espresso Machine => Espresso Shot + Empty Cup = Cup of Coffee
                _tutorialText.text = _tutorialTexts[0];
                sequence = DOTween.Sequence();
                sequence.Append(transform.DOMoveX(-1.6f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f));
                sequence.Append(transform.DOMoveZ(2f, 1f));
                sequence.Append(transform.DOMoveZ(0f, 1f).SetDelay(3f));
                sequence.Append(transform.DOMoveZ(2f, 1f));
                sequence.Append(transform.DOMoveZ(0f, 1f));
                sequence.Append(transform.DOMoveX(1.6f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f));
                sequence.Append(transform.DOMoveZ(-2f, 1f));
                sequence.Append(transform.DOMoveZ(0f, 1f));
                sequence.Append(transform.DOMoveX(1.6f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f));
                sequence.Append(transform.DOJump(Vector3.zero, 1.5f, 1, 1.5f).SetEase(Ease.InOutQuad));
                sequence.Play();
                break;
            case 1:
                sequence.Kill();
                // Empty Tea Cup => TeaPot => Cup of Tea
                _tutorialText.text = _tutorialTexts[1];
                sequence = DOTween.Sequence();
                sequence.Append(transform.DOMoveX(1.2f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f));
                sequence.Append(transform.DOMoveX(-1.2f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f).SetDelay(3f));
                sequence.Append(transform.DOMoveX(-1.2f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f));
                sequence.Append(transform.DOJump(Vector3.zero, 1.5f, 1, 1.5f).SetEase(Ease.InOutQuad));
                sequence.Play();
                break;
            case 2:
                // Any Dessert + Empty Plate = Dessert Plate
                sequence.Kill();
                _tutorialText.text = _tutorialTexts[2];
                sequence = DOTween.Sequence();
                sequence.Append(transform.DOMoveZ(2.3f, 1f));
                sequence.Append(transform.DOMoveZ(0f, 1f));
                sequence.Append(transform.DOMoveZ(-2f, 1f));
                sequence.Append(transform.DOMoveZ(0f, 1f));
                sequence.Append(transform.DOMoveX(1.4f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f));
                sequence.Append(transform.DOMoveZ(-2f, 1f));
                sequence.Append(transform.DOMoveZ(0f, 1f));
                sequence.Append(transform.DOJump(Vector3.zero, 1.5f, 1, 1.5f).SetEase(Ease.InOutQuad));
                sequence.Play();
                break;
            case 3:
                // Double Click to Drop Items and Use Broom to Clean Up
                sequence.Kill();
                _tutorialText.text = _tutorialTexts[3];
                sequence = DOTween.Sequence();
                sequence.Append(transform.DOMoveZ(6.5f, 1f));
                sequence.Append(transform.DOMoveX(-1.40f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f).OnComplete(() => _doubleTapEvent.InvokeAction()));
                sequence.Append(transform.DOMoveZ(3.1f, 1f));
                sequence.Append(transform.DOMoveX(2.39f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f).OnComplete(() => _doubleTapEvent.InvokeAction()));
                sequence.Append(transform.DOMoveZ(0.29f, 1f));
                sequence.Append(transform.DOMoveX(-1.22f, 1f));
                sequence.Append(transform.DOMoveX(0f, 1f).OnComplete(() => _doubleTapEvent.InvokeAction()));
                sequence.Append(transform.DOMoveZ(-3f, 3f));
                sequence.Append(transform.DOMoveZ(6.6f, 6f));
                sequence.Append(transform.DOMoveZ(-3f, 1f));
                sequence.Append(transform.DOMoveZ(0f, 1f));
                sequence.Append(transform.DOJump(Vector3.zero, 1.5f, 1, 1.5f).SetEase(Ease.InOutQuad));
                sequence.Play();
                break;
        }
    }
    
    private void SkipTutorial()
    {
        SceneManager.LoadScene(1);
    }
}
