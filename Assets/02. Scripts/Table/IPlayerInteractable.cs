using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어와 상호작용 할 수 있는 객체 인터페이스
/// </summary>
public interface IPlayerInteractable
{
    public void EnterPlayer(PlayerItemController playerItemController);
    public void ExitPlayer();
}
