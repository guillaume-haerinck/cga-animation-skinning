using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://docs.unity3d.com/ScriptReference/Gizmos.html

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
			/*
			GL.Color3(color);

			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, vector);
			double rotAngle = Math.Acos(vector.E3 / vector.Length); ;

			if (rotAxis == MultiVector.Zero)
			{
				rotAxis = Basis.E1;
				rotAngle = 0.0;
			}


			double length = vector.Length;

			GL.PushMatrix();

			GL.Rotate(MathHelper.RadiansToDegrees(rotAngle), rotAxis.E1, rotAxis.E2, rotAxis.E3);
			GL.Scale(length, length, length);

			drawModel(vectorLineVbo);

			GL.PopMatrix();

			GL.PushMatrix();

			GL.Translate(vector.E1, vector.E2, vector.E3);
			GL.Rotate(MathHelper.RadiansToDegrees(rotAngle), rotAxis.E1, rotAxis.E2, rotAxis.E3);

			drawModel(vectorArrowVbo);

			GL.PopMatrix();
			*/
		}

		void drawPoint(MultiVector point, Color color)
		{
			MultiVector x;

			IPNS.GetPointParams(point, out x);

			/*
			GL.Color3(color);

			GL.PushMatrix();

			GL.Translate(x.E1, x.E2, x.E3);
			GL.Scale(PointRadius, PointRadius, PointRadius);

			drawModel(sphereVbo);

			GL.PopMatrix();
			*/
		}

		void drawSphere(MultiVector sphere, Color color)
		{
			MultiVector c;
			double r;

			IPNS.GetSphereParams(sphere, out c, out r);

			/*
			GL.Color3(color);

			GL.PushMatrix();

			GL.Translate(c.E1, c.E2, c.E3);
			GL.Scale(r, r, r);

			drawModel(sphereVbo);

			GL.PopMatrix();
			*/
		}

		void drawPlane(MultiVector plane, Color color)
		{
			MultiVector n;
			double d;

			IPNS.GetPlaneParams(plane, out n, out d);

			/*
			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, n);
			double rotAngle = Math.Acos(n.E3);

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
			double rotAngle = Math.Acos(d.E3);

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
			double r;

			IPNS.GetCircleParams(circle, out n, out c, out r);

			/*
			MultiVector rotAxis = MultiVector.CrossProduct(Basis.E3, n);
			double rotAngle = Math.Acos(n.E3);

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

		void Start()
        {
			AlgeoObjects = new List<AlgeoObject>();
        }

        void Update()
        {
			// Render();
        }

        #endregion

        #region Private variables

        List<AlgeoObject> AlgeoObjects;

        #endregion
    }
}
