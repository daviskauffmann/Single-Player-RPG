using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    [SerializeField]Text _content = null;
    CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        _canvasGroup.alpha = 0;
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void Activate(Usable usable)
    {
        _content.text = usable.Tooltip();

        _canvasGroup.alpha = 1;
    }

    public void Deactivate()
    {
        _canvasGroup.alpha = 0;
    }
}
