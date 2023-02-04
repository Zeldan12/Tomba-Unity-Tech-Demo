using UnityEngine;
using UnityEngine.UI;

public class HPDivision : MonoBehaviour {

    private HPColor _currentColor;
    private Image _image;
    private string _basePath = "Sprites/UI/HP/";

    private void Awake() {
        _image = GetComponent<Image>();
    }

    public void ChangeColor(HPColor newColor, int division, int number) {
        if (newColor != _currentColor) {
            _image.sprite = Resources.Load<Sprite>(_basePath + "HP" + division.ToString() + "/" + newColor.ToString() + "/" + number.ToString());
            _currentColor = newColor;
        }
    }

    public void ChangeSize(int division, int number) {
        _image.sprite = Resources.Load<Sprite>(_basePath + "HP" + division.ToString() + "/" + _currentColor.ToString() + "/" + number.ToString());
    }

    public void SetEnable(bool enabled) {
        _image.enabled = enabled;
    }
}
