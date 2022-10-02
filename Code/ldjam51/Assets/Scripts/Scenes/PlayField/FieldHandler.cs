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
        private PlayerBehaviour playerBehaviour;

        public FieldState FieldState;
        public PlayFieldBehaviour PlayField;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if ((!isLoaded) && (this.FieldState != default))
            {
                isLoaded = true;

                this.plane = this.gameObject.transform.Find("Plane").gameObject;
                this.LoadField();
            }
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

            for (int z = 0; z < this.FieldState.RowCount; z++)
            {
                for (int x = 0; x < this.FieldState.ColumnCount; x++)
                {
                    var tile = FieldState.Tiles[x, z];

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
                }
            }

            var playerTemplate = PlayField.GetTemplateByName<PlayerBehaviour>(FieldState.Player.TemplateReference);

            if (playerTemplate != default)
            {
                this.playerBehaviour = Instantiate(playerTemplate, this.gameObject.transform);

                var newPosition = new UnityEngine.Vector3(FieldState.Player.PositionX * 2, 0, FieldState.Player.PositionZ * 2);

                this.playerBehaviour.FieldHandler = this;

                this.playerBehaviour.transform.Translate(newPosition, UnityEngine.Space.World);
                this.playerBehaviour.gameObject.SetActive(true);
            }

            plane.SetActive(FieldState.IsPlaneVisible);
            plane.transform.Translate(new Vector3(this.FieldState.ColumnCount -1 , 0, this.FieldState.RowCount - 1), Space.World);

            //var scale = this.FieldState.RowCount * 0.7f;

            //plane.transform.localScale = new Vector3(scale, scale, scale);

            PlayField.AdjustCamera(0, 0);
        }
    }
}
