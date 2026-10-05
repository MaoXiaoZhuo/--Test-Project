using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // 跟随目标（方块）
    public Vector3 offset = new Vector3(0, 1.7f, 0); // 眼睛高度
    public float mouseSensitivity = 2f; // 鼠标灵敏度

    private float pitch = 0f; // 上下俯仰角度

    void Start()
    {
        // 初始朝向目标
        Vector3 dir = target.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        pitch = transform.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // --- 鼠标扭头 ---
        float mouseX = 0f;
        float mouseY = 0f;

#if ENABLE_INPUT_SYSTEM
        var mouse = Mouse.current;
        if (mouse != null)
        {
            mouseX = mouse.delta.ReadValue().x * mouseSensitivity * 0.1f;
            mouseY = mouse.delta.ReadValue().y * mouseSensitivity * 0.1f;
        }
#else
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
#endif

        // 左右旋转：直接转相机自己
        transform.Rotate(0, mouseX, 0, Space.World);

        // 上下旋转：限制角度，别翻过头
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        transform.localEulerAngles = new Vector3(pitch, transform.localEulerAngles.y, 0);

        // --- 位置跟随 ---
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}