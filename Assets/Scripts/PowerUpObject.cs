using UnityEngine;

[CreateAssetMenu(fileName = "PowerUp", menuName = "PowerUp")]
public class PowerUpObject : ScriptableObject
{
    public enum Type
    {
        ROCKET,
        LASER,
        SHIELD,
    };
    public new string name;
    public Type type;
    public Sprite sprite;
    public float duration;

}
