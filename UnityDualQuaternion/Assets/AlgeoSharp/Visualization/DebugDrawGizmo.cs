using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AlgeoSharp.Visualization
{
    public class DebugDrawGizmo : MonoBehaviour
	{

        #region Public methods

        public void Add(AlgeoObject item)
		{
			AlgeoObjects.Add(item);
		}

		public AlgeoObject Add(MultiVector value, Color color)
		{
			AlgeoObject ret = new AlgeoObject(value, color);
			Add(ret);
			return ret;
		}

		public void Remove(AlgeoObject item)
		{
			AlgeoObjects.Remove(item);
		}

        #endregion

        #region Private methods

        void Render()
        {
			foreach (AlgeoObject obj in AlgeoObjects)
			{
				switch (IPNS.GetGeometricEntity(obj.Value))
				{
					case GeometricEntity.Vector:
						drawVector(obj.Value, obj.Color);
						break;

					case GeometricEntity.Point:
						drawPoint(obj.Value, obj.Color);
						break;

					case GeometricEntity.Sphere:
						drawSphere(obj.Value, obj.Color);
						break;

					case GeometricEntity.Plane:
						drawPlane(obj.Value, obj.Color);
						break;

					case GeometricEntity.Line:
						drawLine(obj.Value, obj.Color);
						break;

					case GeometricEntity.Circle:
						drawCircle(obj.Value, obj.Color);
						break;

					case GeometricEntity.PointPair:
						drawPointPair(obj.Value, obj.Color);
						break;

					default:
						break;
				}
			}
		}

		void drawVector(MultiVector vector, Color color)
		{
			Gizmos.color = color;

			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, vector);
			float rotAngle = Mathf.Acos(vector.E3 / vector.Length);

			if (rotAxis == MultiVector.Zero)
			{
				rotAxis = Basis.E1;
				rotAngle = 0f;
			}

			float length = vector.Length;
			Vector3 direction = new Vector3(rotAngle * Mathf.Rad2Deg, rotAngle * Mathf.Rad2Deg, rotAngle * Mathf.Rad2Deg);
			Quaternion rot = Quaternion.AngleAxis(Mathf.Rad2Deg * rotAngle, new Vector3(rotAxis.E1, rotAxis.E2, rotAxis.E3));
			Matrix4x4 rotMat = Matrix4x4.Rotate(rot);

			Gizmos.matrix = rotMat;
			Gizmos.DrawRay(transform.position, direction * length);
			Gizmos.matrix = Matrix4x4.identity;
		}

		void drawPoint(MultiVector point, Color color)
		{
			MultiVector x;
			IPNS.GetPointParams(point, out x);
			Gizmos.color = color;
			Gizmos.DrawSphere(new Vector3(x.E1, x.E2, x.E3), 0.2f);
		}

		void drawSphere(MultiVector sphere, Color color)
		{
			MultiVector c;
			float r;
			IPNS.GetSphereParams(sphere, out c, out r);
			Gizmos.color = color;
			Gizmos.DrawWireSphere(new Vector3(c.E1, c.E2, c.E3), r);
		}

		void drawPlane(MultiVector plane, Color color)
		{
			MultiVector n;
			float d;

			IPNS.GetPlaneParams(plane, out n, out d);

			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, n);
			float rotAngle = Mathf.Acos(n.E3);

			if (rotAxis == MultiVector.Zero)
			{
				rotAxis = Basis.E1;
				rotAngle = 0f;
			}

			Gizmos.color = color;

			Quaternion rot = Quaternion.AngleAxis(Mathf.Rad2Deg * rotAngle, new Vector3(rotAxis.E1, rotAxis.E2, rotAxis.E3));
			Matrix4x4 rotMat = Matrix4x4.Rotate(rot);
			Gizmos.matrix = rotMat;
			Gizmos.DrawWireCube(new Vector3(n.E1 * d, n.E2 * d, n.E3 * d), new Vector3(10f, 10f, 0f));
			Gizmos.matrix = Matrix4x4.identity;
		}

		void drawLine(MultiVector line, Color color)
		{
			MultiVector t, d;

			IPNS.GetLineParams(line, out t, out d);

			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, d);
			float rotAngle = Mathf.Acos(d.E3);

			if (rotAxis == MultiVector.Zero)
			{
				rotAxis = Basis.E1;
				rotAngle = 0f;
			}

			Gizmos.color = color;

			Quaternion rot = Quaternion.AngleAxis(Mathf.Rad2Deg * rotAngle, new Vector3(rotAxis.E1, rotAxis.E2, rotAxis.E3));
			Matrix4x4 rotMat = Matrix4x4.Rotate(rot);

			Gizmos.matrix = rotMat;
			Gizmos.DrawLine(new Vector3(t.E1, t.E2, t.E3 - 10f), new Vector3(t.E1, t.E2, t.E3 + 10f));
			Gizmos.matrix = Matrix4x4.identity;
		}

		void drawCircle(MultiVector circle, Color color)
		{
			MultiVector n, c;
			float r;

			IPNS.GetCircleParams(circle, out n, out c, out r);

			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, n);
			float rotAngle = Mathf.Acos(n.E3);

			if (rotAxis == MultiVector.Zero)
			{
				rotAxis = Basis.E1;
				rotAngle = 0f;
			}

			Gizmos.color = color;

			Quaternion rot = Quaternion.AngleAxis(Mathf.Rad2Deg * rotAngle, new Vector3(rotAxis.E1, rotAxis.E2, rotAxis.E3));
			Gizmos.matrix = Matrix4x4.TRS(new Vector3(c.E1, c.E2, c.E3), rot, new Vector3(1, 1, 0));
			Gizmos.DrawWireSphere(Vector3.zero, r);
			Gizmos.matrix = Matrix4x4.identity;
		}

		void drawPointPair(MultiVector pointPair, Color color)
		{
			MultiVector p1, p2;

			IPNS.GetPointPairParams(pointPair, out p1, out p2);

			drawPoint(p1, color);
			drawPoint(p2, color);
		}

		#endregion

		#region Editor callbacks

		void Awake()
        {
			AlgeoObjects = new List<AlgeoObject>();
        }

        void OnDrawGizmos()
        {
			if (AlgeoObjects != null)
				Render();
        }

        #endregion

        #region Private variables

        List<AlgeoObject> AlgeoObjects;

        #endregion
    }
}
