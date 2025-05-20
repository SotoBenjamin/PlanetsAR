using UnityEngine;
using TMPro;
using Vuforia;
using System.Collections.Generic;

public class InfoManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject infoButton;       // tu ShowInfoButton
    public GameObject infoPanel;        // tu InfoPanel
    public TextMeshProUGUI infoText;    // tu PlanetInfoText

    [Header("Planet Descriptions")]
    [TextArea] public string EarthInfo;
    [TextArea] public string MarsInfo;
    [TextArea] public string JupiterInfo;
    [TextArea] public string SunInfo;

    const string MODEL_CONTAINER_NAME = "ModelContainer";
    private HashSet<string> tracked = new HashSet<string>();
    private Dictionary<string, Transform> containers = new Dictionary<string, Transform>();
    private Dictionary<string, Vector3> originalScales = new Dictionary<string, Vector3>();

    void Start()
    {
        infoButton.SetActive(false);
        infoPanel .SetActive(false);

        foreach (var itb in FindObjectsOfType<ImageTargetBehaviour>())
        {
            string name = itb.TargetName.ToLower();
            itb.OnTargetStatusChanged += OnTrackableStateChanged;

            var container = itb.transform.Find(MODEL_CONTAINER_NAME);
            if (container != null)
            {
                containers[name] = container;
                originalScales[name] = container.localScale;
            }
        }
    }

    void OnTrackableStateChanged(ObserverBehaviour ob, TargetStatus status)
    {
        string name = ob.TargetName.ToLower();
        bool isTracked = status.Status == Status.TRACKED;

        if (isTracked) tracked.Add(name);
        else          tracked.Remove(name);

        // UI: solo cuando EXACTAMENTE 1 target
        if (tracked.Count == 1)
        {
            string sole = null;
            foreach (var t in tracked) sole = t;
            ShowButtonFor(sole);
        }
        else
        {
            infoButton.SetActive(false);
            infoPanel .SetActive(false);
        }

        // Escalado: 1 target → escala=1, else escala original
        if (tracked.Count == 1)
        {
            string sole = null;
            foreach (var t in tracked) sole = t;
            foreach (var kv in containers)
                kv.Value.localScale = (kv.Key == sole)
                    ? Vector3.one
                    : originalScales[kv.Key];
        }
        else
        {
            foreach (var kv in containers)
                kv.Value.localScale = originalScales[kv.Key];
        }
    }

    public void OnInfoButtonPressed()
    {
        infoButton.SetActive(false);
        infoPanel .SetActive(true);
    }

    public void OnCloseButtonPressed()
    {
        infoPanel .SetActive(false);
        if (tracked.Count == 1)
            infoButton.SetActive(true);
    }

    void ShowButtonFor(string name)
    {
        infoButton.GetComponentInChildren<TextMeshProUGUI>()
                  .text = $"Acerca de";

        switch (name)
        {
            case "earth":   infoText.text = EarthInfo;   break;
            case "mars":    infoText.text = MarsInfo;    break;
            case "jupiter": infoText.text = JupiterInfo; break;
            case "sun":     infoText.text = SunInfo;     break;
            default:        infoText.text = "";          break;
        }

        infoPanel.SetActive(false);
        infoButton.SetActive(true);
    }
}