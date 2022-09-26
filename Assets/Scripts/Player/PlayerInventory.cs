using System.Collections;
using System.Collections.Generic;
using Singleton;
using Sirenix.OdinInspector;
using Tools;
using UnityEngine;

public class PlayerInventory : SingletonBehaviour<PlayerInventory>
{

  [Title("Current item")] public Tool currentTool;
  
  [Title("Tools")] public List<Tool> tools;

  
  



}
