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

    private readonly Stack<ACMenu> _menuStack = new Stack<ACMenu>();
    private readonly Stack<Vector3> _offsets = new Stack<Vector3>();

    private Canvas _canvas;
    private float Width => _canvas.pixelRect.width;
    private float Height => _canvas.pixelRect.height;

    [SerializeField] private bool onStartOpenMainMenu = true;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        if (onStartOpenMainMenu)
            OpenMenu(mainMenu, Vector3.zero, 0.0f);
    }

    public void OpenMenu(GameObject menuObject, Vector3 slideFrom = new Vector3(), float duration = 0.2f)
    {
        GameObject instance = Instantiate(menuObject);

        // If it isn't a Menu

        ACMenu menu = instance.GetComponent<ACMenu>();

        if (!menu){
            Debug.LogError("Instance didn't have an ACMenu.");
            return;
        }

        menu.enabled = false;

        RectTransform rt = instance.GetComponent<RectTransform>();
        if (!rt){
            Debug.LogError("Instance didn't have an RectTransform.");
            return;
        }

        // Someone is null here
        rt.sizeDelta = ((RectTransform) _canvas.transform).sizeDelta;

        menu.transform.SetParent(rootMenu);

        Vector3 offset = new Vector3(slideFrom.x * Width, slideFrom.y * Height, slideFrom.z);
        menu.Open();


        if (_menuStack.Count == 0){
            menu.transform.localPosition = Vector3.zero;
            _offsets.Push(Vector3.zero);
            menu.enabled = true;
        }
        else{
            ACMenu currentHead = _menuStack.Peek();
            currentHead.enabled = false;

            menu.transform.localPosition = currentHead.transform.localPosition + offset;
            _offsets.Push(offset);

            Vector3 targetPosition = rootMenu.localPosition - offset;
            DOTween.To(() => rootMenu.localPosition, x => rootMenu.localPosition = x, targetPosition, duration)
                .SetEase(Ease.InOutCubic).SetUpdate(true).onComplete += () => { menu.enabled = true; };
        }

        _menuStack.Push(menu);
    }

    public void CloseMenu(float duration = 0.2f)
    {
        if (_menuStack.Count == 0){
            return;
        }

        ACMenu currentMenu = _menuStack.Pop();
        Vector3 offset = _offsets.Pop();

        if (_menuStack.Count == 0){
            currentMenu.enabled = false;
            currentMenu.Close();
            Destroy(currentMenu.gameObject);
        }
        else{
            Vector3 targetPosition = rootMenu.localPosition + offset;
            DOTween.To(() => rootMenu.localPosition, x => rootMenu.localPosition = x, targetPosition, duration)
                .SetEase(Ease.InOutCubic).SetUpdate(true).onComplete = () =>
            {
                currentMenu.enabled = false;
                currentMenu.Close();
                Destroy(currentMenu.gameObject);
                
                ACMenu newMenu = _menuStack.Peek();
                newMenu.enabled = true;
            };
        }
    }

    public void LoadMainMenu()
    {
        CloseAllMenus();
        OpenMenu(mainMenu, Vector3.zero, 0.0f);
    }
    
    public void CloseAllMenus()
    {
        while (_menuStack.Count > 0){
            CloseMenu(0.0f);
        }
    }
}