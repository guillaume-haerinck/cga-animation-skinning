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
			/*
			Vector3 direction = rotAngle * Mathf.Rad2Deg;
			GL.Rotate(MathHelper.RadiansToDegrees(rotAngle), rotAxis.E1, rotAxis.E2, rotAxis.E3);
			*/
			//Gizmos.DrawRay(transform.position, direction);
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

			/*
			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, n);
			float rotAngle = Math.Acos(n.E3);

			if (rotAxis == MultiVector.Zero)
			{
				rotAxis = Basis.E1;
				rotAngle = 0.0;
			}

			GL.Color3(color);

			GL.PushMatrix();

			GL.Translate(n.E1 * d, n.E2 * d, n.E3 * d);
			GL.Rotate(MathHelper.RadiansToDegrees(rotAngle), rotAxis.E1, rotAxis.E2, rotAxis.E3);

			drawModel(planeVbo);

			GL.PopMatrix();
			*/
		}

		void drawLine(MultiVector line, Color color)
		{
			MultiVector t, d;

			IPNS.GetLineParams(line, out t, out d);

			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, d);
			/*
			float rotAngle = Math.Acos(d.E3);

			if (rotAxis == MultiVector.Zero)
			{
				rotAxis = Basis.E1;
				rotAngle = 0.0;
			}

			GL.Color3(color);

			GL.PushMatrix();

			GL.Translate(t.E1, t.E2, t.E3);
			GL.Rotate(MathHelper.RadiansToDegrees(rotAngle), rotAxis.E1, rotAxis.E2, rotAxis.E3);

			drawModel(lineVbo);

			GL.PopMatrix();
			*/
		}

		void drawCircle(MultiVector circle, Color color)
		{
			MultiVector n, c;
			float r;

			IPNS.GetCircleParams(circle, out n, out c, out r);

			/*
			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, n);
			float rotAngle = Math.Acos(n.E3);

			if (rotAxis == MultiVector.Zero)
			{
				rotAxis = Basis.E1;
				rotAngle = 0.0;
			}

			GL.Color3(color);

			GL.PushMatrix();

			GL.Translate(c.E1, c.E2, c.E3);
			GL.Rotate(MathHelper.RadiansToDegrees(rotAngle), rotAxis.E1, rotAxis.E2, rotAxis.E3);
			GL.Scale(r, r, r);

			drawModel(circleVbo);

			GL.PopMatrix();
			*/
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
