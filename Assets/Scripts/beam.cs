using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beam : MonoBehaviour
{
    private GameObject enemyBeam;
    bool scaling = false;

    IEnumerator scaleOverTime(Transform objectToScale, Vector3 toScaleUp, float duration)
    {
        //Make sure there is only one instance of this function running
        if (scaling)
        {
            yield break; ///exit if this is still running
        }
        scaling = true;

        //Get the current scale of the object to be moved
        Vector3 startScaleSize = objectToScale.localScale;
        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToScale.localScale = Vector3.Lerp(startScaleSize, toScaleUp, counter / duration);
            yield return null;
        }
    
        scaling = false;
        Destroy(enemyBeam);
    }

    // Awake is called before Start once
    void Awake() => enemyBeam = this.gameObject;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 scaleUp = new Vector3(enemyBeam.transform.localScale.x * 7, enemyBeam.transform.localScale.y * 20, 0);

        StartCoroutine(scaleOverTime(enemyBeam.transform, scaleUp, 3f));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
