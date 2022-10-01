using Assets.Scripts.Behaviours.Models;
using Assets.Scripts.Game;

namespace Assets.Scripts.Scenes.PlayField
{
    public class FieldHandler : UnityEngine.MonoBehaviour
    {
        private System.Boolean isLoaded = false;

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

        private void LoadField()
        {
            var fieldGameObject = this.gameObject;
            var fieldsContainer = fieldGameObject.transform.Find("FieldsContainer").gameObject;

            for (int z = 0; z < this.FieldState.RowCount; z++)
            {
                for (int x = 0; x < this.FieldState.ColumnCount; x++)
                {
                    var tile = FieldState.Tiles[z, x];

                    var tileObject = Instantiate(PlayField.GetTemplateByName<TileModelBehaviour>(tile.TemplateReference), fieldsContainer.transform);

                    var xOffset = x * 2;
                    var zOffset = z * 2;

                    tileObject.gameObject.SetActive(true);

                    tileObject.transform.Translate(xOffset, 0, zOffset, UnityEngine.Space.World);

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

            //var playerTemplate = fieldGameObject.transform.Find("Templates/Player/PlayerTemplate2")?.gameObject;

            //if (playerTemplate != default)
            //{
            //    var actualPlayer = Instantiate(playerTemplate, fieldGameObject.transform);

            //    var xOffset = UnityEngine.Random.Range(0, FieldState.ColumnCount) * 2;
            //    var yOffset = UnityEngine.Random.Range(0, FieldState.RowCount) * 2;

            //    var newPosition = new UnityEngine.Vector3(xOffset, 0, yOffset);

            //    actualPlayer.AddComponent<PlayerBehaviour>();

            //    actualPlayer.transform.Translate(newPosition, UnityEngine.Space.World);
            //    actualPlayer.SetActive(true);
            //}
        }

        /*
        
        var gameState = Base.Core.Game?.State;

            if (gameState == default)
            {
                





         */
    }
}
