using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public class SPOutput : SPLogicComponent
    {
        protected SPConnector InConnector;

        protected SPOutput() : base()
        {
        }

        protected override void Awake()
        {
            // Set up connector
            InConnector = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnector);
            InConnector.gameObject.name = "InConnector";
            InConnector.transform.localPosition = new Vector3(-1, 0, -1);
        }

        // Use this for initialisation
        protected override void Start()
        {
            //TODO: attach a backend representation to this object
        }

        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
