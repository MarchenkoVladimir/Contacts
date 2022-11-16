using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Card : MonoBehaviour
{
    [SerializeField] private TMP_Text id;
    [SerializeField] private TMP_Text first_name;
    [SerializeField] private TMP_Text last_name;
    [SerializeField] private TMP_Text email;
    [SerializeField] private TMP_Text gender;
    [SerializeField] private TMP_Text ip_address;
    [SerializeField] private TMP_Text address;

    public TMP_Text ID => id;
    public TMP_Text First_name => first_name;
    public TMP_Text Last_name => last_name;
    public TMP_Text Email => email;
    public TMP_Text Gender => gender;
    public TMP_Text IP_address => ip_address;
    public TMP_Text Address => address;

    public int IDInt => Convert.ToInt32(ID.text);

    public RectTransform RectTr => GetComponent<RectTransform>();
    private Rect rect;
    private bool iVisible;
    public void Setup(Human human)
    {
        id.text = human.ID;
        first_name.text = human.First_name;
        last_name.text = human.Last_name;
        email.text = human.Email;
        gender.text = human.Gender;
        ip_address.text = human.Ip_address;
        address.text = human.Address;
        rect = new Rect(0, 0, Screen.width, Screen.height);
        iVisible = false;
    }

    private void Update()
    {
        CheckRectPosition();
    }

    public void CheckRectPosition()
    {
         bool isVisible = rect.Contains(RectTr.position);
        if (!isVisible && iVisible)
        {
            
            GameObjectPool objectPool = GameObjectPool.Instance;
            if (RectTr.position.y + RectTr.sizeDelta.y  > Screen.height * 2)
            {
                int index = objectPool.MaxIndex + 1;
                objectPool.Unspawn(this.gameObject);
                transform.SetSiblingIndex(transform.parent.childCount - 1);
                objectPool.Spawn(JsonManager.Instance.Humans[index], index);
                objectPool.UpdateIndexses(objectPool.MinIndex + 1, index);
                iVisible = false;
            }
            else if (RectTr.position.y  < RectTr.sizeDelta.y * 4)
            {
                int index = Mathf.Clamp(objectPool.MinIndex, 0, JsonManager.Instance.Humans.Length);

                transform.SetSiblingIndex(0);
                iVisible = false;
                objectPool.Unspawn(objectPool.GetCard().gameObject);
                objectPool.Spawn(JsonManager.Instance.Humans[index], index);
                objectPool.UpdateIndexses(index - 1, objectPool.MaxIndex - 1);
            }
            
        }
        else if(!iVisible && isVisible)
        {
            iVisible = true;
        }
        
    }
}
