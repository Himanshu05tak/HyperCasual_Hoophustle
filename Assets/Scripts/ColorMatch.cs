using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class ColorMatch : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private HoopManager _hoopManager;
    
    private void OnEnable()
    {
        _hoopManager.OnHoopTargetChanged += HoopManager_OnHoopTargetChanged;
    }

    private void OnDisable()
    {
        _hoopManager.OnHoopTargetChanged -= HoopManager_OnHoopTargetChanged;
    }

    private void HoopManager_OnHoopTargetChanged(Hoop hoop)
    {
        switch (hoop.HoopData.HoopName)
        {
            case "Red":
                ChangeImageColor(Color.red);
                break;
            case "Yellow":
                ChangeImageColor(Color.yellow);
                break;
            case "Blue":
                ChangeImageColor(Color.blue);
                break;
        }
    }

    private void ChangeImageColor(Color color)
    {
        color.a = 0.2f;
        image.color = color;
    }

}
