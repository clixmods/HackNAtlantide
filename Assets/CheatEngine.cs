using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using XEditor.Editor;
#endif
public class CheatEngine : MonoBehaviour
{
    public bool isInvicible;
    public Slider sliderTimeScale;
    public TextMeshProUGUI textTimeScaleValue;
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

    public LayerMask LayerToIgnoreCollisionWallEnnemiePlayer;
    LayerMask layerBeforeIgnoring;
    public Toggle toggleIgnoreCollision;
    public Toggle toggleFly;
#if UNITY_EDITOR
    private void OnValidate()
    {
        events = XEditorUtility.GetAssets<ScriptableEvent>();
        //eventsFloat = XEditorUtility.GetAssets<ScriptableEventType<float>>();
        eventsBool = XEditorUtility.GetAssets<ScriptableEventType<bool>>();
        UpdateEvents();
    }
    #endif
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
        Debug.Log("opencheat");
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

    public void TimeScale()
    {
        Time.timeScale = sliderTimeScale.value;
        textTimeScaleValue.text = Time.timeScale.ToString();
    }
    public void AddHealth()
    {
        FindObjectOfType<PlayerHealth>().AddHealth(1);
    }
    public void Invincible(bool value)
    {
        FindObjectOfType<PlayerHealth>().IsInvincible = value;
    }
    public void ChangeCollision()
    {
        if (toggleIgnoreCollision.isOn)
        {
            Debug.Log("cancelcollision");
            Physics.IgnoreLayerCollision(6, 11);
            Physics.IgnoreLayerCollision(6, 3);
            layerBeforeIgnoring = FindObjectOfType<PlayerMovement>().LayerToIgnore ;
            FindObjectOfType<PlayerMovement>().LayerToIgnore = LayerToIgnoreCollisionWallEnnemiePlayer;
        }
        else
        {
            Debug.Log("activecollision");
            Physics.IgnoreLayerCollision(6, 11,false);
            Physics.IgnoreLayerCollision(6, 3,false);
            FindObjectOfType<PlayerMovement>().LayerToIgnore = layerBeforeIgnoring;
        }
    }
    public void Fly()
    {
        if (toggleFly.isOn)
        {
            FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody>().useGravity = false;
            FindObjectOfType<PlayerMovement>().fly = true;
        }
        else
        {
            FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody>().useGravity = true;
            FindObjectOfType<PlayerMovement>().fly = false;
        }
    }
}
