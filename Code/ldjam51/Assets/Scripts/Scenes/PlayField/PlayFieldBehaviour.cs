using System;
using System.Collections.Generic;

using Assets.Scripts.Behaviours.Models;
using Assets.Scripts.Core;
using Assets.Scripts.Game;

using TMPro;

using UnityEngine;

namespace Assets.Scripts.Scenes.PlayField
{
    public class PlayFieldBehaviour : MonoBehaviour
    {
        private readonly Dictionary<String, ModelBehaviour> templatesCache = new Dictionary<String, ModelBehaviour>();

        private GameState gameState;
        private FieldHandler activeField;

        private FieldHandler leftField;
        private FieldHandler rightField;
        private TextMeshProUGUI elapsedTimeText;
        private TextMeshProUGUI remainingTimeText;
        private float remainingTimeFontSize;

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

            this.elapsedTimeText = transform.Find("Canvas/ElapsedConatiner/Text")?.GetComponent<TextMeshProUGUI>();
            this.remainingTimeText = transform.Find("Canvas/RemainingContainer/Text")?.GetComponent<TextMeshProUGUI>();
            remainingTimeFontSize = remainingTimeText.fontSize;
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

            AdjustCamera();
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

        public void MovePlayerRight()
        {
            this.activeField?.playerBehaviour?.MoveRight();
        }

        public void MovePlayerDown()
        {
            this.activeField?.playerBehaviour?.MoveDown();
        }

        public void MovePlayerLeft()
        {
            this.activeField?.playerBehaviour?.MoveLeft();
        }

        public void MovePlayerUp()
        {
            this.activeField?.playerBehaviour?.MoveUp();
        }

        public void AdjustCamera()
        {
            Bounds b = GetBound(this.gameObject);

            float cameraDistance = .25f; // Constant factor
            Vector3 objectSizes = b.max - b.min;

            float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);

            float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * sceneCamera.fieldOfView); // Visible height 1 meter in front
            float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
            distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
            sceneCamera.transform.position = b.center - distance * sceneCamera.transform.forward;
        }

        private void Update()
        {
            if (Time.timeScale > 0)
            {
                gameState.ElapsedTime += Time.deltaTime;
                gameState.TimeRemaining -= Time.deltaTime;

                if (gameState.TimeRemaining < 0)
                {
                    gameState.TimeRemaining = 10;

                    ToggleFields();
                }

                if (gameState.Field1.IsCompleted && gameState.Field2.IsCompleted)
                {
                    gameState.LevelsCompleted += 1;
                }

                UpdateElapsed();
                UpdateRemaining();
            }
        }

        private void UpdateElapsed()
        {
            if (this.elapsedTimeText != default)
            {
                this.elapsedTimeText.text = gameState.ElapsedTime.ToString("###0.0s");
            }
        }

        private void UpdateRemaining()
        {
            if (this.remainingTimeText != default)
            {
                this.remainingTimeText.text = gameState.TimeRemaining.ToString("#0.0");
                this.remainingTimeText.fontSize = remainingTimeFontSize * (10 - remainingTimeFontSize) / 10 + remainingTimeFontSize;
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
            if (gameState.ElapsedTime > 20)
            {
                SetFieldActive(leftField);
                SetFieldActive(rightField);
            }
            else if (gameState.ElapsedTime > 10)
            {
                activeField = leftField;

                leftField.SetActive(true);
                rightField.SetActive(false);
            }
        }

        private void SetFieldActive(FieldHandler field)
        {
            if (!field.FieldState.IsCompleted)
            {
                field.SetActive(!field.FieldState.IsActive);
            }
        }

        internal static Bounds GetBound(GameObject gameObject)
        {
            Bounds b = new Bounds(gameObject.transform.position, Vector3.zero);

            b = GetBoundRec(gameObject.transform, b);

            return b;
        }

        internal static Bounds GetBoundRec(Transform goT, Bounds b)
        {
            foreach (Transform child in goT)
            {
                if (child.gameObject.activeSelf)
                {
                    b = GetBoundRec(child, b);

                    Renderer r = child.GetComponent<MeshRenderer>();

                    if (r != default)
                    {
                        b.Encapsulate(r.bounds);
                        //Debug.Log("dodi", child);
                    }
                    else
                    {
                        //Debug.Log("No render:", child);
                    }
                }
            }

            return b;
        }

    }
}
