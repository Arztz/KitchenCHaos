using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
  [SerializeField ] private PlatesCounter plateCounter;
  [SerializeField ] private Transform platesVisualPrefab;
 [SerializeField ] private Transform counterTopPoint;
private List<GameObject> plateVisualGameObjectList;
  private void Awake()
  {
      plateVisualGameObjectList = new List<GameObject>();
  }
  private void Start()
  {
      plateCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
      plateCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
  }

  private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
  {
      GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
      plateVisualGameObjectList.Remove(plateGameObject);
      Destroy(plateGameObject);
  }

  private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
  {
    
    Transform plateVisualTransform = Instantiate(platesVisualPrefab,counterTopPoint);
    float plateOffsetY = .1f;
    plateVisualTransform.localPosition= new Vector3(0,plateOffsetY*plateVisualGameObjectList.Count ,0);
    plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
  }


}
