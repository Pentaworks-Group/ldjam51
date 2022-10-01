using System.Collections.Generic;

using Assets.Scripts.Models;

using GameFrame.Core.Extensions;

using UnityEngine;

public class FieldHandler : MonoBehaviour
{
    public GameObject FieldGameObject;

    public GameObject TileTemplate;
    public List<ModelBehaviour> Templates;

    const System.Int32 rowCount = 9;
    const System.Int32 columnCount = 9;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < columnCount; x++)
        {
            for (int z = 0; z < rowCount; z++)
            {
                var tile = Instantiate(TileTemplate, FieldGameObject.transform);

                var xOffset = x * 2;
                var zOffset = z * 2;

                tile.SetActive(true);

                tile.transform.Translate(xOffset, 0, zOffset, Space.World);

                var extraTemplate = GetRandomTemplate();

                if (extraTemplate != default)
                {
                    var extraObject = Instantiate(extraTemplate.gameObject, FieldGameObject.transform);

                    if (extraTemplate.IsRotatable)
                    {
                        var randomRotation = Random.value * 360;

                        if (randomRotation > 0)
                        {
                            extraObject.transform.Rotate(0, randomRotation, 0, Space.World);
                        }
                    }

                    extraObject.transform.Translate(xOffset, 0, zOffset, Space.World);
                    extraObject.SetActive(true);
                }
            }
        }
    }

    private ModelBehaviour GetRandomTemplate()
    {
        if (this.Templates?.Count > 0)
        {
            if (Random.value > 0.75)
            {
                return this.Templates.GetRandomEntry();
            }
        }

        return default;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
