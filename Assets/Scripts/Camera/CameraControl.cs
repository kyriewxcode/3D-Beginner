using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    /// <summary>
    /// 相机防挡，跟随控制
    /// </summary>

    public Transform targetFollow;
    Vector3 offset;
    public float rotateSpeed;
    public float moveSpeed;

    private void Start()
    {
        offset = transform.position - targetFollow.position;
    }
#if false
    private void LateUpdate()
    {
        //transform.position = targetFollow.position + offset;
        float dis = Vector3.Distance(transform.position, targetFollow.position);
        Debug.Log((dis * followSpeed / 8));
        Vector3 newPos = Vector3.Lerp(transform.position, targetFollow.position + offset
                        , Time.deltaTime * (dis/8 + followSpeed));
        transform.position = newPos;
    }
#else
    /// <summary>
    /// 设置三个插值点，移动相机直到能看到角色
    /// </summary>
    public void LateUpdate()
    {
        Vector3 beginPos = offset + targetFollow.position;
        Vector3 endPos = offset.magnitude * Vector3.up + targetFollow.position;
        Vector3 pos1 = Vector3.Lerp(beginPos, endPos, 0.25f);
        Vector3 pos2 = Vector3.Lerp(beginPos, endPos, 0.5f);
        Vector3 pos3 = Vector3.Lerp(beginPos, endPos, 0.75f);
        Vector3[] posArray = new Vector3[] { beginPos, pos1, pos2, pos3, endPos };
        Vector3 targetPos = posArray[0];
        for (int i = 0; i < posArray.Length; i++)
        {
            if (Physics.Raycast(posArray[i], GM.myPlayer.position - posArray[i], out RaycastHit hitInfo))
            {
                if (hitInfo.transform != GM.myPlayer)
                {
                    continue;
                }
                else
                {
                    targetPos = posArray[i];
                    break;
                }
            }
            else
            {
                targetPos = posArray[i];
                break;
            }
        }

        Quaternion nowRotation = transform.rotation;
        transform.LookAt(targetFollow);
        transform.rotation = Quaternion.Lerp(nowRotation, transform.rotation, rotateSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

    }
#endif
}
