using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("-Components")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private Coroutine delayRoutine;
    private void OnEnable()
    {
        // 매니저 인스턴싱 대기 후 이벤트 콜백 할당
        delayRoutine = StartCoroutine(Extension.DelayAction(0.1f, ()=>ScoreManager.Instance.OnChangeScore.AddListener(UpdateScore)));
    }
    private void OnDisable()
    {
        ScoreManager.Instance.OnChangeScore.RemoveListener(UpdateScore);
    }
    private void UpdateScore(int value)
    {
        scoreText.text = value.ToString();
    }
}
