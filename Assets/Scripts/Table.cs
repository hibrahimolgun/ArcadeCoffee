using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Table : MonoBehaviour
{
    [Header("Initial Assignments")]
    [SerializeField] private int _tableNumber; //enter the table number in the inspector
    [SerializeField] private IntEvent _coinEvent;
    [SerializeField] private SOint _totalCoin;
    [SerializeField] private Collider _collider;
    [SerializeField] private Collider[] _boundColliders;
    [SerializeField] private TableLockList _tableLockList;
    [SerializeField] private IntEvent _unlockEvent;
    private int _price;
    private bool _isLocked;
    
    [Header("Table Model")]
    [SerializeField] private GameObject _tableModel;
    [SerializeField] private GameObject _tableModelLocked;

    [Header("Unlock Settings")]
    [SerializeField] private float _paySpeed;
    [SerializeField] private int _payAmount;
    [SerializeField] private float _waitTimeToPay;
    private bool _isPaying;

    [Header("Animations")]
    [SerializeField] private float _bopSize=1.3f;
    [SerializeField] private float _bopTime=0.35f;
    //GetCoinPool TO animate payment
    [SerializeField] private Coin _coinPrefab;
    private ObjectPool<Coin> _coinPool;
    [SerializeField] private Image _waitTimeLockImage;
    [SerializeField] private Sequence _waitTimeLockSequence;
    [SerializeField] private TextMeshProUGUI _priceText;

    private void Start()
    {
        _isLocked = _tableLockList._tableLocks[_tableNumber].isLocked;
        _price  = _tableLockList._tableLocks[_tableNumber].price;
        ToggleTable();
        _coinPool = new ObjectPool<Coin>(_coinPrefab, 10, transform);
        _priceText.text = _price.ToString();
    }

    private void ToggleTable()
    {
        if (_isLocked == true)
        {
            _tableModel.SetActive(false);
            _tableModelLocked.SetActive(true);
            for (int i = 0; i < _boundColliders.Length; i++)
            {
                _boundColliders[i].enabled = false;
            }
            _collider.enabled = true;
        }
        else
        {
            _tableModel.SetActive(true);
            _tableModelLocked.SetActive(false);
            for (int i = 0; i < _boundColliders.Length; i++)
            {
                _boundColliders[i].enabled = true;
            }
            _collider.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player) == false) return;
        if (_isLocked == false) return;
        if (_totalCoin.value < 0) return;
        StartCoroutine(WaitToPay(player));
        Debug.Log("player enter");
    }

    private IEnumerator WaitToPay(PlayerController player)
    {
        _waitTimeLockSequence = DOTween.Sequence();
        _waitTimeLockSequence.Append(_waitTimeLockImage.DOFillAmount(1, _waitTimeToPay).SetEase(Ease.Linear));
        yield return new WaitForSeconds(_waitTimeToPay);
        _isPaying = true;
        Debug.Log("start paying");
        StartCoroutine(Pay(player));
    }

    private IEnumerator Pay(PlayerController player)
    {
        while (_isPaying == true)
        {
            if (_totalCoin.value < _payAmount) yield break;
            if (_price <= 0)
            {
                _isLocked = false;
                _tableLockList._tableLocks[_tableNumber].isLocked = false;
                _unlockEvent.InvokeAction(_tableNumber);
                UnlockAnimation();
                ToggleTable();
                StopCoroutine(Pay(player));
                yield break;
            }
            yield return new WaitForSeconds(_paySpeed);
            CoinAnimation(player);
            _price -= _payAmount;
            _tableLockList._tableLocks[_tableNumber].price -= _payAmount;
            _totalCoin.value -= _payAmount;
            _coinEvent.InvokeAction(0);

            Debug.Log("Paid");
        }
        Debug.Log("stop paying");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out _) == false) return;
        _waitTimeLockSequence.Kill();
        _waitTimeLockImage.fillAmount = 0;
        StopAllCoroutines();
        _isPaying = false;
        Debug.Log("player exit");
    }

    private void UnlockAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(_tableModel.transform.DOScale(_bopSize, _bopTime).SetEase(Ease.InExpo));
        sequence.Append(_tableModel.transform.DOScale(1f, _bopTime).SetEase(Ease.OutExpo));
    }

    private void CoinAnimation(PlayerController player)
    {
        Coin coin = _coinPool.Get();
        coin.transform.position = player.transform.position;
        coin.gameObject.SetActive(true);
        var sequence = DOTween.Sequence();
        sequence.Append(coin.transform.DOJump(transform.position,1.5f,1, _paySpeed).SetEase(Ease.InOutCirc));
        sequence.Join(coin.transform.DOScale(0.1f, _paySpeed).SetEase(Ease.InOutCirc));
        sequence.OnComplete(() =>
        {
            _priceText.text = _price.ToString();
            //TEXT ANIMATON
            var sequence2 = DOTween.Sequence();
            sequence2.Append(_priceText.transform.DOScale(1.5f, _paySpeed/2).SetEase(Ease.InCirc));
            sequence2.Append(_priceText.transform.DOScale(1f, _paySpeed/2).SetEase(Ease.OutCirc));
            //RESET COIN VALUES
            coin.transform.localScale = Vector3.one;
            coin.gameObject.SetActive(false);
            _coinPool.Return(coin);
        });
    }
}
