using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Object;
using FishNet.Serializing;
using FishNet;
using UnityEngine.SocialPlatforms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using static Mono.Security.X509.X520;
using DG.Tweening;
using STRAFTAT_CC;

namespace STRAFTAT_CC.Features
{
    public class LegitBot
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;
        private const float HEAD_OFFSET = 1.5f;

        private readonly Cache _cache;
        private bool _isEnabled = false;
        private bool _isMouseHold = false;
        private float _smoothing = 1.0f;
        private float _fov = 5.0f;

        public bool IsEnabled => _isEnabled;

        public LegitBot(Cache cache)
        {
            _cache = cache;
        }

        public void Update()
        {
            if (Input.GetKey(Config.Instance.boundKey_legitbot))
            {
                if (!Config.Instance.LegitAimbot)
                {
                    Config.Instance.LegitAimbot = true;
                    Config.Instance.AddDebugLog("Legit Aimbot: Activated");
                }
                AimAtClosestPlayer();
            }
            else
            {
                if (Config.Instance.LegitAimbot)
                {
                    Config.Instance.LegitAimbot = false;
                    Config.Instance.AddDebugLog("Legit Aimbot: Deactivated");
                    MouseRelease();
                }
            }

            if (!Config.Instance.LegitAimbot || !_cache.LocalPlayer.IsValid || !_cache.MainCamera)
            {
                return;
            }
            AimAtClosestPlayer();
        }

        public void OnGUI()
        {
            if (Config.Instance.drawLegitFOV && Config.Instance.LegitAimbot)
            {
                DrawFOV();
            }
        }

        private void DrawFOV()
        {
            if (!_cache.MainCamera) return;

            float fov = Config.Instance.legitFOV;
            float distance = 100f;
            float radius = Mathf.Tan(fov * Mathf.Deg2Rad) * distance;

            Vector3 center = _cache.MainCamera.transform.position + _cache.MainCamera.transform.forward * distance;
            Vector3 right = _cache.MainCamera.transform.right * radius;
            Vector3 up = _cache.MainCamera.transform.up * radius;

            Vector3[] points = new Vector3[4];
            points[0] = center + right + up;
            points[1] = center - right + up;
            points[2] = center - right - up;
            points[3] = center + right - up;

            GL.PushMatrix();
            GL.LoadProjectionMatrix(_cache.MainCamera.projectionMatrix);
            GL.modelview = _cache.MainCamera.worldToCameraMatrix;

            GL.Begin(GL.LINES);
            GL.Color(new Color(1f, 0f, 0f, 0.5f));

            for (int i = 0; i < 4; i++)
            {
                int next = (i + 1) % 4;
                GL.Vertex(points[i]);
                GL.Vertex(points[next]);
            }

            GL.End();
            GL.PopMatrix();
        }

        public static void MouseHold()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
        }

        public static void MouseRelease()
        {
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private void AimAtClosestPlayer()
        {
            PlayerCache closestPlayer = GetClosestTarget();
            if (closestPlayer == null || closestPlayer.PlayerHealth.health <= 0) return;

            Vector3 targetPosition;
            if (closestPlayer.HeadTransform != null)
            {
                targetPosition = closestPlayer.HeadTransform.position;
            }
            else
            {
                targetPosition = closestPlayer.GameObject.transform.position + Vector3.up * HEAD_OFFSET;
            }

            Vector3 direction = (targetPosition - _cache.MainCamera.transform.position).normalized;
            float angle = Vector3.Angle(_cache.MainCamera.transform.forward, direction);

            if (angle > Config.Instance.legitFOV)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _cache.MainCamera.transform.rotation = Quaternion.Slerp(_cache.MainCamera.transform.rotation, targetRotation, Time.deltaTime * Config.Instance.legitSmoothing);

            if (Config.Instance.LegitAutoShoot)
            {
                RaycastHit hit;
                if (Physics.Raycast(_cache.MainCamera.transform.position, direction, out hit))
                {
                    Transform rootTransform = hit.transform;
                    while (rootTransform.parent != null)
                    {
                        rootTransform = rootTransform.parent;
                    }

                    PlayerHealth hitPlayerHealth = hit.transform.GetComponentInParent<PlayerHealth>();
                    if (hitPlayerHealth != null)
                    {
                        if (hitPlayerHealth.gameObject == closestPlayer.GameObject && closestPlayer.PlayerHealth.health > 0)
                        {
                            Config.Instance.AddDebugLog("LegitBot: Hit correct player");
                            MouseHold();
                            _isMouseHold = true;
                        }
                        else
                        {
                            MouseRelease();
                            _isMouseHold = false;
                        }
                    }
                }
            }
            else
            {
                MouseRelease();
                _isMouseHold = false;
            }
        }

        public PlayerCache GetClosestTarget()
        {
            float closestDistance = float.MaxValue;
            PlayerCache closestPlayer = null;

            foreach (PlayerCache player in _cache.Players)
            {
                if (!player.IsValid)
                    continue;

                float distance = Vector3.Distance(_cache.LocalPlayer.GameObject.transform.position, player.GameObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }

            return closestPlayer;
        }
    }
} 