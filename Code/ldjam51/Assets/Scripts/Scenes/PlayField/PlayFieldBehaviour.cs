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

        internal static Bounds GetBound(GameObject go)
        {

            Bounds b = new Bounds(go.transform.position, Vector3.zero);
            b = GetBoundRec(go.transform, b);
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

        public void AdjustCamera(float width, float height)
        {
            //var halfWit = (width / 2f);

            //sceneCamera.transform.position = new Vector3(halfWit, halfWit * 1.25f, -(halfWit / 6f));
            Bounds b = GetBound(this.gameObject);

            //Debug.Log(b);
            //Vector3 max = b.size;
            //float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));

            //float dist = radius / (Mathf.Tan(sceneCamera.fieldOfView * Mathf.Deg2Rad / 2f));
            //Debug.Log("Radius = " + radius + " dist = " + dist);


            //Vector3 view_direction = sceneCamera.transform.TransformDirection(Vector3.forward);

            //Vector3 pos = b.center - view_direction * dist / 2 ;
            //sceneCamera.transform.position = pos;

            float cameraDistance = .25f; // Constant factor
            Vector3 objectSizes = b.max - b.min;

            float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);

            float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * sceneCamera.fieldOfView); // Visible height 1 meter in front
            float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
            distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object
            sceneCamera.transform.position = b.center - distance * sceneCamera.transform.forward;

            //sceneCamera.transform.LookAt(b.center);
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
