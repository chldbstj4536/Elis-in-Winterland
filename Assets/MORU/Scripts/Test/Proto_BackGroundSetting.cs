using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Proto_BackGroundSetting : MonoBehaviour
{
    [SerializeField] List<AudioClip> clips; //BGM 클립들
    [SerializeField] AudioSource BGM_Player;

    [SerializeField] GameObject BGM_UI;     //BGM 선택 UI
    public bool isActive => BGM_UI.activeInHierarchy;

    [SerializeField] Dropdown dropdownBox;

    [SerializeField] double playTime;
    [SerializeField] double currentPlayTime;

    private void Start()
    {
        dropdownBox.onValueChanged.AddListener(PlayBGM);
        for (int i = 0; i < clips.Count; i++)
        {
            dropdownBox.options.Add(new Dropdown.OptionData(clips[i].name));
        }
        dropdownBox.value = Random.Range(0,dropdownBox.options.Count);
    }

    private void Update()
    {
        OnInputKey();

        currentPlayTime += Time.deltaTime;
        if(currentPlayTime >= playTime)
        {
            dropdownBox.value = Random.Range(0, dropdownBox.options.Count);
        }
    }

    public void OnInputKey()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (isActive && BGM_UI != null)
            {
                BGM_UI.SetActive(false);
            }
            else if (!isActive && BGM_UI != null)
            {
                BGM_UI.SetActive(true);
            }
        }
    }

    public void PlayBGM(int index)
    {
        Debug.Log($"플레이 뮤직 : {index}");
        BGM_Player.clip = clips[index];
        BGM_Player.Play();
        playTime = clips[index].length+3;
        currentPlayTime = 0;
    }

}
