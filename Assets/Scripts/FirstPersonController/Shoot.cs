using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shoot : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioPistola;
    public AudioClip disparoClip;
    public AudioClip recargaClip;
    public float shootInterval = 0.2f; //Tiempo para volver a disparar
    public int maxBulletsPerRound = 7; //Maximo de balas que puede disparar antes de recargar
    public int totalBullets = 14; //Total de balas que se tiene
    public float reloadTime = 3f; //Tiempo de recarga
    public bool armed;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (armed)
        {
            if (Input.GetKeyUp(KeyCode.R) && totalBullets > 0)
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
                shootCooldown = shootInterval;
                currentBullets--;

                if (Physics.Raycast(transform.position, transform.forward, out whatIHit, Mathf.Infinity))
                {
                    //Debug.Log(whatIHit.collider.name);
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

    IEnumerator playEngineSound(AudioClip clip)
    {
        yield return new WaitForSeconds(audioPistola.clip.length);
        audioPistola.clip = clip;
        audioPistola.Play();
    }
}
