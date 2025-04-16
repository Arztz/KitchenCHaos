using System;
using UnityEngine;
using UnityEngine.UI;

public class DeliverManagerUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCpmpleted;

        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeCpmpleted(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTemplate.gameObject.SetActive(true);
        }
    }
}
