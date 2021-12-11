using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2D;
    PhotonView PV;

    [SerializeField] float speed;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        PV = GetComponent<PhotonView>();
    }

    void Update()
    {
        InputProcess();
    }

    private void InputProcess()
    {
        if (PV.IsMine)
        {
            rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
            PlayerLookAtMouse();
        }
    }

    private void PlayerLookAtMouse()
    {
        var dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0, angle +90);
    }
}
