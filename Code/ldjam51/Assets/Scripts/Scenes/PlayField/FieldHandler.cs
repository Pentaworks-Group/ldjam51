using System;

using Assets.Scripts.Behaviours.Models;
using Assets.Scripts.Game;

using UnityEngine;

namespace Assets.Scripts.Scenes.PlayField
{
    public class FieldHandler : UnityEngine.MonoBehaviour
    {
        private GameObject plane;
        private GameObject tilesContainer;
        private PlayFieldBehaviour PlayField;

        public PlayerBehaviour playerBehaviour;
        public FieldState FieldState;

        void Awake()
        {
            this.tilesContainer = this.gameObject.transform.Find("TilesContainer").gameObject;
            this.plane = this.gameObject.transform.Find("Plane").gameObject;
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
            this.plane.SetActive(isActive);
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

                    var template = PlayField.GetTemplateByName<TileModelBehaviour>(tile.Reference);

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
                        var extraTemplate = PlayField.GetTemplateByName<ExtraModelBehaviour>(tile.ExtraTemplate.Reference);

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

            var playerTemplate = PlayField.GetTemplateByName<PlayerBehaviour>(FieldState.Player.TemplateReference);

            if (playerTemplate != default)
            {
                this.playerBehaviour = Instantiate(playerTemplate, this.gameObject.transform);

                var newPosition = new UnityEngine.Vector3(FieldState.Player.PositionX * 2, 0, FieldState.Player.PositionZ * 2);

                if (FieldState.Player.Material != default)
                {
                    var meshRenderer = playerBehaviour.GetComponent<MeshRenderer>();

                    if (meshRenderer != default)
                    {
                        meshRenderer.material = FieldState.Player.Material;
                    }
                }

                this.playerBehaviour.FieldHandler = this;

                this.playerBehaviour.transform.Translate(newPosition, UnityEngine.Space.World);
                this.playerBehaviour.gameObject.SetActive(true);
            }

            var totalColumns = this.FieldState.ColumnCount + 2;
            var totalRows = this.FieldState.RowCount + 2;

            var xPosition = totalColumns - 2;
            var zPosition = totalRows - 2;

            var xScale = this.FieldState.ColumnCount + 2;
            var zScale = this.FieldState.RowCount + 2;

            plane.transform.position = new Vector3(xPosition, plane.transform.position.y, zPosition);
            plane.transform.localScale = new Vector3(xScale, zScale, plane.transform.localScale.z);

            if (FieldState.IsActive)
            {
                if (FieldState.IsPlaneVisible)
                {
                    FieldState.IsPlaneVisible = false;
                }

                plane.SetActive(FieldState.IsActive);
            }
            else if (FieldState.IsPlaneVisible)
            {
                plane.SetActive(true);
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
