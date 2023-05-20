namespace Markwardt;

public class SpriteBehavior : UnityBehavior
{
    public override void Start()
    {
        GameObject.AddComponent<SpriteRenderer>();
    }
}