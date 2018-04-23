using UnityEngine;
using UnityEngine.UI;

public class SlideshowMenu : MonoBehaviour
{
    [SerializeField] private Image[] _images;
    [SerializeField] private float _imageTransitionDuration;
    [SerializeField] private AnimationCurve _imageTransitionCurve;

    private int _currentImageId;
    private float _timePassed;
    private bool _completed;

    public bool Completed
    {
        get { return _completed; }
    }

    public void Init()
    {
        _currentImageId = 0;
        _timePassed = 0;

        for (var i = 0; i < _images.Length; i++)
        {
            _images[i].gameObject.SetActive(false);
        }
        _images[_currentImageId].gameObject.SetActive(true);
    }

    public void CustomUpdate (float dt)
    {
        _timePassed += dt;

        var passedPart = Mathf.Clamp01(_timePassed / _imageTransitionDuration);
        var curvedPart = _imageTransitionCurve.Evaluate(passedPart);

        var color = _images[_currentImageId].color;
        color.a = curvedPart;
        _images[_currentImageId].color = color;

        if (_timePassed >= _imageTransitionDuration && _currentImageId < _images.Length - 1)
        {
            _timePassed -= _imageTransitionDuration;

            _images[_currentImageId].gameObject.SetActive(false);
            ++_currentImageId;
            _images[_currentImageId].gameObject.SetActive(true);
            var newColor = _images[_currentImageId].color;
            newColor.a = 0f;
            _images[_currentImageId].color = newColor;
        }

        if ((_timePassed >= _imageTransitionDuration && _currentImageId >= _images.Length - 1) || Input.GetKeyDown(KeyCode.Space))
        {
            _completed = true;
        }
    }
}
