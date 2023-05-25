using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MapCamera : MonoBehaviour
{

    public struct MapItemInfo {
        public GameObject gameObject;
        public Vector2 position;
        public float rotation;
    }

    public Transform playerTransform;

    private Vector3 offset;

    public float moveSpeed = 2.5f;


    public Canvas mapCanvas;

    public RectTransform mapRectTransform;

    public RawImage mapImage;


    public UnityEngine.Camera mapCamera;
    public GameObject playerUIIcon;

    void Start()
    {
        offset = transform.position - playerTransform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + offset, moveSpeed * Time.deltaTime);
        UpdatePlayerIcon();

    }

    public MapItemInfo GetGameObjectUIPosition(GameObject worldObject)
    {
        Vector3 screenPos = mapCamera.WorldToScreenPoint(worldObject.transform.position);
        Debug.Log("Screen position: " + screenPos);
        Vector2 screenPos2D = ScreenToMapPoint(new Vector2(screenPos.x, screenPos.y));
        return new MapItemInfo {
            gameObject = worldObject,
            position = screenPos2D,
            rotation = worldObject.transform.rotation.eulerAngles.y
        };
        
    }

    public Vector3 ScreenToMapPoint(Vector2 screenPoint) {
        Vector3 mapPositon = mapImage.gameObject.transform.position;
        Vector2 mapSize = mapImage.rectTransform.sizeDelta;
        //map top left corner
        mapPositon.x -= mapSize.x / 2;
        mapPositon.y -= mapSize.y / 2;
        return new Vector3(mapPositon.x + screenPoint.x, mapPositon.y - screenPoint.y, mapPositon.z);
    }

    public void UpdatePlayerIcon() {
        MapItemInfo playerInfo = GetGameObjectUIPosition(playerTransform.gameObject);
        playerUIIcon.transform.position = playerInfo.position;
        playerUIIcon.transform.rotation = Quaternion.Euler(0, 0, playerInfo.rotation);
        
    }
}
