using System.Collections.Generic;
using UnityEngine;

public class RecipeDatabase : ScriptableObject
{
    [SerializeField] private List<DrinkRecipe> _recipes;

    // Returns the result prefab if a match is found, otherwise null
    public GameObject FindResult(GameObject a, GameObject b)
    {
        foreach (var recipe in _recipes)
        {
            bool matchAB = IsSamePrefab(a, recipe.ingredientA) &&
                           IsSamePrefab(b, recipe.ingredientB);
            bool matchBA = IsSamePrefab(a, recipe.ingredientB) &&
                           IsSamePrefab(b, recipe.ingredientA);

            if (matchAB || matchBA)
                return recipe.result;
        }

        return null;
    }

    // Compares by prefab name — simple and reliable for your level
    private bool IsSamePrefab(GameObject instance, GameObject prefab)
    {
        if (instance == null || prefab == null) return false;
        return instance.name.Replace("(Clone)", "").Trim() == prefab.name;
    }

}
