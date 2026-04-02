using UnityEngine;

public class DestructVisual : MonoBehaviour
{
    [SerializeField] private Destuct destuct;
    [SerializeField] private GameObject bushDeathVFX;

    private void Start()
    {
        destuct.OnDestructibleTakeDamage += DestructiblePlant_OnDestructibleTakeDamage;
    }

    private void DestructiblePlant_OnDestructibleTakeDamage(object sender, System.EventArgs e)
    {
        ShowDeathVFX();
    }

    private void ShowDeathVFX()
    {
        Instantiate(bushDeathVFX, destuct.transform.position, Quaternion.identity);
    }

    private void OnDestroy()
    {
        destuct.OnDestructibleTakeDamage -= DestructiblePlant_OnDestructibleTakeDamage;
    }
}
