using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIAudioController : MonoBehaviour
{
    [Header("Sonidos")]
    public AudioClip selectSound;
    public AudioClip confirmSound;

    void Start()
    {
        AssignSoundsToButtons();
    }

    public void AssignSoundsToButtons()
    {
        // La version moderna: Incluye objetos inactivos y no garantiza orden para ser mas rapido
        Button[] buttons = Object.FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            // Limpiamos listeners previos para no duplicar sonidos si se llama varias veces
            btn.onClick.RemoveListener(PlayConfirm);
            btn.onClick.AddListener(PlayConfirm);

            // Configuramos el EventTrigger para el Hover/Select
            EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>() ?? btn.gameObject.AddComponent<EventTrigger>();

            // Limpiamos triggers antiguos para evitar sonidos duplicados
            trigger.triggers.Clear();

            // Evento: Al seleccionar (Teclado/Mando o SetSelected)
            AddTrigger(trigger, EventTriggerType.Select, (data) => PlaySelect());

            // Evento: Al pasar el mouse (para que se seleccione automaticamente)
            AddTrigger(trigger, EventTriggerType.PointerEnter, (data) => {
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
            });
        }
    }

    private void AddTrigger(EventTrigger trigger, EventTriggerType type, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    void PlaySelect() => AudioManager.instance.PlaySFX(selectSound);
    void PlayConfirm() => AudioManager.instance.PlaySFX(confirmSound);
}