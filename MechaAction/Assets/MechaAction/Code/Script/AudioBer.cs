using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioBer : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    private void Start()
    {
        // 保存済み音量をロードしてスライダーに反映
        //var audio = SaveManager.Instance.AudioLoad();

        //bgmSlider.value = audio.data.BGMVolume;
        //seSlider.value = audio.data.SEVolume;

        // 初期値を AudioManager にも反映
        //AudioManager.Instance.ChangeVolume(
        //    audio.data.BGMVolume,
        //    audio.data.SEVolume
        //);
    }

    // スライダー変更時に呼ぶ
    public void OnValueChanged()
    {
        //AudioManager.Instance.ChangeVolume(
        //    bgmSlider.value,
        //    seSlider.value
        //);
    }

}
