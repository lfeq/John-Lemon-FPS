using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public Camera camera;
    public Animator animator;
    public AudioSource audioPistola;
    public AudioClip disparoClip;
    public AudioClip recargaClip;
    public GameObject gunGO;
    public LayerMask enemyLayer;
    public float shootInterval = 0.2f; //Tiempo para volver a disparar
    public int maxBulletsPerRound = 7; //Maximo de balas que puede disparar antes de recargar
    public float reloadTime = 3f; //Tiempo de recarga
    public float damage = 2.3f;
    public int bulletsInBox = 14;
    public bool armed; //Checar si el jugador esta armado
    public int totalBullets = 0; //Total de balas que se tiene

    private float shootCooldown = 0;
    private TMP_Text bulletCountText;
    private Image reloadImage;
    private int currentBullets; //Balas que se tiene actualmente
    private float reloadCooldown = 0;
    private bool reloading = false;

    // Start is called before the first frame update
    void Start()
    {
        bulletCountText = GameObject.Find("Cuentadebalas").GetComponent<TMP_Text>();
        reloadImage = GameObject.Find("ReloadImage").GetComponent<Image>();
        currentBullets = maxBulletsPerRound;
        bulletCountText.text = "";
        reloadImage.gameObject.SetActive(false);
        EventManagerLvl1.current.pickedUpGun += PickUpGun;
        EventManagerLvl1.current.pickedUpBulllets += PickUpBullets;
    }

    // Update is called once per frame
    void Update()
    {
        if (armed)
        {
            if (Input.GetKeyUp(KeyCode.R) && totalBullets > 0 && !reloading)
            {
                if (currentBullets != maxBulletsPerRound)
                {
                    Reload();
                }
            }

            if (Input.GetMouseButtonDown(0) && shootCooldown <= 0 && currentBullets > 0 && !reloading)
            {
                animator.SetTrigger("New Trigger");
                ReproducirAudio(audioPistola, disparoClip, false);
                RaycastHit whatIHit;
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                shootCooldown = shootInterval;
                currentBullets--;

                Transform cameraTransform = Camera.main.transform;

                if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out whatIHit, 100.0f, enemyLayer))
                {
                    Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100.0f, Color.yellow);
                    if (whatIHit.collider.tag == "Enemy")
                    {
                        WaypointPatrol enemy = whatIHit.collider.GetComponent<WaypointPatrol>();
                        enemy.TakeDamage(damage);
                    }
                }

                if (currentBullets == 0)
                {
                    Reload();
                } //Se acabaron las balas
            } //Disparar

            else
            {
                shootCooldown -= Time.deltaTime;
            } //Tiempo de espera para volver a disparar

            if (reloading)
            {
                reloadCooldown += Time.deltaTime;
                float reloadRemaining = reloadCooldown / reloadTime;
                reloadImage.fillAmount = reloadRemaining;

                if (reloadCooldown >= reloadTime)
                {
                    reloadImage.gameObject.SetActive(false);
                    int savedBullets = maxBulletsPerRound - currentBullets;

                    totalBullets -= savedBullets;

                    if (totalBullets == 0)
                    {
                        currentBullets = 0;
                        reloading = false;
                    }

                    else
                    {
                        currentBullets = maxBulletsPerRound;
                        reloading = false;
                    }

                    if (totalBullets <= 0)
                    {
                        totalBullets = 0;
                    } //Se acabaron las balas
                } //Termino de recargar
            } //Esta recargando

            bulletCountText.text = currentBullets.ToString() + "/" + totalBullets.ToString();
        }      
    }

    void Reload()
    {
        reloadImage.gameObject.SetActive(true);
        animator.SetTrigger("New Trigger 0");
        ReproducirAudio(audioPistola, recargaClip, true);
        reloading = true;
        reloadCooldown = 0;
    }

    void ReproducirAudio(AudioSource source, AudioClip clip, bool waitForClipToEnd)
    {
        if (waitForClipToEnd)
        {
            StartCoroutine(playEngineSound(clip));
        }
        else
        {
            source.clip = clip;
            source.Play();
        }

    }

    void PickUpGun()
    {
        gunGO.SetActive(true);
        totalBullets = 14;
        armed = true;
    }

    void PickUpBullets()
    {
        totalBullets += bulletsInBox;
    }

    IEnumerator playEngineSound(AudioClip clip)
    {
        yield return new WaitForSeconds(audioPistola.clip.length);
        audioPistola.clip = clip;
        audioPistola.Play();
    }
}
