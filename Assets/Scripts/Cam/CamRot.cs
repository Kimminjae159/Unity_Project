using UnityEngine;
// 마우스로 오브젝트 회전 제어
public class CamRot : MonoBehaviour
{
    public float mouseSpd = 200f;   // 마우스 감도
    float mx = 0f;  // 마우스 x값을 저장
    float my = 0f;  // 마우스 y값을 저장

    public bool canRotX;
    public bool canRotY;

    // Update is called once per frame
    void Update()
    {
        // 마우스의 움직임에 대한 값을 받아오자.
        float mouse_x = Input.GetAxis("Mouse X");
        float mouse_y = Input.GetAxis("Mouse Y");
        // P = p0 + vt
        if(canRotY) mx = mx + mouse_x * mouseSpd * Time.deltaTime;
        if(canRotX) my = my + mouse_y * mouseSpd * Time.deltaTime;
        // 값을 제한한다. (제한할 변수, min, max)
        my = Mathf.Clamp(my, -90, 90);
        transform.localEulerAngles = new Vector3(-my, mx, 0);
    }
}