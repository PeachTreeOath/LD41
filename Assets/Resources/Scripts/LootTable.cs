using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class LootTableRow {
    public CardPrototype cardPrototype;
    public int cardsAvailable = -1;
    public int weight = 1;
}

public class LootTable : MonoBehaviour {

    public List<LootTableRow> drawTable;

    public CardModel PullCard() {
        var filteredRows = drawTable.Where(row => row.cardsAvailable != 0);

        int totalWeight = filteredRows.Sum(row => row.weight);
        int target = UnityEngine.Random.Range(0, totalWeight);
        int weight = 0;
        var foundRow = filteredRows.First(row => {
            weight += row.weight;
            return target < weight;
        });

        var card = foundRow.cardPrototype.Instantiate();
        if(foundRow.cardsAvailable > 0) {
            foundRow.cardsAvailable--; 
        }

        return card;
    }
}
