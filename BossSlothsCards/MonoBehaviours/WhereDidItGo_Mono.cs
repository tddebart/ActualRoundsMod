using System.Diagnostics;
using UnboundLib;
using UnityEngine;

namespace BossSlothsCards.MonoBehaviours
{
    public class WhereDidItGo_Mono : MonoBehaviour
    {
        private static readonly System.Random rng = new System.Random();
        public void RemoveRandomObject(SpriteRenderer[] objects)
        {
            var rID = rng.Next(0, objects.Length);
            
            while (true)
            {
                UnityEngine.Debug.LogWarning("looping");
                rID = rng.Next(0, objects.Length);
                if (objects[rID] == null) continue;
                var randomObject = objects[rID].gameObject;
                UnityEngine.Debug.LogWarning("got object: " + randomObject.name);
                if (Condition(randomObject))
                {
                    UnityEngine.Debug.LogWarning("found");
                    UnityEngine.Debug.LogWarning(randomObject.name);
                    var pieces = BossSlothCards.EffectAsset.LoadAsset<GameObject>("Pieces");
                    var _pieces = Instantiate(pieces, randomObject.transform.parent);
                    _pieces.transform.position = randomObject.transform.position;
                    _pieces.transform.rotation = randomObject.transform.rotation;
                    randomObject.SetActive(false);
                    this.ExecuteAfterSeconds(6, () =>
                    {
                        Destroy(_pieces);
                    });
                }
                else
                {
                    UnityEngine.Debug.LogWarning("next loop");
                    continue;
                }
                //#TODO make a effect when it disapears by making a cube with animation
                break;
            }
        }

        private static bool Condition(GameObject obj)
        {
            return obj.GetComponent<SpriteRenderer>() && !obj.name.Contains("Color") && !obj.name.Contains("Lines");
        }
    }
}