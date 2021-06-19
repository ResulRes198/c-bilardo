using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeInput : MonoBehaviour
{
    EventSystem system;
    // Start is called before the first frame update
    void Start()
    {
        system = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown() != null)
            {
                system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown().Select();
            }
        }
    }
}
