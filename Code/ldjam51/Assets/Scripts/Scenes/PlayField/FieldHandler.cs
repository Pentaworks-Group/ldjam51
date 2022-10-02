using System;

using Assets.Scripts.Behaviours.Models;
using Assets.Scripts.Game;

using UnityEngine;

namespace Assets.Scripts.Scenes.PlayField
{
    public class FieldHandler : UnityEngine.MonoBehaviour
    {
        private System.Boolean isLoaded = false;
        private GameObject plane;
        private GameObject tilesContainer;
        public PlayerBehaviour playerBehaviour;

        public FieldState FieldState;
        public PlayFieldBehaviour PlayField;

        void Start()
        {

            this.plane = this.gameObject.transform.Find("Plane").gameObject;
        }



        public void LoadNewField(FieldState fieldState)
        {
            this.FieldState = fieldState;
            this.LoadField();
        }

        public void SetActive(Boolean isActive)
        {
            this.FieldState.IsActive = isActive;
            this.FieldState.Player.IsActive = isActive;
            this.plane.SetActive(isActive);
        }

        private void LoadField()
        {
            this.tilesContainer = this.gameObject.transform.Find("TilesContainer").gameObject;

            var fenceTemplate = PlayField.GetTemplateByName<ExtraModelBehaviour>("Fence");
            //var fenceTemplate = PlayField.GetTemplateByName<ExtraModelBehaviour>("Wall");

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

                var newPosition = new UnityEngine.Vector3(FieldState.Player.PositionX*2, 0, FieldState.Player.PositionZ*2);

                this.playerBehaviour.FieldHandler = this;

                this.playerBehaviour.transform.Translate(newPosition, UnityEngine.Space.World);
                this.playerBehaviour.gameObject.SetActive(true);
            }

            if (plane == default)
            {
                this.plane = this.gameObject.transform.Find("Plane").gameObject;
            }
            plane.SetActive(FieldState.IsPlaneVisible);
            plane.transform.Translate(new Vector3(this.FieldState.ColumnCount - 1, 0, this.FieldState.RowCount - 1), Space.World);

            PlayField.AdjustCamera();
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
