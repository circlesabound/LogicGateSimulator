using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.ScratchPad
{
    public class SPTrueConst : SPLogicComponent
    {
        protected SPConnector OutConnector;

        protected SPTrueConst() : base()
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
            LogicComponent = new TrueConst();
        }

        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
