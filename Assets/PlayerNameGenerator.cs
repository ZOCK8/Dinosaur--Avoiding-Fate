// PlayerNameGenerator.cs - Generiert zufällige Spielernamen
using UnityEngine;

public static class PlayerNameGenerator
{

    private static readonly string[] adjectives = {
    "Swift", "Mighty", "Clever", "Brave", "Fierce", "Thunder", "Shadow", "Fire", "Ice", "Iron",
    "Wild", "Noble", "Quick", "Strong", "Wise", "Bold", "Dark", "Light", "Steel", "Crystal"

    };


    private static readonly string[] nouns = {
    "Tyranno", "Rex", "Raptor", "Stego", "Trice", "Ankylo", "Spino", "Ptero", "Brachio", "Allo",
    "Veloci", "Dino", "Giga", "Sauro", "Mega", "Chasmo", "Cera", "Iguanodon", "Pachy", "Deino"

    };


    public static string GenerateRandomName()
    {
        string adjective = adjectives[Random.Range(0, adjectives.Length)];
        string noun = nouns[Random.Range(0, nouns.Length)];
        int number = Random.Range(100, 999);
        
        return $"{adjective}{noun}{number}";
    }
}