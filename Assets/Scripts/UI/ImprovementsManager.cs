using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovementsManager : MonoBehaviour
{
    [SerializeField] private List<ImprovementSO> improvements;
    [SerializeField] private ImprovementListItem template;
    [SerializeField] private GameObject improvementsList;

    private void Start()
    {
        for (int i = 0; i < improvements.Count; i++)
        {
            AddItemToList(improvements[i]);
        }
    }

    private void AddItemToList(ImprovementSO imp)
    {
        ImprovementListItem improvementListItem = Instantiate(template, improvementsList.transform);
        improvementListItem.RenderListItem(imp.Icon, imp.ItemName, imp.ItemEffect, imp.ItemDescription, imp.ItemPrice, imp.effect);
    }

    public void ImprovePlayerSpeed(int price)
    {
        TavernEventsManager.TryToSpendCoins(TavernEventsManager.SpeedImproved, price);

    }

    public void ImproveCoinsIncome(int price)
    {
        TavernEventsManager.TryToSpendCoins(TavernEventsManager.BeerIncomeImproved, price);

    }

    public void ImproveHappinessDrop(int price)
    {
        TavernEventsManager.TryToSpendCoins(TavernEventsManager.HappinessImproved, price);
    }

    public void NextButtonClickHandler()
    {
        GameManager.UpdateGameState(GameManager.GameState.Day);
    }
}
