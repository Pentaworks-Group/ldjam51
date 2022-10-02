using System;

using Assets.Scripts.Behaviours.Models;
using Assets.Scripts.Game;

using UnityEngine;

namespace Assets.Scripts.Scenes.PlayField
{
    public class FieldHandler : UnityEngine.MonoBehaviour
    {
        private System.Boolean isLoaded = false;
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

                this.LoadField();
            }            
        }

        public void SetActive(Boolean isActive)
        {
            this.FieldState.IsActive = isActive;
            this.playerBehaviour.Player.IsActive = isActive;
        }

        private void LoadField()
        {
            this.tilesContainer = this.gameObject.transform.Find("TilesContainer").gameObject;

            for (int z = 0; z < this.FieldState.RowCount; z++)
            {
                for (int x = 0; x < this.FieldState.ColumnCount; x++)
                {
                    var tile = FieldState.Tiles[z, x];

                    var tileObject = Instantiate(PlayField.GetTemplateByName<TileModelBehaviour>(tile.TemplateReference), tilesContainer.transform);

                    var xOffset = x * 2;
                    var zOffset = z * 2;

                    tileObject.gameObject.SetActive(true);

                    tileObject.transform.Translate(xOffset, 0, zOffset, UnityEngine.Space.World);

                    if (tile.Material != default)
                    {
                        var meshRenderer = tileObject.GetComponent<MeshRenderer>();

                        if (meshRenderer != default)
                        {
                            meshRenderer.material = tile.Material;
                        }
                    }

                    if (!System.String.IsNullOrEmpty(tile.ExtraTemplateReference))
                    {
                        var extraTemplate = PlayField.GetTemplateByName<ExtraModelBehaviour>(tile.ExtraTemplateReference);

                        if (extraTemplate != default)
                        {
                            //var extraObject = Instantiate(extraTemplate.gameObject, fieldsContainer.transform);
                            var extraObject = Instantiate(extraTemplate.gameObject, tileObject.transform);

                            if (extraTemplate.IsRotatable)
                            {
                                var randomRotation = UnityEngine.Random.value * 360;

                                if (randomRotation > 0)
                                {
                                    extraObject.transform.Rotate(0, randomRotation, 0, UnityEngine.Space.World);
                                }
                            }

                            //extraObject.transform.Translate(xOffset, 0, zOffset, Space.World);
                            extraObject.SetActive(true);
                        }
                    }
                }
            }

            var playerTemplate = PlayField.GetTemplateByName<PlayerBehaviour>(FieldState.Player.TemplateReference);

            if (playerTemplate != default)
            {
                this.playerBehaviour = Instantiate(playerTemplate, this.gameObject.transform);

                var newPosition = new UnityEngine.Vector3(FieldState.Player.PositionX, 0, FieldState.Player.PositionY);

                this.playerBehaviour.Player = FieldState.Player;
                
                this.playerBehaviour.transform.Translate(newPosition, UnityEngine.Space.World);
                this.playerBehaviour.gameObject.SetActive(true);
            }

            PlayField.AdjustCamera(0, 0);
        }
    }
}
