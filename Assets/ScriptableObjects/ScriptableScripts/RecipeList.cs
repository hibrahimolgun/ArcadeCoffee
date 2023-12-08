using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeList", menuName = "ScriptableObjects/RecipeList", order = 1)]
public class RecipeList : ScriptableObject
{
    public List<Recipes> recipes;

    [Serializable]
    public struct Recipes
    {
        public int input1ID;
        public int input2ID;

        public int outputID;
    }
}
