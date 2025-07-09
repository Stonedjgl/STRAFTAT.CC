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
    public class TestEntity
    {
        private readonly Cache _cache;
        private List<FakeEntity> _testEntities = new List<FakeEntity>();
        private int _selectedEntityIndex = -1;
        private bool _shouldCreateEntity = false;

        private class FakeEntity
        {
            public Vector3 Position { get; set; }
            public float Health { get; set; }
            public Vector3 HeadPosition { get; set; }
            public bool IsValid { get; set; }

            public FakeEntity(Vector3 position)
            {
                Position = position;
                HeadPosition = position + new Vector3(0, 1.7f, 0);
                Health = 60f;
                IsValid = true;
            }
        }

        public TestEntity(Cache cache)
        {
            _cache = cache;
        }

        public void Update()
        {
            if (!Config.Instance.TestEntityEnabled)
            {
                ClearAllEntities();
                return;
            }

            if (Input.GetKeyDown(Config.Instance.boundKey_createTestEntity))
            {
                CreateTestEntity();
            }
        }

        public void CreateEntityButtonPressed()
        {
            _shouldCreateEntity = true;
        }

        public void CreateTestEntity()
        {
            if (_cache.MainCamera == null || _cache.LocalPlayer == null || _cache.LocalPlayer.GameObject == null) return;

            Vector3 spawnPos = _cache.LocalPlayer.GameObject.transform.position;
            spawnPos.y = -10f;  // Set to ground level

            FakeEntity entity = new FakeEntity(spawnPos);
            _testEntities.Add(entity);
            Config.Instance.AddDebugLog($"Created test entity {_testEntities.Count}");
        }

        private void ClearAllEntities()
        {
            _testEntities.Clear();
            _selectedEntityIndex = -1;
        }

        private Texture2D MakeTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color;

            Texture2D texture = new Texture2D(width, height);
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        public void OnGUI()
        {
            if (!Config.Instance.TestEntityEnabled) return;

            // Draw entity list and stats
            float windowWidth = 200;
            float windowHeight = 300;
            float windowX = Screen.width - windowWidth - 10;
            float windowY = 10;

            GUI.Box(new Rect(windowX, windowY, windowWidth, windowHeight), "Test Entities");

            float contentY = windowY + 25;
            float buttonHeight = 25;
            float spacing = 5;

            // Entity list
            for (int i = 0; i < _testEntities.Count; i++)
            {
                var entity = _testEntities[i];
                if (!entity.IsValid) continue;

                bool isSelected = i == _selectedEntityIndex;
                if (GUI.Button(new Rect(windowX + 10, contentY, windowWidth - 20, buttonHeight), 
                    $"Entity {i + 1}" + (isSelected ? " [Selected]" : "")))
                {
                    _selectedEntityIndex = i;
                }
                contentY += buttonHeight + spacing;
            }

            // Selected entity stats
            if (_selectedEntityIndex >= 0 && _selectedEntityIndex < _testEntities.Count)
            {
                var selectedEntity = _testEntities[_selectedEntityIndex];
                if (selectedEntity.IsValid)
                {
                    contentY += spacing;
                    GUI.Label(new Rect(windowX + 10, contentY, windowWidth - 20, buttonHeight), "Selected Entity Stats:");
                    contentY += buttonHeight;

                    GUI.Label(new Rect(windowX + 10, contentY, windowWidth - 20, buttonHeight), $"Health: {selectedEntity.Health:F1}");
                    contentY += buttonHeight;

                    float distance = Vector3.Distance(_cache.MainCamera.transform.position, selectedEntity.Position);
                    GUI.Label(new Rect(windowX + 10, contentY, windowWidth - 20, buttonHeight), $"Distance: {distance:F1}m");
                    contentY += buttonHeight;

                    if (GUI.Button(new Rect(windowX + 10, contentY, windowWidth - 20, buttonHeight), "Destroy Selected"))
                    {
                        selectedEntity.IsValid = false;
                        _testEntities.RemoveAt(_selectedEntityIndex);
                        _selectedEntityIndex = -1;
                        Config.Instance.AddDebugLog("Destroyed selected test entity");
                    }
                }
            }

            // Draw ESP for all entities
            foreach (var entity in _testEntities)
            {
                if (!entity.IsValid) continue;

                Vector3 screenPos = _cache.MainCamera.WorldToScreenPoint(entity.Position);
                if (screenPos.z > 0)
                {
                    float distance = Vector3.Distance(_cache.MainCamera.transform.position, entity.Position);
                    string text = "";
                    
                    if (Config.Instance.enableNameESP)
                        text += "Test Entity\n";
                    
                    if (Config.Instance.enableDistanceESP)
                        text += $"Distance: {distance:F1}m\n";
                    
                    if (Config.Instance.enableHealthESP)
                    {
                        if (Config.Instance.showHealthNumber)
                            text += $"Health: {entity.Health:F0}\n";
                    }

                    if (text != "")
                    {
                        GUI.Label(new Rect(screenPos.x - 50, Screen.height - screenPos.y - 30, 100, 60), text);
                    }

                    // Draw box ESP
                    if (Config.Instance.enable3DBoxESP)
                    {
                        Bounds bounds = new Bounds(entity.Position, new Vector3(0.5f, 1.7f, 0.5f));
                        Utils.SetupExtentsBounds(bounds);
                        Utils.Draw3DBox(_cache.MainCamera, Color.red);
                    }
                    else if (Config.Instance.enable2DBoxESP)
                    {
                        Vector3 headScreenPos = _cache.MainCamera.WorldToScreenPoint(entity.HeadPosition);
                        if (headScreenPos.z > 0)
                        {
                            float height = screenPos.y - headScreenPos.y;
                            float width = height * 0.5f;
                            Drawing.DrawBox(new Rect(headScreenPos.x - width/2, Screen.height - headScreenPos.y - height, width, height), 2f, Color.red);
                        }
                    }

                    // Draw health bar if enabled
                    if (Config.Instance.enableHealthESP && Config.Instance.showHealthBar)
                    {
                        float healthPercent = entity.Health / 100f;
                        float barWidth = 50f;
                        float barHeight = 5f;
                        float barX = screenPos.x - barWidth/2;
                        float barY = Screen.height - screenPos.y - 40;

                        // Background
                        GUI.DrawTexture(new Rect(barX, barY, barWidth, barHeight), MakeTexture(1, 1, Color.black));
                        // Health
                        GUI.DrawTexture(new Rect(barX, barY, barWidth * healthPercent, barHeight), 
                            MakeTexture(1, 1, Utils.DoubleColorLerp(healthPercent, Color.green, Color.yellow, Color.red)));
                    }
                }
            }
        }

        public PlayerCache GetClosestTestEntity()
        {
            if (_testEntities.Count == 0) return null;

            float closestDistance = float.MaxValue;
            FakeEntity closestEntity = null;

            foreach (var entity in _testEntities)
            {
                if (!entity.IsValid) continue;

                float distance = Vector3.Distance(_cache.LocalPlayer.GameObject.transform.position, entity.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEntity = entity;
                }
            }

            if (closestEntity == null) return null;

            // Create a temporary GameObject for the test entity
            GameObject tempObj = new GameObject("TestEntity");
            tempObj.transform.position = closestEntity.Position;
            tempObj.tag = "Player";  // Add player tag for targeting
            
            // Add required components
            var health = tempObj.AddComponent<PlayerHealth>();
            health.health = closestEntity.Health;
            
            // Create head transform
            GameObject headObj = new GameObject("Head_Col");
            headObj.transform.parent = tempObj.transform;
            headObj.transform.position = closestEntity.HeadPosition;

            // Create and return PlayerCache
            return new PlayerCache(tempObj);
        }
    }
} 