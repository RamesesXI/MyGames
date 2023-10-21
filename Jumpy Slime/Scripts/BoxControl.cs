using UnityEngine;
using System.Collections;


public enum BoxSize
{
    Small,
    Medium,
    Big
}

public class BoxControl : MonoBehaviour
{
    public BoxSize DefaultSize = BoxSize.Small;
    public BoxSize Size = BoxSize.Medium;

    public BoxSize CurrentSize
    {
        get
        {
            return Size;
        }
        set
        {
            Size = value;
            SetSpriteBySize(value);
        }
    }

    private SpriteRenderer sr;
    public Sprite BigSprite;
    public Sprite SmallSprite;
    public Sprite MediumSprite;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SetSpriteBySize(Size);
    }

    void SetSpriteBySize(BoxSize bs)
    {
        if (bs == BoxSize.Big)
        {
            sr.sprite = BigSprite;
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if (bs == BoxSize.Medium)
        {
            sr.sprite = MediumSprite;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            sr.sprite = SmallSprite;
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        }
    }

    public GameObject Player;
    public bool Jumped = false;

    public void PlayerHitBox(GameObject player)
    {
        
        if (player.name == Player.name)
        {
            // if already jumped
            if (Jumped)
            {
                if (CurrentSize == BoxSize.Small)
                    CurrentSize = BoxSize.Medium; // is medium 
                else if (CurrentSize == BoxSize.Medium && DefaultSize == BoxSize.Big) // is medium  && default big
                    CurrentSize = BoxSize.Big;
                else if (CurrentSize == BoxSize.Medium && DefaultSize == BoxSize.Medium) // is medium && default medium
                    CurrentSize = BoxSize.Small;
                else CurrentSize = BoxSize.Medium;

            }
            else if (CurrentSize == BoxSize.Small) // Small by default
                Destroy(this.gameObject);
            else if (CurrentSize == BoxSize.Medium)
            {
                CurrentSize = BoxSize.Small;
                if (DefaultSize == BoxSize.Big)
                    DefaultSize = BoxSize.Medium;
               
                Jumped = true;
            }
            else
            {
                CurrentSize = BoxSize.Medium;
                Jumped = true;
            }
        }

    }
}
