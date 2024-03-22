using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    [Header("MosueCursor")]
    // 메인카메라의 마우스 커서 이미지를 원하는 이미지로 바꿈.
    [SerializeField] Texture2D cursorImage;
    [SerializeField] Texture2D cursorClickImage;


    void Start()
    {
        // 처음 마우스 이미지 설정.
        Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
    }

    
    void Update()
    {
        // 마우스를 눌렀을 때.
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorClickImage, Vector2.zero, CursorMode.Auto);
        }
        // 마우스를 누르지 않았을 때.
        else if(!Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
        }
    }
}
