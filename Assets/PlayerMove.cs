using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform; // 相机

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = 0f;
        float v = 0f;

#if ENABLE_INPUT_SYSTEM
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) h = -1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) h = 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) v = -1f;
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) v = 1f;
        }
#else
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
#endif

        // 根据相机朝向移动
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * v + right * h).normalized * speed;
        // 用物理速度移动，才能触发碰撞
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
    }
}