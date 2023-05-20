using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using XEditor.Editor;

public class CheatEngine : MonoBehaviour
{
    public bool isInvicible;
    public TMP_InputField TimeScaleField;
    public ScriptableEvent[] events;
    public ScriptableEventType<float>[] eventsFloat;
    public ScriptableEventType<bool>[] eventsBool;
    public GameObject UI;
    public InputButtonScriptableObject _openCheatinput;
    public GameObject EventsPanel;
    public GameObject EventsFloatPanelPrefab;
    public GameObject EventsBoolPanelPrefab;
    public GameObject EventsPanelPrefab;
    public List<GameObject> EventsPanelPrefabs = new List<GameObject>();


    private void OnValidate()
    {
        events = XEditorUtility.GetAssets<ScriptableEvent>();
        //eventsFloat = XEditorUtility.GetAssets<ScriptableEventType<float>>();
        eventsBool = XEditorUtility.GetAssets<ScriptableEventType<bool>>();
        UpdateEvents();
    }
    private void OnEnable()
    {
        _openCheatinput.OnValueChanged += OpenMenu;
    }
    private void OnDisable()
    {
        _openCheatinput.OnValueChanged -= OpenMenu;
    }
    private void Awake()
    {
        UI.SetActive(false);
    }
    void OpenMenu(bool value)
    {
        if(value)
        {
            UI.SetActive(!UI.activeSelf);
        }
    }
    private void UpdateEvents()
    {
        foreach (Transform child in EventsPanel.transform)
        {
            StartCoroutine(Destroy(child.gameObject));
        }

        IEnumerator Destroy(GameObject go)
        {
            yield return null;
            DestroyImmediate(go);
        }

        EventsPanelPrefabs.Clear();
        //create PanelEvents
        for (int i = 0; i < events.Length; i++)
        {
            GameObject prfb = Instantiate(EventsPanelPrefab, EventsPanel.transform);
            EventsPanelPrefabs.Add( prfb);
            prfb.GetComponentInChildren<Button>().onClick.AddListener(events[i].LaunchEvent);
            prfb.GetComponentInChildren<TextMeshProUGUI>().text = events[i].name;
        }
        /*//create PanelEventsfloat
        for (int i = 0; i < eventsFloat.Length; i++)
        {
            GameObject prfb = Instantiate(EventsFloatPanelPrefab, EventsPanel.transform);
            EventsPanelPrefabs.Add(prfb);
            InputField textField = prfb.GetComponentInChildren<InputField>();

            prfb.GetComponentInChildren<Button>().onClick.AddListener(() => eventsFloat[i].LaunchEvent(float.Parse(textField.text)));
            prfb.GetComponentInChildren<Button>().GetComponentInChildren<TextMeshProUGUI>().text = eventsFloat[i].name;
        }*/
        //create PanelEventsbool
        for (int i = 0; i < eventsBool.Length; i++)
        {
            GameObject prfb = Instantiate(EventsBoolPanelPrefab, EventsPanel.transform);
            EventsPanelPrefabs.Add(prfb);
            Toggle toggle = prfb.GetComponentInChildren<Toggle>();

            prfb.GetComponentInChildren<Button>().onClick.AddListener(() => eventsBool[i].LaunchEvent(toggle.isOn));
            prfb.GetComponentInChildren<TextMeshProUGUI>().text = eventsBool[i].name;
        }
    }

    public void TimeScale(string value)
    {
        Time.timeScale = float.Parse(TimeScaleField.text);
    }
    public void AddHealth()
    {
        FindObjectOfType<PlayerHealth>().AddHealth(1);
    }
    public void Invincible(bool value)
    {
        FindObjectOfType<PlayerHealth>().IsInvincible = value;
    }
}
