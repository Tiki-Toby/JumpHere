using ECM.Controllers;
using HairyEngine.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Obstacles
{
    class TriggerCapture : MonoBehaviour
	{
		public List<Rigidbody> rigidbodiesInTriggerArea = new List<Rigidbody>();
		void OnTriggerEnter(Collider col)
		{
			if (col.attachedRigidbody != null && col.GetComponent<RunnerController>() != null)
			{
				rigidbodiesInTriggerArea.Add(col.attachedRigidbody);
			}
		}

		void OnTriggerExit(Collider col)
		{
			if (col.attachedRigidbody != null && col.GetComponent<RunnerController>() != null)
			{
				rigidbodiesInTriggerArea.Remove(col.attachedRigidbody);
			}
		}
	}
}
