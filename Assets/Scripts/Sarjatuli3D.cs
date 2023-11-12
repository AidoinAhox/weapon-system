using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//public TextMeshProUGUI ammusTeksti; // Lis‰‰ t‰m‰ Unityss‰ tekstikomponenttina
//public TextMeshProUGUI lipasTeksti; // Lis‰‰ t‰m‰ Unityss‰ tekstikomponenttina

public class Sarjatuli3D : MonoBehaviour

{
    public GameObject ammusPrefab;
    public Transform ampumispiste;
    public float ampumisvoima = 10f;
    public float ampumisTaajuus = 0.1f; // Aikav‰li ampumisen v‰lill‰
    public float latausaika = 1f;
    public int maxAmmukset = 120;
    public int lipasKoko = 30;

    private float viimeisinAmpumisaika = 0f;
    private int jalkivaAmmukset;
    private int jalkivaLipasAmmukset;
    private bool latausK‰ynniss‰ = false;

    public TextMeshProUGUI ammusTeksti; // Lis‰‰ t‰m‰ Unityss‰ tekstikomponenttina
    public TextMeshProUGUI lipasTeksti; // Lis‰‰ t‰m‰ Unityss‰ tekstikomponenttina

    void Start()
    {
        jalkivaAmmukset = maxAmmukset;
        jalkivaLipasAmmukset = lipasKoko;

        // Alusta UI-tekstit
        P‰ivit‰AmmusTeksti();
        P‰ivit‰LipasTeksti();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > viimeisinAmpumisaika + ampumisTaajuus && jalkivaLipasAmmukset > 0)
        {
            Ampu();
            viimeisinAmpumisaika = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.R) && !latausK‰ynniss‰)
        {
            StartCoroutine(LataaLipas());
        }
    }

    void Ampu()
    {
        GameObject ammus = Instantiate(ammusPrefab, ampumispiste.position, ampumispiste.rotation);
        Rigidbody ammusRb = ammus.GetComponent<Rigidbody>();

        // Aseta ammuksen nopeus
        ammusRb.AddForce(ampumispiste.forward * ampumisvoima, ForceMode.Impulse);

        VahennaAmmus();
        VahennaLipasAmmus();
    }

    void VahennaAmmus()
    {
        jalkivaAmmukset--;

        // Aseta ammusten m‰‰r‰ v‰hint‰‰n nollaan
        jalkivaAmmukset = Mathf.Max(jalkivaAmmukset, 0);

        // P‰ivit‰ UI-teksti
        P‰ivit‰AmmusTeksti();

        if (jalkivaAmmukset <= 0)
        {
            // Voit lis‰t‰ toiminnallisuuden lataamiseen tai muuhun k‰sittelyyn
            // esimerkiksi latauspistooliin tai n‰ytt‰‰ pelaajalle, ett‰ ammukset ovat loppu.
            Debug.Log("Ammukset loppu!");
        }
    }



    void VahennaLipasAmmus()
    {
        jalkivaLipasAmmukset--;

        // Aseta lippaan ammusten m‰‰r‰ v‰hint‰‰n nollaan
        jalkivaLipasAmmukset = Mathf.Max(jalkivaLipasAmmukset, 0);

        // P‰ivit‰ UI-teksti
        P‰ivit‰LipasTeksti();
    }

    IEnumerator LataaLipas()
    {
        latausK‰ynniss‰ = true;

        // Odota m‰‰ritelty aika
        yield return new WaitForSeconds(latausaika);

        // Lataa lipas, jos kokonaispanosm‰‰r‰ on viel‰ j‰ljell‰
        if (maxAmmukset > 0)
        {
            int ladattavatAmmukset = Mathf.Min(lipasKoko - jalkivaLipasAmmukset, maxAmmukset);
            jalkivaLipasAmmukset += ladattavatAmmukset;
            maxAmmukset -= ladattavatAmmukset;
        }

        // P‰ivit‰ UI-teksti
        P‰ivit‰LipasTeksti();
        P‰ivit‰AmmusTeksti();

        latausK‰ynniss‰ = false;
    }

    void P‰ivit‰AmmusTeksti()
    {
        // P‰ivit‰ UI-teksti j‰ljell‰ olevien ammusten m‰‰r‰ll‰
        if (ammusTeksti != null)
        {
            ammusTeksti.text = "Ammukset: " + jalkivaAmmukset.ToString();
        }
    }

    void P‰ivit‰LipasTeksti()
    {
        // P‰ivit‰ UI-teksti j‰ljell‰ olevien lippaan ammusten m‰‰r‰ll‰
        if (lipasTeksti != null)
        {
            lipasTeksti.text = "Lipas: " + jalkivaLipasAmmukset.ToString();
        }
    }
}
