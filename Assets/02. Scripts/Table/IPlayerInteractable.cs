using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �÷��̾�� ��ȣ�ۿ� �� �� �ִ� ��ü �������̽�
/// </summary>
public interface IPlayerInteractable
{
    public void EnterPlayer(PlayerItemController playerItemController);
    public void ExitPlayer();
}
