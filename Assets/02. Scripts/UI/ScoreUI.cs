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
        // �Ŵ��� �ν��Ͻ� ��� �� �̺�Ʈ �ݹ� �Ҵ�
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
