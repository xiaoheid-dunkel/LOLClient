using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {


  
    public spriteSlider hp;
    private uint value=1;

	// Use this for initialization
	void Start () {
        hp=GetComponentInChildren<spriteSlider>();
        Init();
	}

    public void Init()
    {
        hp.Value = value;
    }

    public void TakeDamage(float damage)
    {
        hp.Value -= damage;
    }
	
}
