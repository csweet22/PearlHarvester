using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : PersistentSingleton<MainCanvas>
{
    [SerializeField] private Transform rootMenu;
    [SerializeField] private GameObject mainMenu;

    private Stack<ACMenu> menuStack = new Stack<ACMenu>();
    private Stack<Vector3> offsets = new Stack<Vector3>();

    private Canvas _canvas;
    private float _width => _canvas.pixelRect.width;
    private float _height => _canvas.pixelRect.height;

    [SerializeField] private Vector3 slideFrom;

    [ContextMenu("Show Main Menu")]
    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        OpenMenu(mainMenu, slideFrom, 1.0f);
    }

    public void OpenMenu(GameObject menuObject, Vector3 slideFrom = new Vector3(), float duration = 0.2f)
    {
        GameObject instance = Instantiate(menuObject);

        // If it isn't a Menu

        ACMenu menu = instance.GetComponent<ACMenu>();

        if (!menu){
            return;
        }

        menu.transform.SetParent(rootMenu);

        Vector3 offset = new Vector3(slideFrom.x * _width, slideFrom.y * _height, slideFrom.z);
        menu.Open();

        // Disable current head
        if (menuStack.Count > 0){
            ACMenu currentHead = menuStack.Peek();
            currentHead.enabled = false;
            menu.transform.localPosition = currentHead.transform.localPosition + offset;
        }
        else{
            menu.transform.localPosition = Vector3.zero;
            offsets.Push(Vector3.zero);
        }

        if (menuStack.Count > 0){
            offsets.Push(offset);
            Vector3 targetPosition = rootMenu.localPosition - offset;
            DOTween.To(() => rootMenu.localPosition, x => rootMenu.localPosition = x, targetPosition, duration)
                .SetEase(Ease.InOutCubic);
        }

        menuStack.Push(menu);
    }

    [ContextMenu("Close Menu")]
    public void CloseMenu()
    {
        if (menuStack.Count == 0){
            return;
        }

        ACMenu currentMenu = menuStack.Pop();
        Vector3 offset = offsets.Pop();

        if (menuStack.Count != 0){
            Vector3 targetPosition = rootMenu.localPosition + offset;
            DOTween.To(() => rootMenu.localPosition, x => rootMenu.localPosition = x, targetPosition, 1.0f)
                .SetEase(Ease.InOutCubic).onComplete = () => { currentMenu.Close(); };
        }
        else{
            currentMenu.Close();
        }


        if (menuStack.Count > 0){
            ACMenu newMenu = menuStack.Peek();
            newMenu.enabled = true;
        }
    }
}