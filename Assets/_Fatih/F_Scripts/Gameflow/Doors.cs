using System.Collections;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [SerializeField] Transform doorTransform; 
    [SerializeField] float openAngle = 90f;     
    [SerializeField] float openSpeed = 2f;   
    private bool isOpen = false;    
    private Quaternion closedRotation; 
    private Quaternion openRotation;   

    void Start()
    {
        closedRotation = doorTransform.localRotation; 
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    public void DoorMovements()
    {
        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openRotation : closedRotation));
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(doorTransform.localRotation, targetRotation) > 0.01f)
        {
            doorTransform.localRotation = Quaternion.Lerp(doorTransform.localRotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }
    }
}


