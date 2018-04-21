using UnityEngine;

public class SlideshowMenu : MonoBehaviour
{
    private bool _completed;

    public bool Completed
    {
        get { return _completed; }
    }

    public void Init()
    {

    }

    public void CustomUpdate (float dt)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _completed = true;
        }
    }
}
