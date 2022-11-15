using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UnitBehaviour : MonoBehaviour
{
    public Unit unit;

    protected SpriteRenderer spriteRenderer;
    protected BoxCollider2D collider2D;
    protected GameObject descriptionBox;
    protected TextMeshPro descriptionTextMesh;
    protected GameObject statsBox;
    protected TextMeshPro statsTextMesh;

    protected Vector3 pos;

    public void SetParameters(Unit unit, Vector3 pos, GameObject descriptionBox, GameObject statsBox)
    {
        this.unit = unit;
        this.descriptionBox = descriptionBox;
        this.statsBox = statsBox;

        ChangePos(pos);
    }

    void Awake()
    {
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        collider2D = gameObject.AddComponent<BoxCollider2D>();
    }

    protected virtual void Start()
    {
        descriptionTextMesh = descriptionBox.GetComponentInChildren<TextMeshPro>();
        statsTextMesh = statsBox.GetComponent<TextMeshPro>();

        descriptionTextMesh.text = unit.description;
        spriteRenderer.sprite = unit.sprite;
        collider2D.size = spriteRenderer.bounds.size;
        collider2D.isTrigger = true;
        RefreshStatsBox();

        ToggleDescription(false);

        StartCoroutine(Fade());
    }

    protected void RefreshStatsBox()
    {
        statsTextMesh.SetText(unit.GetStatsText());
    }

    public void Delete()
    {
        Destroy(this.gameObject);
    }

    void OnMouseDown()
    {
        OnClick();
    }

    void OnMouseDrag()
    {
        OnDrag();
    }

    void OnMouseUp()
    {
        OnRelease();
        ChangePos(pos);
    }

    void OnMouseEnter()
    {
        ToggleDescription(true);
    }

    void OnMouseExit()
    {
        ToggleDescription(false);
    }

    void OnClick()
    {
        
    }

    void OnDrag()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.Translate(mousePosition);
    }

    protected virtual void OnRelease()
    {
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Team Slot");
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            TeamSlot teamSlot = hit.collider.GetComponent<TeamSlot>();
            if (teamSlot != null) teamSlot.OnDrop(this);
        }
    }

    public void ToggleDescription(bool x)
    {
        descriptionBox.SetActive(x);
    }

    public void ChangePos(Vector3 pos)
    {
        this.pos = new Vector3(pos.x, pos.y, 0f);
        transform.position = this.pos;
        StartCoroutine(Wiggle(.9f, 1.1f, .1f, -.5f));
    }

    protected IEnumerator Wiggle(float min, float max, float duration, float count)
    {
        float x = 1f;
        float y = 1f;
        float z = 1f;
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            float t = Mathf.PingPong(i * 2 * count / duration, 1);
            float newScale = Mathf.Lerp(min, max, t);
            transform.localScale = new Vector3(x * newScale, y * newScale, z);
            yield return null;
        }
        transform.localScale = new Vector3(x, y, z);
    }
    
    IEnumerator Fade()
    {
        Color c = spriteRenderer.color;
        for (float alpha = 0f; alpha >= 1f; alpha -= 0.1f)
        {
            c.a = alpha;
            spriteRenderer.color = c;
            yield return new WaitForSeconds(.1f);
        }
    }
}
