using System.Collections.Generic;

using Assets.Scripts.Behaviours.Models;
using Assets.Scripts.Game;

using GameFrame.Core.Extensions;

using Unity.VisualScripting;

using UnityEngine;

namespace Assets.Scripts.Scenes.PlayField
{
    public class FieldHandler : MonoBehaviour
    {
        const System.Int32 rowCount = 9;
        const System.Int32 columnCount = 9;

        private readonly List<ModelBehaviour> templates = new List<ModelBehaviour>();

        public GameObject TileTemplate;


        private void Awake()
        {
            if (GameHandler.AvailableGameModes == default)
            {
                Assets.Scripts.Base.Core.Game.ChangeScene(SceneNames.MainMenu);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            var gameState = Base.Core.Game?.State;

            if (gameState == default)
            {
                var fieldGameObject = this.gameObject;
                var fieldsContainer = fieldGameObject.transform.Find("FieldsContainer").gameObject;

                this.templates.AddRange(fieldGameObject.GetComponentsInChildren<ModelBehaviour>(true));

                for (int x = 0; x < columnCount; x++)
                {
                    for (int z = 0; z < rowCount; z++)
                    {
                        var tile = Instantiate(TileTemplate, fieldsContainer.transform);

                        var xOffset = x * 2;
                        var zOffset = z * 2;

                        tile.SetActive(true);

                        tile.transform.Translate(xOffset, 0, zOffset, Space.World);

                        var extraTemplate = GetRandomTemplate();

                        if (extraTemplate != default)
                        {
                            //var extraObject = Instantiate(extraTemplate.gameObject, fieldsContainer.transform);
                            var extraObject = Instantiate(extraTemplate.gameObject, tile.transform);

                            if (extraTemplate.IsRotatable)
                            {
                                var randomRotation = Random.value * 360;

                                if (randomRotation > 0)
                                {
                                    extraObject.transform.Rotate(0, randomRotation, 0, Space.World);
                                }
                            }

                            //extraObject.transform.Translate(xOffset, 0, zOffset, Space.World);
                            extraObject.SetActive(true);
                        }
                    }
                }

                var playerTemplate = fieldGameObject.transform.Find("Templates/Player/PlayerTemplate2")?.gameObject;

                if (playerTemplate != default)
                {
                    var actualPlayer = Instantiate(playerTemplate, fieldGameObject.transform);

                    var xOffset = Random.Range(0, columnCount) * 2;
                    var yOffset = Random.Range(0, rowCount) * 2;

                    var newPosition = new Vector3(xOffset, 0, yOffset);

                    actualPlayer.AddComponent<PlayerBehaviour>();

                    actualPlayer.transform.Translate(newPosition, Space.World);
                    actualPlayer.SetActive(true);
                }
            }
        }

        private ModelBehaviour GetRandomTemplate()
        {
            if (this.templates?.Count > 0)
            {
                if (Random.value > 0.75)
                {
                    return this.templates.GetRandomEntry();
                }
            }

            return default;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
