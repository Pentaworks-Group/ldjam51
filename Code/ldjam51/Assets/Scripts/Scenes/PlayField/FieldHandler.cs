using System;

using Assets.Scripts.Behaviours.Models;
using Assets.Scripts.Behaviours.Monsters;
using Assets.Scripts.Game;

using UnityEngine;

namespace Assets.Scripts.Scenes.PlayField
{
    public class FieldHandler : UnityEngine.MonoBehaviour
    {
        private GameObject shadowPlane;
        private GameObject tilesContainer;
        private PlayFieldBehaviour PlayField;
        private System.Collections.Generic.List<MonsterBehaviour> monsterBehaviours = new System.Collections.Generic.List<MonsterBehaviour>();

        public PlayerBehaviour playerBehaviour;
        public FieldState FieldState;

        void Awake()
        {
            this.tilesContainer = this.gameObject.transform.Find("TilesContainer").gameObject;
            this.shadowPlane = this.gameObject.transform.Find("Plane").gameObject;
        }

        public void LoadNewField(PlayFieldBehaviour playField, FieldState fieldState)
        {
            this.PlayField = playField;
            this.FieldState = fieldState;

            this.ClearField();
            this.LoadField();
        }

        public void SetActive(Boolean isActive)
        {
            this.FieldState.IsActive = isActive;
            this.FieldState.Player.IsActive = isActive;
            this.shadowPlane.SetActive(isActive);

            if (this.FieldState.Monsters?.Count > 0)
            {
                for (int i = 0; i < this.FieldState.Monsters.Count; i++)
                {
                    this.FieldState.Monsters[i].IsActive = !isActive;
                }
            }
        }

