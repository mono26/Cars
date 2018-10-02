using UnityEngine;

public class BillboardGameObject : MonoBehaviour
{
    [Header("Billboard Image settings")]
    [SerializeField] private Camera cameraToBillBoardTo;
    [SerializeField] private Transform[] gameObjectsToBillboard;

    private void Awake()
    {
        if(cameraToBillBoardTo == null) {
            cameraToBillBoardTo = Camera.main;
        }
        return;
    }

    private void Update ()
    {
		foreach(Transform gameObject in gameObjectsToBillboard) {
            FaceTransformTowardsCamera(gameObject);
        }
        return;
	}

    private void FaceTransformTowardsCamera(Transform _gameObjectToTransform)
    {
        Vector3 cameraPosition = cameraToBillBoardTo.transform.position;
        _gameObjectToTransform.LookAt(cameraPosition);
        return;
    }
}
