using Cinemachine;
using LiquidSnake.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiquidSnake.Character
{
    public class ResetPlayer : MonoBehaviour, IResetteable
    {
        public Transform playerSpawnPoint;
        public Transform cameraSpawnPoint;
        public CinemachineVirtualCamera vcam;


        public void Reset()
        {
            vcam.PreviousStateIsValid = false;
            transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
            vcam.ForceCameraPosition(cameraSpawnPoint.position, cameraSpawnPoint.rotation);
        }
    } // ResetPlayer
} // namespace LiquidSnake.Character
