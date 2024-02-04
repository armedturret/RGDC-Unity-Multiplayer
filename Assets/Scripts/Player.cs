using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float sensitivity = 0.1f;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private GameObject bulletPrefab;

    private CharacterController _characterController;
    float _rotY = 0f;

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.parent = cameraTransform;
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.localRotation = Quaternion.identity;
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;

            Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            _characterController.Move(transform.TransformDirection(moveInput.normalized) * speed * Time.deltaTime);

            //look
            _rotY += Input.GetAxis("Mouse X") * sensitivity;
            float newX = cameraTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * sensitivity;

            if (newX > 180f)
            {
                newX = Mathf.Clamp(newX, 360f - 89.99f, 370f);
            }
            else
            {
                newX = Mathf.Clamp(newX, -10f, 89.99f);
            }

            cameraTransform.localRotation = Quaternion.Euler(newX, 0f, 0f);
            transform.rotation = Quaternion.Euler(0f, _rotY, 0f);

            if (Input.GetButtonDown("Fire1"))
            {
                CmdFire();
            }
        }
    }

    [Command]
    public void CmdFire()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = cameraTransform.position;
        NetworkServer.Spawn(bullet);
        bullet.GetComponent<Bullet>().Fire(cameraTransform.forward);
    }
}
