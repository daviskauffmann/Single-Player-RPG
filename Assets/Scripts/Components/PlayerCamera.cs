using UnityEngine;
using UnityEngine.EventSystems;
using RPG.UI;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]Transform _target = null;
    [SerializeField]float _minDistance = 1;
    [SerializeField]float _maxDistance = 10;
    [SerializeField]float _zoomSpeed = 5;
    [SerializeField]float _xSpeed = 200;
    [SerializeField]float _ySpeed = 200;
    [SerializeField]float _yMinLimit = -45;
    [SerializeField]float _yMaxLimit = 90;
    float _distance = 5f;
    float _autoDistance = 5f;
    float _manualDistance = 5f;
    float _x = 0.0f;
    float _y = 0.0f;
    bool _dragging;

    void LateUpdate()
    {
        if (GameManager.character == null)
        {
            return;
        }

        _target = GameManager.character.transform.FindChild("Camera Target");

        _target.transform.LookAt(gameObject.transform);

        if (Input.GetAxis("Mouse ScrollWheel") != 0 && !EventSystem.current.IsPointerOverGameObject())
        {
            _manualDistance -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        }

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(_target.transform.position, _target.transform.forward, out hit, _manualDistance))
        {
            _autoDistance = hit.distance;
            _distance = _autoDistance;
        }
        else
        {
            _distance = Mathf.MoveTowards(_distance, _manualDistance, _zoomSpeed);
        }

        _manualDistance = Mathf.Clamp(_manualDistance, _minDistance, _maxDistance);
        _autoDistance = Mathf.Clamp(_autoDistance, _minDistance, _maxDistance);
        _distance = Mathf.Clamp(_distance, _minDistance, _maxDistance);

        if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && !EventSystem.current.IsPointerOverGameObject())
        {
            _dragging = true;
        }

        if (_dragging)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            _x += Input.GetAxis("Mouse X") * _xSpeed * 0.02f;
            _y -= Input.GetAxis("Mouse Y") * _ySpeed * 0.02f;

            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                GameManager.character.cameraMoving = true;
            }

            if (Input.GetButton("Fire2") && Input.GetAxis("Mouse X") != 0)
            {
                GameManager.character.transform.rotation = Quaternion.Euler(0.0f, _x, 0.0f);
            }

            _y = ClampAngle(_y, _yMinLimit, _yMaxLimit);

            transform.rotation = Quaternion.Euler(_y, _x, 0.0f);
            transform.position = transform.rotation * new Vector3(0.0f, 0.0f, -_distance) + _target.position;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            _x += Input.GetAxis("Horizontal") * _xSpeed * Time.deltaTime;

            transform.rotation = Quaternion.Euler(_y, _x, 0.0f);
            transform.position = transform.rotation * new Vector3(0.0f, 0.0f, -_distance) + _target.position;
        }

        if (!Input.GetButton("Fire2"))
        {
            GameManager.character.transform.Rotate(Vector3.up * _xSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
        }

        if (!Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
        {
            _dragging = false;
            GameManager.character.cameraMoving = false;
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }

        if (angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }
}