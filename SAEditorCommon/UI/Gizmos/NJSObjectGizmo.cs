using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SonicRetro.SAModel.Direct3D;

namespace SonicRetro.SAModel.SAEditorCommon.UI.Gizmos
{
	public class NJSObjectGizmo : TransformGizmo
	{
		private NJS_OBJECT selectedObject;

		public NJS_OBJECT SelectedObject
		{
			get { return selectedObject; }
			set { selectedObject = value; Enabled = selectedObject != null; SetGizmo(); }
		}

		private Vector3 CenterObjects(List<NJS_OBJECT> objectList)
		{
			Vertex[] positionList = new Vertex[objectList.Count];

			for(int vertIndx = 0; vertIndx < objectList.Count; vertIndx++)
			{
				positionList[vertIndx] = objectList[vertIndx].Position;
				if (objectList[vertIndx].Attach != null) positionList[vertIndx] += objectList[vertIndx].Attach.Bounds.Center;
			}

			return Vertex.CenterOfPoints(positionList).ToVector3();
		}

		public override void SetGizmo()
		{
			if (!Enabled) return;

			Vertex gizPosition = new Vertex();

			// todo : we need to do a recursive walk of the model until we get to the selectedObject

			gizPosition = selectedObject.Position;

			if(Pivot == Pivot.CenterOfMass)
			{
				if (selectedObject.Attach != null) gizPosition += selectedObject.Attach.Bounds.Center;
			}

			if (selectedObject != null && LocalTransform)
				GizRotation = selectedObject.Rotation;
			else
				GizRotation = new Rotation();

			Position = gizPosition.ToVector3();
		}

		/// <summary>
		/// Transforms the Items that belong to this Gizmo.
		/// </summary>
		/// <param name="xChange">Input for x axis.</param>
		/// <param name="yChange">Input for y axis.</param>
		public override void TransformAffected(float xChange, float yChange, EditorCamera cam)
		{
			Vector2 input = new Vector2(xChange, yChange);
			// don't operate with an invalid axis seleciton, or invalid mode
			if (!Enabled)
				return;
			if ((SelectedAxes == GizmoSelectedAxes.NONE) || (Mode == TransformMode.NONE))
				return;

			switch (Mode)
			{
				case TransformMode.TRANFORM_MOVE:
					Vector3 Up = new Vector3(), Look = new Vector3(), Right = new Vector3();
					if (LocalTransform)
					{
						SonicRetro.SAModel.Direct3D.Extensions.GetLocalAxes(selectedObject.Rotation, out Up, out Right, out Look);
					}
					else
					{
						Up = new Vector3(0, 1, 0);
						Look = new Vector3(0, 0, 1);
						Right = new Vector3(1, 0, 0);
					}

					Vector3 position = Move(input, selectedObject.Position.ToVector3(), cam, Up, Look, Right);
					selectedObject.Position = position.ToVertex();
					break;

				case TransformMode.TRANSFORM_ROTATE:
					Rotate(input, cam, null, selectedObject.Rotation);
					break;

				case TransformMode.TRANSFORM_SCALE:
					selectedObject.Scale = Scale(input, selectedObject.Scale, cam, false, 0);
					break;
			}

			SetGizmo();

			//LevelData.InvalidateRenderState(); // todo: send some kind of state-change or re-draw signal
		}
	}
}