        private void ClearField()
        {
            if (this.tilesContainer != default)
            {
                foreach (Transform child in tilesContainer.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            if (this.playerBehaviour != null)
            {
                Destroy(this.playerBehaviour.gameObject);
            }

            if (this.monsterBehaviours.Count > 0)
            {
                for (int i = 0; i < this.monsterBehaviours.Count; i++)
                {
                    Destroy(this.monsterBehaviours[i].gameObject);
                }

                this.monsterBehaviours.Clear();
            }
        }

        private void LoadField()
        {
            var fenceTemplate = PlayField.GetTemplateByName<ExtraModelBehaviour>("Fence");

            var maxColumnIndex = this.FieldState.ColumnCount - 1;
            var maxRowIndex = this.FieldState.RowCount - 1;

            for (int z = 0; z < this.FieldState.RowCount; z++)
            {
                for (int x = 0; x < this.FieldState.ColumnCount; x++)
                {
                    var tile = FieldState.Tiles[x, z];
                    tile.FieldState = FieldState;

                    var template = PlayField.GetTemplateByName<TileModelBehaviour>(tile.TemplateReference);

                    var tileObject = Instantiate(template, tilesContainer.transform);

                    tileObject.Tile = tile;

                    var xOffset = x * 2;
                    var zOffset = z * 2;

                    tileObject.transform.Translate(xOffset, 0, zOffset, Space.World);

                    tileObject.gameObject.SetActive(true);

                    if (tile.Material != default)
                    {
                        var meshRenderer = tileObject.GetComponent<MeshRenderer>();

                        if (meshRenderer != default)
                        {
                            meshRenderer.material = tile.Material;
                        }
                    }

                    if (tile.ExtraTemplate != default)
                    {
                        var extraTemplate = PlayField.GetTemplateByName<ExtraModelBehaviour>(tile.ExtraTemplate.TemplateReference);

                        if (extraTemplate != default)
                        {
                            var extraObject = Instantiate(extraTemplate, tileObject.transform);

                            extraObject.Tile = tile;

                            if (extraTemplate.IsRotatable)
                            {
                                var randomRotation = UnityEngine.Random.value * 360;

                                if (randomRotation > 0)
                                {
                                    extraObject.transform.Rotate(0, randomRotation, 0, UnityEngine.Space.World);
                                }
                            }

                            extraObject.gameObject.SetActive(true);
                        }
                    }

                    if (x == 0)
                    {
                        AddFence(tileObject, fenceTemplate, 90);
                    }
                    else if (x == maxColumnIndex)
                    {
                        AddFence(tileObject, fenceTemplate, -90);
                    }

                    if (z == 0)
                    {
                        AddFence(tileObject, fenceTemplate, 0);
                    }
                    else if (z == maxRowIndex)
                    {
                        AddFence(tileObject, fenceTemplate, 180);
                    }
                }
            }

            AddPlayer();

            if (FieldState.Monsters?.Count > 0)
            {
                AddMonsters();
            }

            var totalColumns = this.FieldState.ColumnCount + 2;
            var totalRows = this.FieldState.RowCount + 2;

            var xPosition = this.transform.position.x + this.FieldState.ColumnCount;
            var zPosition = this.transform.position.z + this.FieldState.RowCount;

            shadowPlane.transform.position = new Vector3(xPosition, shadowPlane.transform.position.y, zPosition);
            shadowPlane.transform.localScale = new Vector3(totalColumns, totalRows, shadowPlane.transform.localScale.z);

            if (FieldState.IsActive)
            {
                if (FieldState.IsPlaneVisible)
                {
                    FieldState.IsPlaneVisible = false;
                }

                SetMonsterActive(!FieldState.IsActive);

                shadowPlane.SetActive(FieldState.IsActive);
            }
            else if (FieldState.IsPlaneVisible)
            {
                SetMonsterActive(false);

                shadowPlane.SetActive(true);
            }
            else
            {
                SetMonsterActive(true);
            }
        }

        private void SetMonsterActive(Boolean isActive)
        {
            if (this.FieldState.Monsters?.Count > 0)
            {
                foreach (var monster in this.FieldState.Monsters)
                {
                    monster.IsActive = isActive;
                }
            }
        }

        private void AddPlayer()
        {
            var player = FieldState.Player;

            var playerTemplate = PlayField.GetTemplateByName<PlayerBehaviour>(player.TemplateReference);

            if (playerTemplate != default)
            {
                this.playerBehaviour = Instantiate(playerTemplate, this.gameObject.transform);

                var newPosition = new UnityEngine.Vector3(player.PositionX * 2, 0, player.PositionZ * 2);

                if (player.Material != default)
                {
                    var meshRenderer = playerBehaviour.GetComponent<MeshRenderer>();

                    if (meshRenderer != default)
                    {
                        meshRenderer.material = player.Material;
                    }
                }

                this.playerBehaviour.FieldHandler = this;

                this.playerBehaviour.transform.Translate(newPosition, UnityEngine.Space.World);
                this.playerBehaviour.gameObject.SetActive(true);
            }
        }

        private void AddMonsters()
        {
            foreach (var monster in FieldState.Monsters)
            {
                var monsterTemplate = PlayField.GetTemplateByName<MonsterBehaviour>(monster.TemplateReference);

                if (monsterTemplate != default)
                {
                    var monsterBehaviour = Instantiate(monsterTemplate, this.gameObject.transform);

                    var newPosition = new UnityEngine.Vector3(monster.PositionX * 2, 0, monster.PositionZ * 2);

                    if (monster.Material != default)
                    {
                        var meshRenderer = monsterBehaviour.GetComponent<MeshRenderer>();

                        if (meshRenderer != default)
                        {
                            meshRenderer.material = monster.Material;
                        }
                    }

                    monsterBehaviour.FieldHandler = this;
                    monsterBehaviour.Monster = monster;

                    monsterBehaviour.transform.Translate(newPosition, UnityEngine.Space.World);
                    monsterBehaviour.gameObject.SetActive(true);

                    this.monsterBehaviours.Add(monsterBehaviour);
                }
            }
        }

        private void AddFence(TileModelBehaviour tileObject, ExtraModelBehaviour fenceTemplate, Int32 rotationAngle)
        {
            var fenceObject = Instantiate(fenceTemplate, tileObject.transform);
            fenceObject.Tile = tileObject.Tile;

            if (rotationAngle != 0)
            {
                fenceObject.transform.Rotate(new Vector3(0, 0, rotationAngle));
            }

            fenceObject.gameObject.SetActive(true);
        }
    }
}
