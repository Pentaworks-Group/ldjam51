using System;
using System.Collections.Generic;

using Assets.Scripts.Behaviours.Models;
using Assets.Scripts.Core;
using Assets.Scripts.Game;

using UnityEngine;

namespace Assets.Scripts.Scenes.PlayField
{
    public class PlayFieldBehaviour : MonoBehaviour
    {
        private readonly Dictionary<String, ModelBehaviour> templatesCache = new Dictionary<String, ModelBehaviour>();

        private FieldHandler leftField;
        private FieldHandler rightField;
        private GameState gameState;

        public Camera sceneCamera;

        private void Awake()
        {
            if (GameHandler.AvailableGameModes == default)
            {
                Assets.Scripts.Base.Core.Game.ChangeScene(SceneNames.MainMenu);
            }
        }

        private void Start()
        {
            this.gameState = Base.Core.Game.State;

            var templateConatiner = transform.Find("Templates");

            if (templateConatiner != default)
            {
                var modelTemplates = templateConatiner.GetComponentsInChildren<ModelBehaviour>(true);

                if (modelTemplates?.Length > 0)
                {
                    foreach (var modelTemplate in modelTemplates)
                    {
                        modelTemplate.gameObject.transform.position = Vector3.zero;
                        modelTemplate.gameObject.transform.localPosition = Vector3.zero;

                        this.templatesCache[modelTemplate.name] = modelTemplate;
                    }
                }
                else
                {
                    throw new Exception("Missing Templates!");
                }
            }

            FindField(transform.Find("LeftField")?.gameObject, ref leftField, gameState.Field1);
            FindField(transform.Find("RightField")?.gameObject, ref rightField, gameState.Field2);

            rightField.gameObject.transform.position = new Vector3(gameState.Field1.ColumnCount * 2 + 5, 0, 0);

            var width = (gameState.Field1.ColumnCount + gameState.Field2.ColumnCount) * 2 + 5;
            var height = (Math.Max(gameState.Field1.RowCount, gameState.Field2.RowCount) * 2);

            AdjustCamera(width, height);
        }

        public T GetTemplateByName<T>(String templateName) where T : ModelBehaviour
        {
            if (this.templatesCache.TryGetValue(templateName, out ModelBehaviour template))
            {
                if (template is T castedTemplate)
                {
                    return castedTemplate;
                }
            }

            throw new Exception($"No model template found for name '{templateName}'");
        }

        private void Update()
        {
            if (Time.timeScale > 0)
            {
                gameState.TimeRemaining -= Time.deltaTime;

                if (gameState.TimeRemaining < 0)
                {
                    gameState.TimeRemaining = 10;

                    ToggleFields();
                }
            }
        }

        private Boolean FindField(GameObject fieldObject, ref FieldHandler fieldHandlerValue, FieldState fieldState)
        {
            if (fieldObject != null)
            {
                var fieldHandler = fieldObject.GetComponent<FieldHandler>();

                if (fieldHandler != default)
                {
                    fieldHandler.PlayField = this;
                    fieldHandler.FieldState = fieldState;
                    fieldHandlerValue = fieldHandler;

                    return true;
                }
            }

            return default;
        }

        private void ToggleFields()
        {
            leftField.SetActive(!leftField.FieldState.IsActive);
            rightField.SetActive(!rightField.FieldState.IsActive);
        }

        private void AdjustCamera(float width, float height)
        {
            var halfWit = (width / 2f);

            sceneCamera.transform.position = new Vector3(halfWit, halfWit * 1.25f, -(halfWit / 6f));

            //var halfWit = (width / 2f);

            //var alpha = (90 - sceneCamera.transform.eulerAngles.x);
            //var alphaRad = alpha * Mathf.Deg2Rad;

            //var beta = (sceneCamera.fieldOfView / 2f);
            //var betaRad = beta * Mathf.Deg2Rad;

            //var gamma = 90 - alpha;
            //var gammaRad = gamma * Mathf.Deg2Rad;


            //var q = halfWit / Mathf.Tan(betaRad);
            //var yotaRad = Mathf.Asin(halfWit / q);
            //var yota = yotaRad*Mathf.Rad2Deg;

            //var roh = 180 - gamma - yota;
            //var rohRad = roh*Mathf.Deg2Rad;

            //var l = (halfWit / Mathf.Tan(betaRad)) * (Mathf.Sin(rohRad) / Mathf.Sin(gammaRad));
            //var h = Mathf.Cos(alphaRad) * l;
            //var d = Mathf.Sin(alphaRad) * l;

            //var dActual = d - halfWit;

            //Debug.Log(l);
            //Debug.Log(h);
            //Debug.Log(dActual);

            //sceneCamera.transform.position = new Vector3(halfWit, h, 0f - dActual);
        }
    }
}
