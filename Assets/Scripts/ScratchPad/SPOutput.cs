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
        protected SPConnector InConnector
        {
            get
            {
                return InConnectors[0];
            }
            set
            {
                InConnectors[0] = value;
            }
        }

        protected SPOutput() : base()
        {
        }

        protected override void Awake()
        {
            base.Awake();
            InConnectors.AddRange(Enumerable.Repeat<SPConnector>(null, 1));

            // Set up connector
            InConnector = Instantiate(SPInConnectorPrefab, gameObject.transform, false);
            Assert.IsNotNull(InConnector);
            InConnector.gameObject.name = "InConnector";
            InConnector.transform.localPosition = new Vector3(-1, 0, -1);
        }

        // Use this for initialisation
        protected override void Start()
        {
            base.Start();
            //TODO: attach a backend representation to this object
        }

        // Update is called once per frame
        protected override void Update()
        {
        }
    }
}
