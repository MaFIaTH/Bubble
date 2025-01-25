using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointManager : MonoBehaviour
{
   public static PointManager instance;
   [field: SerializeField]
   public int Point { get; set ; }
   [SerializeField] TextMeshProUGUI pointText;

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
      else
      {
         Destroy(gameObject);
      }
   }

   private void Update()
   {
      pointText.text = Point.ToString();
   }
}
