using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public InputActionAsset inputActions;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;

    private InputAction shootAction;

    private void OnEnable()
    {
        var onFootActionMap = inputActions.FindActionMap("OnFoot");
        shootAction = onFootActionMap.FindAction("Shoot");
        shootAction.Enable();

        shootAction.performed += Shoot;
    }

    private void OnDisable()
    {
        shootAction.performed -= Shoot;
        shootAction.Disable();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        FireBullet();
    }

    private void FireBullet()
    {
      GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
      Rigidbody rb = bullet.GetComponent<Rigidbody>();
      rb.velocity = firePoint.forward * bulletSpeed;
    }
}
