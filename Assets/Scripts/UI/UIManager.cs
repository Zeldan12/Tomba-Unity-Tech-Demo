using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI _scoreUI;
    [SerializeField]
    private TextMeshProUGUI _healthNumberUI;
    [SerializeField]
    private Transform _divisionParent;
    [SerializeField]
    private GameObject _divisionPrefab;
    [SerializeField]
    private int _healthUpdateBlinks;
    [SerializeField]
    private float _healthUpdateBlinkDelay = 0.1f;


    private Dictionary<int, HPDivision> _divisions;
    private int _maxHp;
    private int _currentHp;
    private Coroutine _healthUpdateCoroutine;

    private void Awake() {
        if (Instance != null) {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void SetUp(Tomba tomba) {
        _divisions = new Dictionary<int, HPDivision>();
        _maxHp = tomba.MaxHealth;
        _currentHp = tomba.Health;
        int divQuant = 16;
        if (_maxHp <= 8) {
            divQuant = 8;
        }

        HPColor newColor = FindHPColor(_currentHp);

        _healthNumberUI.text = "<sprite name=\"" + _currentHp + "\">";
        for (int i = 1; i <= _maxHp; i++) {
            GameObject newDiv = Instantiate(_divisionPrefab, _divisionParent);
            HPDivision newScritp = newDiv.GetComponent<HPDivision>();
            _divisions.Add(i, newScritp);
            newScritp.SetEnable(true);

            if (i <= _currentHp) {
                newScritp.ChangeColor(newColor, divQuant, i);
            } else {
                newScritp.ChangeColor(HPColor.Clear, divQuant, i);
            }

        }
    }
    public void UpdateScore(int score) {
        string newScore = "";

        foreach (char digit in score.ToString("D" + 6.ToString())) {
            newScore += "<sprite name=\"" + digit + "\">";
        }

        _scoreUI.text = newScore;

    }

    public void UpdateHealth(int hp) {

        HPColor newColor = FindHPColor(hp);
        HPColor oldColor = FindHPColor(_currentHp);

        if (_healthUpdateCoroutine != null) {
            StopCoroutine(_healthUpdateCoroutine);
        }

        _healthUpdateCoroutine = StartCoroutine(HPChange(_currentHp, hp, oldColor, newColor));
    }

    public void IncreaseMaxHealth() {
        int divQuant = 8;
        if (_maxHp == 8) {

            divQuant = 16;
            for (int i = 1; i <= _divisions.Count; i++) {
                _divisions[i].ChangeSize(divQuant, i);
            }
        }
        _maxHp++;

        GameObject newDiv = Instantiate(_divisionPrefab, _divisionParent);
        HPDivision newScritp = newDiv.AddComponent<HPDivision>();
        _divisions[_maxHp] = newScritp;

        newScritp.SetEnable(true);
        newScritp.ChangeColor(HPColor.Clear, divQuant, _maxHp);

    }

    private IEnumerator HPChange(int oldHP, int newHP, HPColor oldColor, HPColor newColor) {
        _currentHp = newHP;
        int divQuant = 16;
        if (_maxHp <= 8) {
            divQuant = 8;
        }
        for (int i = 0; i < _healthUpdateBlinks; i++) {
            for (int j = 1; j <= _maxHp; j++) {
                if (i % 2 == 0) {
                    if (j > newHP) {
                        _divisions[j].ChangeColor(HPColor.Clear, divQuant, j);
                    } else {
                        _divisions[j].ChangeColor(newColor, divQuant, j);
                    }
                    _healthNumberUI.text = "<sprite name=\"" + newHP + "\">";
                } else {
                    if (j > oldHP) {
                        _divisions[j].ChangeColor(HPColor.Clear, divQuant, j);
                    } else {
                        _divisions[j].ChangeColor(oldColor, divQuant, j);
                    }
                    _healthNumberUI.text = "<sprite name=\"" + oldHP + "\">";
                }

            }
            yield return new WaitForSeconds(_healthUpdateBlinkDelay);
        }
        for (int i = 1; i <= _maxHp; i++) {
            if (i > newHP) {
                _divisions[i].ChangeColor(HPColor.Clear, divQuant, i);
            } else {
                _divisions[i].ChangeColor(newColor, divQuant, i);
            }

        }
        _healthNumberUI.text = "<sprite name=\"" + newHP + "\">";

    }

    private HPColor FindHPColor(int hp) {
        if (hp <= 2) {
            return HPColor.Red;
        } else if (hp <= 4) {
            return HPColor.Orange;
        } else if (hp <= 6) {
            return HPColor.Yellow;
        }
        return HPColor.Green;
    }
}
