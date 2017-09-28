using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public class SPFalseConst : SPLogicComponent
    {
        protected SPConnector OutConnector;

        protected SPFalseConst() : base()
        {
        }

        protected override void Awake()
        {
            // Set up connectors
            OutConnector = Instantiate(SPOutConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(OutConnector);
            OutConnector.gameObject.name = "OutConnector";
            OutConnector.transform.localPosition = new Vector3(1, 0, -1);
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
