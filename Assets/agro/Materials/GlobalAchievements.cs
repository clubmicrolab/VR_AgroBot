using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class GlobalAchievements : MonoBehaviour
{
    public GameObject notaAch;
    public AudioSource sunetAch;
    public bool achActiv = false;
    public GameObject titluAch;
    public GameObject descriereAch;

    public GameObject imagineAch01;
    public static int numarAch01;
    public int declansatorAch01 = 5;
    public int codAch01;

    void Update()
    {
        codAch01 = PlayerPrefs.GetInt("Ach01");
        if (numarAch01 == declansatorAch01 && codAch01 != 12345)
        {
            StartCoroutine(DeclanseazaAch01());
        }
    }

    IEnumerator DeclanseazaAch01()
    {
        achActiv = true;
        codAch01 = 12345;
        PlayerPrefs.SetInt("Ach01", codAch01);
        sunetAch.Play();
        imagineAch01.SetActive(true);
        titluAch.GetComponent<Text>().text = "Nevoie de Viteză";
        descriereAch.GetComponent<Text>().text = "Uau, de necrezut, ai ajuns cu adevărat la acest punct!";
        notaAch.SetActive(true);
        yield return new WaitForSeconds(7);
        notaAch.SetActive(false);
        imagineAch01.SetActive(false);
        titluAch.GetComponent<Text>().text = "";
        descriereAch.GetComponent<Text>().text = "";
        achActiv = false;
    }
}
