using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FuelManager : MonoBehaviour
{
    public static FuelManager instance;
    UnityEvent fuelEmpty = new UnityEvent();
    public UnityEvent FuelEmptyEvent { get => fuelEmpty; }

    [SerializeField]float fuelLevel = 100;

    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);

        instance = this;


    }

    public void Start()
    {
        StartCoroutine(FuelDecrement());
    }
    IEnumerator FuelDecrement()
    {
        WaitForSeconds wait = new WaitForSeconds(0.33f);

        while (fuelLevel > 0)
        {
            yield return wait; // wait 0.33 sec, then decrement fuel and iterate.
            fuelLevel--;
        }
        fuelEmpty.Invoke();
        yield return null;

    }
}
