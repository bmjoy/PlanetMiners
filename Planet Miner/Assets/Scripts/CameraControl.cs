using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private Camera _camera;
    [Space]
    [Header("Move speed")]
    [SerializeField]
    private float _moveSpeed;
    [SerializeField]
    private Vector3 _moveDepth;
    [SerializeField]
    private Vector3 _rotationSpeed;



    void Update()
    {
        //A D left right
        if(Input.GetAxis("Horizontal") < 0)
        {
            //_camera.transform.Translate(-1 * _moveHorizontal * Time.deltaTime,Space.World);
            _camera.transform.Translate(-1 * _moveSpeed * _camera.transform.right * Time.deltaTime, Space.World);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            //_camera.transform.Translate(_moveHorizontal * Time.deltaTime, Space.World);
            _camera.transform.Translate(_moveSpeed * _camera.transform.right * Time.deltaTime, Space.World);
        }
        //W S up down
        if (Input.GetAxis("Vertical") > 0)
        {
            //_camera.transform.Translate(_moveVertical * Time.deltaTime, Space.World);
            Vector3 forward = _camera.transform.forward;
            forward.y = 0;
            _camera.transform.Translate(_moveSpeed * forward * Time.deltaTime, Space.World);
        }
        if (Input.GetAxis("Vertical") < 0)
        {
            //_camera.transform.Translate(-1 * _moveVertical * Time.deltaTime, Space.World);
            Vector3 forward = _camera.transform.forward;
            forward.y = 0;
            _camera.transform.Translate(_moveSpeed * forward * Time.deltaTime  * -1, Space.World);
        }
        //Scroll up and down
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _camera.transform.Translate(-1 * _moveDepth * Time.deltaTime, Space.World);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            _camera.transform.Translate(_moveDepth * Time.deltaTime, Space.World);
        }

        //Rotate left q, rotate right e
        if (Input.GetKey(KeyCode.Q))
        {
            _camera.transform.Rotate(-1 *_rotationSpeed * Time.deltaTime,Space.World);
        }

        if (Input.GetKey(KeyCode.E))
        {
            _camera.transform.Rotate(_rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
