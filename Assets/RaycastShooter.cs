using UnityEngine;
using UnityEngine.UI;

public class RaycastShooter : MonoBehaviour
{
    public Transform shootPoint; // Điểm bắn raycast, có thể kéo thả trong Inspector
    public Camera mainCamera; // Camera để chuyển đổi vị trí chuột thành vị trí thế giới
    public RawImage crosshair; // Hình crosshair
    public float raycastDistance = 100f; // Độ dài của Raycast có thể chỉnh trong Inspector
    public Color defaultColor = Color.white;
    public Color hitZombieColor = Color.red;
    public Color hitZombieHeadColor = Color.green;
    private Vector3 savedHitPosition; // Biến lưu tọa độ khi bắn trúng

    void Start()
    {
        if (crosshair != null)
        {
            crosshair.color = defaultColor;
        }
    }

    void Update()
    {
        ShootRaycast();
        CheckMouseClick();
    }

    void ShootRaycast()
    {
        if (shootPoint == null || mainCamera == null) return;

        // Lấy vị trí con trỏ chuột trên màn hình
        Vector3 mousePosition = Input.mousePosition;

        // Bắn một tia từ camera để lấy vị trí tương ứng trong thế giới
        Ray cameraRay = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit cameraHit;

        Vector3 targetPoint = shootPoint.position + shootPoint.forward * raycastDistance; // Sử dụng raycastDistance từ Inspector
        if (Physics.Raycast(cameraRay, out cameraHit, raycastDistance))
        {
            targetPoint = cameraHit.point; // Nếu ray từ camera chạm vào vật thể, lấy vị trí chạm đó
        }

        // Bắn một raycast từ shootPoint đến targetPoint
        Vector3 direction = (targetPoint - shootPoint.position).normalized;
        Ray ray = new Ray(shootPoint.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            // Kiểm tra tag của đối tượng trúng raycast
            if (hit.collider.CompareTag("ZombieHead"))
            {
                crosshair.color = hitZombieHeadColor;
            }
            else if (hit.collider.CompareTag("Zombie") || hit.collider.CompareTag("ZombieBody"))
            {
                crosshair.color = hitZombieColor;
            }
            else
            {
                crosshair.color = defaultColor;
            }
        }
        else
        {
            crosshair.color = defaultColor;
        }
    }

    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // Nếu người chơi bấm chuột trái
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray cameraRay = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit cameraHit;

            Vector3 targetPoint = shootPoint.position + shootPoint.forward * raycastDistance;
            if (Physics.Raycast(cameraRay, out cameraHit, raycastDistance))
            {
                targetPoint = cameraHit.point;
            }

            Vector3 direction = (targetPoint - shootPoint.position).normalized;
            Ray ray = new Ray(shootPoint.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                savedHitPosition = hit.point;
                Debug.Log("Hit Object: " + hit.collider.gameObject.name + " at Position: " + savedHitPosition);
            }
            else
            {
                savedHitPosition = shootPoint.position + direction * raycastDistance;
                Debug.Log("No object hit. Raycast endpoint: " + savedHitPosition);
            }
        }
    }
}
