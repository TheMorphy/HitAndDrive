using UnityEngine;
[System.Serializable]
public class CarChange 
{
    public int levelToReach;
    public GameObject newCarModel;
    [HideInInspector] public bool changed = false;
    public string animationName;
    public string levelupAnim;
    public string CoroutineName;
    public int index = 0;
    public Texture normalTexture, feverTexture;
    public MeshRenderer mainMatMeshRenderer;
    public int materialIndex;
    public CarController.carType type;
}
