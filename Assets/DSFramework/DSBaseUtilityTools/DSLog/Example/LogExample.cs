using System;
using UnityEngine;

namespace DSFramework.Example
{
    public class LogExample : MonoBehaviour
    {
        private void Start()
        {
            DSLog.I("Hello,UnitySir");
            DSLog.E("Hello,UnitySir");
            
        }
    }
}