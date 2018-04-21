using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    public delegate void ClickLighterAction(Vector3 position);
    public static event ClickLighterAction OnClickLighterAction;

    public void ClickLight(Vector3 position)
    {
        if (OnClickLighterAction != null)
            OnClickLighterAction(position);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Update click!");
            ClickLight(gameObject.transform.position);
        }
    }
}
