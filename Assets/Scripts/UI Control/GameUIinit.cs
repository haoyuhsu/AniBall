using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIinit : MonoBehaviour
{
    public Sprite FoxSprite;
    public Sprite RacoonSprite;
    public Sprite CatSprite;
    public Sprite DogSprite;
    public Sprite FoxName;
    public Sprite RacoonName;
    public Sprite CatName;
    public Sprite DogName;
    public GameObject[] PlayerCanvas;
    public Image[] AnimalImages;
    public Image[] AnimalNameImages;
    GameSetting gameSetting;

    void Start()
    {
        gameSetting = FindObjectOfType<GameSetting> ();
        SetGameUI();
    }

    void SetGameUI()
    {
        int numPlayers = gameSetting.numPlayers;
        for (int i = 0; i < numPlayers; i++)
        {
            PlayerCanvas[i].SetActive(true);
            SetAnimals(i);
        }
    }

    void SetAnimals(int idx)
    {
        string animalName = gameSetting.playersAnimal[idx];
        if (animalName == "Fox")
        {
            AnimalImages[idx].sprite = FoxSprite;
            AnimalNameImages[idx].sprite = FoxName;
        }
        else if (animalName == "Racoon")
        {
            AnimalImages[idx].sprite = RacoonSprite;
            AnimalNameImages[idx].sprite = RacoonName;
        }
        else if (animalName == "Cat")
        {
            AnimalImages[idx].sprite = CatSprite;
            AnimalNameImages[idx].sprite = CatName;
        }
        else if (animalName == "Dog")
        {
            AnimalImages[idx].sprite = DogSprite;
            AnimalNameImages[idx].sprite = DogName;
        }
        AnimalNameImages[idx].SetNativeSize();
    }

}
