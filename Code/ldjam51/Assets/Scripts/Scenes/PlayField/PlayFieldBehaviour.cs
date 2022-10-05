using System;
using System.Collections.Generic;
using System.Linq;

using Assets.Scripts.Behaviours;
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
        private List<FieldHandler> fieldHandlers = new List<FieldHandler>();

        private TextMeshProUGUI elapsedTimeText;
        private TextMeshProUGUI remainingTimeText;
        private TextMeshProUGUI levelsCompletedText;
        private float remainingTimeFontSize;

        public Camera sceneCamera;
        public GameObject Tutorial;

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
            this.levelsCompletedText = transform.Find("Canvas/LevelsCompletedText/Text")?.GetComponent<TextMeshProUGUI>();
            this.remainingTimeFontSize = remainingTimeText.fontSize;
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

            GenerateFields();

            //FindField(transform.Find("LeftField")?.gameObject, ref leftField, gameState.Field1);
            //FindField(transform.Find("RightField")?.gameObject, ref rightField, gameState.Field2);

            if (this.levelsCompletedText != default)
            {
                this.levelsCompletedText.text = gameState.LevelsCompleted.ToString();
            }

            if (Assets.Scripts.Base.Core.Game.Options.ShowTutorial)
            {
                ShowTutorial();
            }


            AdjustCamera();
        }

        private void GenerateFields()
        {
            var fieldTemplate = transform.Find("Templates/Fields/Field")?.gameObject;
            var fieldsContainer = transform.Find("FieldsContainer")?.gameObject;

            if ((fieldTemplate != default) && (fieldsContainer != default))
            {
                foreach (var field in gameState.Fields)
                {
                    var fieldGameObject = Instantiate(fieldTemplate, fieldsContainer.transform);

                    var fieldHandler = fieldGameObject.GetComponent<FieldHandler>();

                    if (fieldHandler != default)
                    {
                        fieldHandler.LoadNewField(this, field);

                        this.fieldHandlers.Add(fieldHandler);
                    }
                }
            }

            //var fieldOffset = (gameState.Field1.ColumnCount * 2) + 5;

            //leftField.transform.position = Vector3.zero;

            //rightField.transform.position = Vector3.zero;
            //rightField.transform.Translate(new Vector3(fieldOffset, 0, 0), Space.World);
        }


        private void Update()
        {
            if (Time.timeScale > 0)
            {
                gameState.ElapsedTime += Time.deltaTime;
                gameState.TimeRemaining -= Time.deltaTime;

                if (gameState.TimeRemaining < 0)
                {
                    //Scripts.Base.Core.Game.EffectsAudioManager.Play("Horn");

                    gameState.TimeRemaining = gameState.Mode.Interval;
                    gameState.NextTick = gameState.Mode.TickStart;
                    gameState.LastTick = 0;

                    ToggleFields();
                }

                if (gameState.TimeRemaining < gameState.NextTick)
                {
                    gameState.NextTick -= gameState.Mode.TickInterval;

                    if (gameState.LastTick == 1)
                    {
                        gameState.LastTick = 0;
                        Scripts.Base.Core.Game.EffectsAudioManager.Play("Tock");
                    }
                    else
                    {
                        gameState.LastTick = 1;
                        Scripts.Base.Core.Game.EffectsAudioManager.Play("Tick");
                    }
                }

                if (!gameState.Fields.Any(f => f.IsCompleted))
                {
                    gameState.LevelsCompleted += 1;

                    if (this.levelsCompletedText != default)
                    {
                        this.levelsCompletedText.text = gameState.LevelsCompleted.ToString();
                    }

                    Time.timeScale = 0;
                    LoadNewFields();
                    Time.timeScale = 1;
                }

                UpdateElapsed();
                UpdateRemaining();
            }
        }

        public void ShowTutorial()
        {
            Tutorial.SetActive(true);
            Time.timeScale = 0;
        }

        public void CloseTutorial()
        {
            Tutorial.SetActive(false);
            Assets.Scripts.Base.Core.Game.Options.ShowTutorial = false;
            Time.timeScale = 1;
        }

        private void LoadNewFields()
        {
            
            gameState.Field1 = Base.Core.Game.GenerateField(gameState.Mode, false);
            gameState.Field2 = Base.Core.Game.GenerateField(gameState.Mode, true);

            gameState.TimeRemaining = gameState.Mode.Interval;
            gameState.NextTick = gameState.Mode.TickStart;
            gameState.ToggleIndex = 0;

            leftField.LoadNewField(this, gameState.Field1);
            rightField.LoadNewField(this, gameState.Field2);
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
            Bounds b = GetBounds(this.gameObject);

            float cameraDistance = .25f; // Constant factor
            Vector3 objectSizes = b.max - b.min;

            float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);

            float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * sceneCamera.fieldOfView); // Visible height 1 meter in front
            float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
            distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
            sceneCamera.transform.position = b.center - distance * sceneCamera.transform.forward;
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
                this.remainingTimeText.fontSize = remainingTimeFontSize * (10 - gameState.TimeRemaining) / 5 + remainingTimeFontSize;
            }
        }

        private Boolean FindField(GameObject fieldObject, ref FieldHandler fieldHandlerValue, FieldState fieldState)
        {
            if (fieldObject != null)
            {
                var fieldHandler = fieldObject.GetComponent<FieldHandler>();

                if (fieldHandler != default)
                {
                    fieldHandler.LoadNewField(this, fieldState);

                    fieldHandlerValue = fieldHandler;

                    return true;
                }
            }

            return default;
        }

        private void ToggleFields()
        {
            if (gameState.ToggleIndex == 0)
            {
                var newToggleState = 1;

                foreach (var fieldHandler in this.fieldHandlers)
                {
                    var fieldWillBeActive = true;

                    if (fieldHandler.FieldState.IsCompleted)
                    {
                        newToggleState++;
                        fieldWillBeActive = false;
                    }

                    fieldHandler.SetActive(fieldWillBeActive);
                }

                if (this.fieldHandlers[0].FieldState.IsCompleted)
                {
                    newToggleState = 2;
                }

                if (gameState.Field1.IsCompleted)
                {
                    newToggleState = 2;
                    firstFieldWillBeActive = false;
                }

                gameState.ToggleIndex = newToggleState;

                activeField = leftField;

                leftField.SetActive(firstFieldWillBeActive);
                rightField.SetActive(!firstFieldWillBeActive);
            }
            else
            {
                var field1Active = false;

                if (gameState.ToggleIndex == 2)
                {
                    field1Active = true;
                    gameState.ToggleIndex = 1;
                }
                else
                {
                    gameState.ToggleIndex = 2;
                }

                SetFieldActive(leftField, field1Active);
                SetFieldActive(rightField, !field1Active);
            }
        }

        private void SetFieldActive(FieldHandler field, Boolean isActive)
        {
            if (!field.FieldState.IsCompleted)
            {
                var newIsActive = isActive;

                field.SetActive(newIsActive);

                if (newIsActive)
                {
                    this.activeField = field;
                }
            }
        }

        internal static Bounds GetBounds(GameObject gameObject)
        {
            Bounds b = new Bounds(gameObject.transform.position, Vector3.zero);

            b = GetBoundRec(gameObject.transform, b);

            return b;
        }

        internal static Bounds GetBoundRec(Transform goT, Bounds b)
        {
            var boundsBehaviour = goT.GetComponent<BoundsCalculationBehaviour>();

            if (boundsBehaviour == default || boundsBehaviour.IsIncluded)
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
            }

            return b;
        }
    }
}
