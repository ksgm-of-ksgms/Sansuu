using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetBoardController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    [SerializeField] GameObject impactVFX;

    [SerializeField] Vector3 impactVFXOffset;

    [SerializeField] float impactVFXLifetime = 3f;

    [SerializeField] GameObject impactSFX;
    [SerializeField] AudioClip impactSFXClip;

    event System.Action OnBang;


    void Start()
    {
    }

    void Update()
    {
    }

    public void RegisterBangListener(System.Action callback)
    {
        OnBang += callback;
    }

    public void Bang()
    {
        if (impactVFX)
        {
            GameObject impactVFXInstance = Instantiate(impactVFX, gameObject.transform.position + impactVFXOffset, gameObject.transform.rotation);
            Destroy(impactVFXInstance.gameObject, impactVFXLifetime);
        }
 
        if (impactSFXClip)
        {
            GameObject impactSFXInstance = Instantiate(impactSFX, gameObject.transform.position + impactVFXOffset, gameObject.transform.rotation);
            Destroy(impactSFXInstance.gameObject, impactVFXLifetime);
        }

        OnBang?.Invoke();
    }

    public void SetVisible(bool b)
    {
        gameObject.SetActive(b);
    }
    public void Kill()
    {
        Destroy(gameObject);
    }

    public void SetText(string s)
    {
        text.text = s;
    }

    public string GetText()
    {
        return text.text;
    }
}
