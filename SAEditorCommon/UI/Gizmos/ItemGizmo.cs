using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using SonicRetro.SAModel.Direct3D;
using SonicRetro.SAModel.SAEditorCommon.DataTypes;

namespace SonicRetro.SAModel.SAEditorCommon.UI.Gizmos
{
	/// <summary>
	/// Version of TransformGizmo meant for transforming the Item class (LevelItem, CAMItem, SETITem, etc.)
	/// </summary>
	public class ItemGizmo : TransformGizmo
	{
		private List<Item> affectedItems; // these are the items that will be affected by any transforms we are given.

		public List<Item> AffectedItems
		{
			get { return affectedItems; }
			set
			{
				affectedItems = value;
				SetGizmo();
			}
		}

		public override void SetGizmo()
		{
			if (!Enabled)
				return;

			try
			{
				if (Pivot == Pivot.CenterOfMass)
				{
					Position = Item.CenterFromSelection(affectedItems).ToVector3();
				}
				else
				{
					Position = (affectedItems.Count > 0) ? affectedItems[0].Position.ToVector3() : new Vector3();
				}

				if ((affectedItems.Count == 1) && LocalTransform)
					GizRotation = new Rotation(affectedItems[0].Rotation.X, affectedItems[0].Rotation.Y, affectedItems[0].Rotation.Z);
				else
					GizRotation = new Rotation();

				Enabled = affectedItems.Count > 0;
			}
			catch (NotSupportedException)
			{
				Console.WriteLine("Certain Item types don't support rotations. This can be ignored.");
			}
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

			for (int i = 0; i < affectedItems.Count; i++) // loop through operands
			{
				Item currentItem = affectedItems[i];
				switch (Mode)
				{
					case TransformMode.TRANFORM_MOVE:
						Vector3 Up = new Vector3(), Look = new Vector3(), Right = new Vector3();
						if (LocalTransform)
						{
							SonicRetro.SAModel.Direct3D.Extensions.GetLocalAxes(affectedItems[i].Rotation, out Up, out Right, out Look);
						}
						else
						{
							Up = new Vector3(0, 1, 0);
							Look = new Vector3(0, 0, 1);
							Right = new Vector3(1, 0, 0);
						}

						Vector3 position = Move(input, currentItem.Position.ToVector3(), cam, Up, Look, Right);
						currentItem.Position = position.ToVertex();
						break;

					case TransformMode.TRANSFORM_ROTATE:
						Rotate(input, cam, null, currentItem.Rotation);
						break;

					case TransformMode.TRANSFORM_SCALE:
						if (currentItem is LevelItem)
						{
							LevelItem levelItem = (LevelItem)currentItem;
							levelItem.CollisionData.Model.Scale = Scale(input, levelItem.CollisionData.Model.Scale, cam, false, 0);
						}
						else if (currentItem is CAMItem)
						{
							CAMItem camItem = (CAMItem)currentItem;
							camItem.Scale = Scale(input, camItem.Scale, cam, true, 1);
						}
						break;
				}
			}

			SetGizmo();

			LevelData.InvalidateRenderState();
		}
	}
}
